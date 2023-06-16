using backgroundJob.Custom.FileWatcher.Database;
using backgroundJob.Infrastructure.Monitor;
using backgroundJob.Infrastructure.Option;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace backgroundJob.Custom.FileWatcher
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
				var fileWatcherContext = scope.ServiceProvider.GetRequiredService<FileWatcherContext>();
				//fileWatcherContext.Database.EnsureCreated();
#if !DEBUG
				fileWatcherContext.Database.Migrate();
#endif
				// end ensure database is created

				var fileWatcher = scope.ServiceProvider
					.GetRequiredService<CustomService>();

				var fileWatcherOptions = new ServiceOption()
				{
					Status = ServiceStatus.New,
					Repeat = ServiceRepeat.None,
				};

				_scheduler.AddService(fileWatcher, fileWatcherOptions);
			}

			return Task.CompletedTask;
		}
	}
}
