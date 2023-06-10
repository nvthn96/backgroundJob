using backgroundJob.Custom.ProcessTracking.Database;
using backgroundJob.Custom.ProcessTracking.Flows;
using backgroundJob.Extensions;
using backgroundJob.Infrastructure.Option;
using backgroundJob.Infrastructure.Service;
using backgroundJob.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace backgroundJob.Custom.ProcessTracking
{
	public class CustomService : IScopedService
	{
		private readonly ILogger<CustomService> _logger;
		private readonly ProcessDatabase _processDatabase;
		private readonly IConfiguration _configuration;
		public ServiceOption Option { get; set; } = new();

		private static readonly ProcessFlow _process = new();

		public CustomService(ILogger<CustomService> logger, ProcessDatabase processDatabase, IConfiguration configuration)
		{
			_logger = logger;
			_processDatabase = processDatabase;
			_configuration = configuration;
		}

		public async Task RunAsync(CancellationToken token)
		{
			_logger.LogInformation("Process tracking is running.");

			Option.Status.AddEnum(ServiceStatus.Ready);
			Option.Status.AddEnum(ServiceStatus.Running);

			try
			{
				var processes = ProcessUtility.GetProcessesInformation();
				var model = _process.GetModel(_processDatabase, processes);

				await _process.CreateProcessesAsync(_processDatabase, processes, model, token);
				await _process.CheckLastDetectAsync(_processDatabase, model, Option, token);
				Option.Status.AddEnum(ServiceStatus.Success);
			}
			catch (Exception)
			{
				Option.Status.AddEnum(ServiceStatus.Stopped);
			}
			finally
			{
				_logger.LogInformation("Process tracking is run completed.");
			}
		}
	}
}
