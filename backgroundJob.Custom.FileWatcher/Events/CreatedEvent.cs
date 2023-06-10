using backgroundJob.Custom.FileWatcher.Database;
using backgroundJob.Custom.FileWatcher.Entity;

namespace backgroundJob.Custom.FileWatcher.EventHandlers
{
	public class CreatedEvent
	{
		private readonly FileWatcherDatabase _fileWatcherDatabase;
		private readonly Guid _pathId;
		private readonly CancellationToken _token;

		public CreatedEvent(FileWatcherDatabase fileWatcherDatabase, Guid pathId, CancellationToken token)
		{
			_fileWatcherDatabase = fileWatcherDatabase;
			_pathId = pathId;
			_token = token;
		}

		public FileSystemEventHandler OnCreated => new FileSystemEventHandler(async (obj, e) =>
		{
			var newEvent = new FWEvent()
			{
				PathId = _pathId,
				Timestamp = DateTime.UtcNow,
				Event = e.ChangeType.ToString(),
				Name = e.Name,
			};
			await _fileWatcherDatabase.Event.AddAsync(newEvent, _token);
			await _fileWatcherDatabase.SaveChangesAsync(_token);
		});
	}
}
