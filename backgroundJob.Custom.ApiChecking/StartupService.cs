using backgroundJob.Custom.ApiChecking.Database;
using backgroundJob.Infrastructure.Monitor;
using backgroundJob.Infrastructure.Option;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace backgroundJob.Custom.ApiChecking
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
				var apiCheckingContext = scope.ServiceProvider.GetRequiredService<ApiCheckingContext>();
				//apiCheckingContext.Database.EnsureCreated();
#if !DEBUG
				apiCheckingContext.Database.Migrate();
#endif
				// end ensure database is created

				var apiChecking = scope.ServiceProvider
					.GetRequiredService<CustomService>();

				var apiCheckingOptions = new ServiceOption()
				{
					Status = ServiceStatus.New,
					Repeat = ServiceRepeat.None,
				};

				_scheduler.AddService(apiChecking, apiCheckingOptions);
			}

			return Task.CompletedTask;
		}
	}
}
