using backgroundJob.Custom.ProcessTracking.Database;
using backgroundJob.Custom.ProcessTracking.Entity;
using backgroundJob.Infrastructure.Option;
using backgroundJob.Infrastructure.View.Process;

namespace backgroundJob.Custom.ProcessTracking.Flows
{
	public class ProcessFlow
	{
		public FlowModel GetModel(ProcessDatabase processDatabase, IEnumerable<ProcessInformation> processes)
		{
			// get processes from system
			IEnumerable<string> items = processes.Select(p => p.ExecutablePath);

			// add to check list if process is exists on database
			var onCheckingRaw = processDatabase.Process
				.Filter(p => items.Contains(p.ExecutablePath));

			var onChecking = onCheckingRaw.Select(p => p.ExecutablePath);
			var onCheckingId = onCheckingRaw.Select(p => p.Id);

			// add to create list if process isn't exists on database
			var onCreating = items.Where(u => !onChecking.Contains(u));

			return new FlowModel()
			{
				Items = items.ToArray(),
				OnChecking = onChecking.ToArray(),
				OnCheckingId = onCheckingId.ToArray(),
				OnCreating = onCreating.ToArray(),
			};
		}

		public async Task CreateProcessesAsync(
			ProcessDatabase processDatabase,
			IEnumerable<ProcessInformation> processes,
			FlowModel model,
			CancellationToken token = default)
		{
			// create a function for transaction
			async Task<bool> func(ProcessDatabase unitOfWork, CancellationToken token = default)
			{
				// get processes from system which is exists in OnCreating list
				// convert and insert into Process table
				var processCreate = processes
					.Where(p => model.OnCreating.Contains(p.ExecutablePath))
					.Select(p => new PTProcess()
					{
						Name = p.Name,
						CommandLine = p.CommandLine,
						ExecutablePath = p.ExecutablePath,
					}).ToArray();

				var addedId = await unitOfWork.Process.AddRangeAsync(processCreate, token);

				// for each created process, create a Time entity
				// insert into Time table
				var currentTime = DateTime.UtcNow;
				var timeCreate = addedId.Select(id => new PTTime()
				{
					ProcessId = id,
					FirstDetect = currentTime,
					LastDetect = currentTime,
				}).ToArray();

				await unitOfWork.Time.AddRangeAsync(timeCreate, token);

				return true;
			}
			// end create a function for transaction

			await processDatabase.RunTransactionAsync(processDatabase, func, token);
		}

		public async Task CheckLastDetectAsync(
			ProcessDatabase processDatabase,
			FlowModel model,
			ServiceOption option,

			CancellationToken token = default)
		{
			// create a function for transaction
			async Task<bool> func(ProcessDatabase unitOfWork, CancellationToken token = default)
			{
				// get time items which is already exists in the database filter by process id
				var topLastDetect = unitOfWork.Time.TopPartitionTracking(g => g.ProcessId, o => o.LastDetect);
				var topLastDetectOnChecking = topLastDetect.Where(t => model.OnCheckingId.Contains(t.ProcessId));

				var currentTime = DateTime.UtcNow;
				var acceptTimeSpan = option.Timed.Interval.Add(TimeSpan.FromSeconds(10));
				var acceptLastDetect = currentTime.Subtract(acceptTimeSpan);
				var timeOnCreate = topLastDetectOnChecking.Where(t => t.LastDetect < acceptLastDetect);
				var timeOnUpdate = topLastDetectOnChecking.Where(t => t.LastDetect >= acceptLastDetect);

				// if last detect is smaller than accept last detect
				// then create a new Time entity
				var timeCreate = timeOnCreate.Select(t => new PTTime()
				{
					ProcessId = t.ProcessId,
					LastDetect = currentTime,
					FirstDetect = currentTime,
				});
				await unitOfWork.Time.AddRangeAsync(timeCreate, token);

				// if last detect is accepted
				// then update the Time entity
				foreach (var time in timeOnUpdate)
				{
					time.LastDetect = currentTime;
				}
				await unitOfWork.Time.UpdateRangeAsync(timeOnUpdate, token);
				return true;
			}
			// end create a function for transaction

			await processDatabase.RunTransactionAsync(processDatabase, func, token);
		}
	}
}
