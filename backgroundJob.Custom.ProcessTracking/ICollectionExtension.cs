using backgroundJob.Custom.ProcessTracking.Database;
using Microsoft.Extensions.DependencyInjection;

namespace backgroundJob.Custom.ProcessTracking
{
	public static class ICollectionExtension
	{
		public static void AddProcessTracking(this IServiceCollection services)
		{
			services.AddDbContext<ProcessContext>(options => { });
			services.AddScoped<ProcessDatabase>();
			services.AddScoped<CustomService>();

			services.AddHostedService<StartupService>();
		}
	}
}
