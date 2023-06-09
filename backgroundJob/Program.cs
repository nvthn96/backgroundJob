using backgroundJob;
using backgroundJob.Custom.ProcessTracking.Database;
using backgroundJob.Infrastructure.Monitor;
using backgroundJob.Infrastructure.Queue;
using backgroundJob.Infrastructure.Service;

IHost host = Host.CreateDefaultBuilder(args)
	.ConfigureServices((hostContext, services) =>
	{
		var configuration = hostContext.Configuration;

		//services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));
		//services.AddOptions();

		services.AddSingleton<Scheduler>();
		services.AddSingleton<BaseQueue<IScopedService>>();
		services.AddSingleton(configuration);

		services.AddScoped<ProcessDatabase>();
		services.AddScoped<backgroundJob.Custom.ProcessTracking.CustomService>();

		services.AddHostedService(sp => sp.GetRequiredService<Scheduler>());
		services.AddHostedService(sp => sp.GetRequiredService<BaseQueue<IScopedService>>());
		services.AddHostedService<Startup>();

		services.AddDbContext<ProcessContext>(options =>
		{
			//options.UseSqlServer(configuration.GetConnectionString("BackgroundJob"));
		});
	})
	.ConfigureLogging(logging =>
	{
		logging.AddConsole();
	})
	.Build();

await host.RunAsync();
