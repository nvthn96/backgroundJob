using backgroundJob.Infrastructure.Option;
using backgroundJob.Infrastructure.Queue;
using backgroundJob.Infrastructure.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backgroundJob.Infrastructure.Monitor
{
	public class Scheduler : BackgroundService, IScheduler
	{
		private readonly BaseQueue<IScopedService> _queue;
		private readonly ILogger _logger;
		private IServiceProvider _services;

		private readonly List<TimedItem> _listTimedService = new();
		private readonly List<TimedItem> _listCompleted = new();
		private readonly SchedulerStatus _status = new();

		public Scheduler(ILogger<Scheduler> logger, IServiceProvider services)
		{
			_logger = logger;
			_services = services;

			using (var scope = _services.CreateScope())
			{
				_queue = scope.ServiceProvider.GetRequiredService<BaseQueue<IScopedService>>();
			}
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			using var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));
			try
			{
				while (await timer.WaitForNextTickAsync(stoppingToken))
				{
					var readyItems = GetReadyItems();

					foreach (var item in readyItems)
					{
						await _queue.EnQueue(item.Service, stoppingToken);
						item.SetNextTimeRunning();
					}

					var completedItems = GetCompleletedItems();
					_listCompleted.AddRange(completedItems);
					_listTimedService.RemoveAll(item => completedItems.Contains(item));
				}
			}
			catch (OperationCanceledException)
			{
				_logger.LogInformation("Scheduler is stopping.");
			}
		}

		public void AddService(IScopedService service, ServiceOption options)
		{
			_listTimedService.Add(new TimedItem(service, options));
		}

		public SchedulerStatus GetStatus()
		{
			_status.Tracking = _listTimedService.Count();
			_status.Completed = _listCompleted.Count();

			var queueStatus = _queue.GetStatus();
			_status.EnQueued = queueStatus.EnQueued;
			_status.DeQueued = queueStatus.DeQueued;

			return _status;
		}

		private IEnumerable<TimedItem> GetReadyItems()
		{
			var currentTime = DateTime.UtcNow;
			var readyTimedItems = _listTimedService.Where(item => currentTime >= item.NextTimeRunning);

			return readyTimedItems;
		}

		private IEnumerable<TimedItem> GetCompleletedItems()
		{
			var currentTime = DateTime.UtcNow;
			var completedItems = _listTimedService
				.Where(item => item.Service.Option.Status == ServiceStatus.Completed);

			return completedItems;
		}
	}
}
