using backgroundJob.Custom.ApiChecking;
using backgroundJob.Custom.FileWatcher;
using backgroundJob.Custom.ProcessTracking;

IHost host = Host.CreateDefaultBuilder(args)
	.ConfigureServices((hostContext, services) =>
	{
		var configuration = hostContext.Configuration;
		services.AddSingleton(configuration);

		services.AddInfrastructure();
		services.AddApiChecking();
		services.AddFileWacher();
		services.AddProcessTracking();
	})
	.ConfigureLogging(logging =>
	{
		logging.AddConsole();
	})
	.Build();

await host.RunAsync();
