using backgroundJob.Custom.FileWatcher.Database;
using backgroundJob.Custom.ProcessTracking.Database;
using backgroundJob.Infrastructure.Monitor;
using backgroundJob.Infrastructure.Queue;
using backgroundJob.Infrastructure.Service;

IHost host = Host.CreateDefaultBuilder(args)
	.ConfigureServices((hostContext, services) =>
	{
		var configuration = hostContext.Configuration;
		services.AddSingleton(configuration);

		//services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));
		//services.AddOptions();

		services.AddSingleton<Scheduler>();
		services.AddSingleton<BaseQueue<IScopedService>>();

		services.AddDbContext<ProcessContext>(options => { });
		services.AddScoped<ProcessDatabase>();
		services.AddScoped<backgroundJob.Custom.ProcessTracking.CustomService>();

		services.AddDbContext<FileWatcherContext>(options => { });
		services.AddScoped<FileWatcherDatabase>();
		services.AddScoped<backgroundJob.Custom.FileWatcher.CustomService>();

		services.AddHostedService(sp => sp.GetRequiredService<Scheduler>());
		services.AddHostedService(sp => sp.GetRequiredService<BaseQueue<IScopedService>>());

		services.AddHostedService<backgroundJob.Custom.ProcessTracking.Startup>();
		services.AddHostedService<backgroundJob.Custom.FileWatcher.Startup>();
	})
	.ConfigureLogging(logging =>
	{
		logging.AddConsole();
	})
	.Build();

await host.RunAsync();
