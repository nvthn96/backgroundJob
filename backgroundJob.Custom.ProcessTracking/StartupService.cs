using backgroundJob.Custom.ProcessTracking.Database;
using backgroundJob.Infrastructure.Monitor;
using backgroundJob.Infrastructure.Option;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace backgroundJob.Custom.ProcessTracking
{
	public class StartupService : BackgroundService
	{
		private readonly IServiceProvider _services;
		private readonly Scheduler _scheduler;

		public StartupService(IServiceProvider services, Scheduler scheduler)
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
				//processContext.Database.EnsureCreated();
#if !DEBUG
				processContext.Database.Migrate();
#endif
				// end ensure database is created

				var processTracking = scope.ServiceProvider
					.GetRequiredService<CustomService>();

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
			}

			return Task.CompletedTask;
		}
	}
}
