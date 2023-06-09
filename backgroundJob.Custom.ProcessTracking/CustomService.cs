using backgroundJob.Custom.ProcessTracking.Database;
using backgroundJob.Custom.ProcessTracking.Flows;
using backgroundJob.Infrastructure.Option;
using backgroundJob.Infrastructure.Service;
using backgroundJob.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace backgroundJob.Custom.ProcessTracking
{
	public class CustomService : IScopedService
	{
		protected readonly ILogger<CustomService> _logger;
		protected readonly ProcessDatabase _processDatabase;
		protected readonly IConfiguration _configuration;
		public ServiceOption Option { get; set; } = new();

		private static readonly StatusFlow _status = new();
		private static readonly ProcessFlow _process = new();

		public CustomService(ILogger<CustomService> logger, ProcessDatabase processDatabase, IConfiguration configuration)
		{
			_logger = logger;
			_processDatabase = processDatabase;
			_configuration = configuration;
		}

		public async Task RunAsync(CancellationToken token = default)
		{
			_logger.LogInformation("Process tracking is running.");

			_status.SetStartStatus(Option);

			var processes = ProcessUtility.GetProcessesInformation();
			var model = _process.GetModel(_processDatabase, processes);

			await _process.CreateProcessesAsync(_processDatabase, processes, model, token);
			await _process.CheckLastDetectAsync(_processDatabase, model, Option, token);

			_status.SetEndStatus(Option);

			_logger.LogInformation("Process tracking is run completed.");
		}
	}
}
