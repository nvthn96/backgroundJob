using backgroundJob.Custom.ApiChecking.Database;
using backgroundJob.Custom.ApiChecking.Extension;
using Microsoft.Extensions.Logging;

namespace backgroundJob.Custom.ApiChecking.Flows
{
	public class ProcessFlow
	{
		private readonly ILogger _logger;
		public ProcessFlow(ILogger logger)
		{
			_logger = logger;
		}

		public FlowModel GetModel(ApiCheckingDatabase apiCheckingDatabase, HashSet<string> pathSet)
		{
			var paths = apiCheckingDatabase.Url.GetAll();

			var newUrls = paths.Where(p => !p.IsDeleted && !pathSet.Contains(p.Hash)).ToArray();

			var movedUrls = paths.Where(p => p.IsDeleted && pathSet.Contains(p.Hash)).ToArray();

			return new FlowModel()
			{
				NewUrls = newUrls,
				MovedUrls = movedUrls,
			};
		}

		public void CheckingNew(
			ApiCheckingDatabase apiCheckingDatabase,
			IDictionary<string, UrlWatcher> urlWatcherDictionary,
			HashSet<string> urlSet,
			FlowModel model,
			CancellationToken token)
		{
			foreach (var url in model.NewUrls)
			{
				_logger.LogInformation($"New path is added: {url.GetFullURL()}");
				urlSet.Add(url.Hash);

				var apiChecking = new UrlWatcher(url);
				_ = apiChecking.RunAsync(apiCheckingDatabase, token).ConfigureAwait(false);

				urlWatcherDictionary.Add(url.Hash, apiChecking);
			}
		}

		public void CheckingMoved(
			IDictionary<string, UrlWatcher> urlWatcherDictionary,
			HashSet<string> pathSet,
			FlowModel model)
		{
			foreach (var path in model.MovedUrls)
			{
				_logger.LogInformation($"Path is moved: {path.GetFullURL()}");
				var urlWatcher = urlWatcherDictionary[path.Hash];

				pathSet.Remove(path.Hash);
				urlWatcherDictionary.Remove(path.Hash);
			}
		}
	}
}
