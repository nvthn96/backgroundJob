using backgroundJob.Custom.FileWatcher.Database;
using Microsoft.Extensions.DependencyInjection;

namespace backgroundJob.Custom.FileWatcher
{
	public static class ICollectionExtension
	{
		public static void AddFileWacher(this IServiceCollection services)
		{
			services.AddDbContext<FileWatcherContext>(options => { });
			services.AddScoped<FileWatcherDatabase>();
			services.AddScoped<CustomService>();

			services.AddHostedService<StartupService>();
		}
	}
}
