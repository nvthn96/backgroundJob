using backgroundJob.Custom.ApiChecking.Database;
using backgroundJob.Custom.ApiChecking.Flows;
using backgroundJob.Extensions;
using backgroundJob.Infrastructure.Option;
using backgroundJob.Infrastructure.Service;
using Microsoft.Extensions.Logging;

namespace backgroundJob.Custom.ApiChecking
{
	public class CustomService : IScopedService
	{
		private readonly ILogger<CustomService> _logger;
		private readonly IDictionary<string, UrlWatcher> _urlWatcherDictionary;
		private readonly HashSet<string> _urlSet;
		private readonly ApiCheckingDatabase _apiCheckingDatabase;

		private readonly ProcessFlow _process;

		public ServiceOption Option { get; set; } = new();

		public CustomService(ILogger<CustomService> logger, ApiCheckingDatabase apiCheckingDatabase)
		{
			_logger = logger;
			_apiCheckingDatabase = apiCheckingDatabase;
			_urlWatcherDictionary = new Dictionary<string, UrlWatcher>();
			_urlSet = new();

			_process = new(logger);
		}

		public async Task RunAsync(CancellationToken token)
		{
			_logger.LogInformation($"ApiChecking is starting.");

			Option.Status.AddEnum(ServiceStatus.Ready);

			try
			{
				var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
				do
				{
					Option.Status.AddEnum(ServiceStatus.Running);

					var model = _process.GetModel(_apiCheckingDatabase, _urlSet);

					_process.CheckingNew(_apiCheckingDatabase, _urlWatcherDictionary, _urlSet, model, token);
					_process.CheckingMoved(_urlWatcherDictionary, _urlSet, model);

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
				_logger.LogInformation($"ApiChecking is stopping.");
			}
		}
	}
}