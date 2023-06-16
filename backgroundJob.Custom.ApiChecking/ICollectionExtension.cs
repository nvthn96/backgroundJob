using backgroundJob.Custom.ApiChecking.Database;
using Microsoft.Extensions.DependencyInjection;

namespace backgroundJob.Custom.ApiChecking
{
	public static class ICollectionExtension
	{
		public static void AddApiChecking(this IServiceCollection services)
		{
			services.AddDbContext<ApiCheckingContext>(options => { });
			services.AddScoped<ApiCheckingDatabase>();
			services.AddScoped<CustomService>();

			services.AddHostedService<StartupService>();
		}
	}
}
