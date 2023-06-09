using backgroundJob.Custom.ProcessTracking.Database;
using backgroundJob.Infrastructure.Monitor;
using backgroundJob.Infrastructure.Option;

namespace backgroundJob
{
	public class Startup : BackgroundService
	{
		private IServiceProvider _services;
		private Scheduler _scheduler;

		public Startup(IServiceProvider services, Scheduler scheduler)
		{
			_services = services;
			_scheduler = scheduler;
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			using (var scope = _services.CreateScope())
			{
				// ensure database is created
				var processContext = scope.ServiceProvider.GetRequiredService<ProcessContext>();
				processContext.Database.EnsureCreated();
				//processContext.Database.Migration();
				// end ensure database is created

				// add process tracking to scheduler
				var processTracking = scope.ServiceProvider
					.GetRequiredService<Custom.ProcessTracking.CustomService>();

				var processTrackingOptions = new ServiceOption()
				{
					Status = ServiceStatus.New,
					Repeat = ServiceRepeat.Second,
					Timed = new TimedOption()
					{
						StartTime = DateTime.UtcNow,
						EndTime = DateTime.UtcNow.AddYears(1),
						Interval = TimeSpan.FromMinutes(1),
					},
				};

				_scheduler.AddService(processTracking, processTrackingOptions);
				// end add process tracking to scheduler
			}

			return Task.CompletedTask;
		}
	}
}
