using backgroundJob.Custom.FileWatcher.Database;
using backgroundJob.Custom.FileWatcher.EventHandlers;
using Microsoft.Extensions.Logging;

namespace backgroundJob.Custom.FileWatcher.Flows
{
	public class ProcessFlow
	{
		private readonly ILogger _logger;
		public ProcessFlow(ILogger logger)
		{
			_logger = logger;
		}

		public FlowModel GetModel(FileWatcherDatabase fileWatcherDatabase, HashSet<string> pathSet)
		{
			var paths = fileWatcherDatabase.Path.GetAll();

			var newPaths = paths.Where(p => !p.IsDeleted && !pathSet.Contains(p.Path)).ToArray();

			var movedPaths = paths.Where(p => p.IsDeleted && pathSet.Contains(p.Path)).ToArray();

			return new FlowModel()
			{
				NewPaths = newPaths,
				MovedPaths = movedPaths,
			};
		}

		public void CheckingNew(
			FileWatcherDatabase fileWatcherDatabase,
			IDictionary<string, FileSystemWatcher> fileWatcherDictionary,
			HashSet<string> pathSet,
			FlowModel model,
			CancellationToken token)
		{
			foreach (var path in model.NewPaths)
			{
				_logger.LogInformation($"New path is added: {path.Path}");
				pathSet.Add(path.Path);

				var fileWatcher = new FileSystemWatcher();
				fileWatcher.Path = path.Path;
				fileWatcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess;
				fileWatcher.Filter = path.Extension;
				fileWatcher.IncludeSubdirectories = path.IncludeSubFolders;
				fileWatcher.Changed += new ChangedEvent(fileWatcherDatabase, path.Id, token).OnChanged;
				fileWatcher.Created += new CreatedEvent(fileWatcherDatabase, path.Id, token).OnCreated;
				fileWatcher.Deleted += new DeletedEvent(fileWatcherDatabase, path.Id, token).OnDeleted;
				fileWatcher.Renamed += new RenamedEvent(fileWatcherDatabase, path.Id, token).OnRenamed;
				fileWatcher.EnableRaisingEvents = true;

				fileWatcherDictionary.Add(path.Path, fileWatcher);
			}
		}

		public void CheckingMoved(
			IDictionary<string, FileSystemWatcher> fileWatcherDictionary,
			HashSet<string> pathSet,
			FlowModel model)
		{
			foreach (var path in model.MovedPaths)
			{
				_logger.LogInformation($"Path is moved: {path.Path}");
				var fileWatcher = fileWatcherDictionary[path.Path];
				fileWatcher.EnableRaisingEvents = false;
				fileWatcher.Dispose();

				pathSet.Remove(path.Path);
				fileWatcherDictionary.Remove(path.Path);
			}
		}
	}
}
