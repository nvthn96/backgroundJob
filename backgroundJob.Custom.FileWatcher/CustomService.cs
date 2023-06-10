using backgroundJob.Custom.FileWatcher.Database;
using backgroundJob.Custom.FileWatcher.Flows;
using backgroundJob.Extensions;
using backgroundJob.Infrastructure.Option;
using backgroundJob.Infrastructure.Service;
using Microsoft.Extensions.Logging;

namespace backgroundJob.Custom.FileWatcher
{
	public class CustomService : IScopedService
	{
		private readonly ILogger<CustomService> _logger;
		private readonly IDictionary<string, FileSystemWatcher> _watcherDictionary;
		private readonly HashSet<string> _pathSet;
		private readonly FileWatcherDatabase _fileWatcherDatabase;

		private readonly ProcessFlow _process;

		public ServiceOption Option { get; set; } = new();

		public CustomService(ILogger<CustomService> logger, FileWatcherDatabase fileWatcherDatabase)
		{
			_logger = logger;
			_fileWatcherDatabase = fileWatcherDatabase;
			_watcherDictionary = new Dictionary<string, FileSystemWatcher>();
			_pathSet = new();

			_process = new(logger);
		}

		public async Task RunAsync(CancellationToken token)
		{
			_logger.LogInformation($"FileWatcher is starting.");

			Option.Status.AddEnum(ServiceStatus.Ready);

			try
			{
				var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
				do
				{
					Option.Status.AddEnum(ServiceStatus.Running);

					var model = _process.GetModel(_fileWatcherDatabase, _pathSet);

					_process.CheckingNew(_fileWatcherDatabase, _watcherDictionary, _pathSet, model, token);
					_process.CheckingMoved(_watcherDictionary, _pathSet, model);

					Option.Status.AddEnum(ServiceStatus.Success);
				}
				while (await timer.WaitForNextTickAsync(token));
			}
			catch (OperationCanceledException)
			{
				Option.Status.AddEnum(ServiceStatus.Stopped);
			}
			finally
			{
				Option.Status.AddEnum(ServiceStatus.Completed);
				_logger.LogInformation($"FileWatcher is stopping.");
			}
		}
	}
}