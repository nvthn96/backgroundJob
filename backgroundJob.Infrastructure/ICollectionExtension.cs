using backgroundJob.Infrastructure.Monitor;
using backgroundJob.Infrastructure.Queue;
using backgroundJob.Infrastructure.Service;
using Microsoft.Extensions.DependencyInjection;

namespace backgroundJob.Custom.ApiChecking
{
	public static class ICollectionExtension
	{
		public static void AddInfrastructure(this IServiceCollection services)
		{
			services.AddSingleton<Scheduler>();
			services.AddSingleton<BaseQueue<IScopedService>>();

			services.AddHostedService(sp => sp.GetRequiredService<Scheduler>());
			services.AddHostedService(sp => sp.GetRequiredService<BaseQueue<IScopedService>>());
		}
	}
}
