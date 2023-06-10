using backgroundJob.Custom.FileWatcher.Database;
using backgroundJob.Custom.FileWatcher.Entity;

namespace backgroundJob.Custom.FileWatcher.EventHandlers
{
	public class RenamedEvent
	{
		private readonly FileWatcherDatabase _fileWatcherDatabase;
		private readonly Guid _pathId;
		private readonly CancellationToken _token;

		public RenamedEvent(FileWatcherDatabase fileWatcherDatabase, Guid pathId, CancellationToken token)
		{
			_fileWatcherDatabase = fileWatcherDatabase;
			_pathId = pathId;
			_token = token;
		}

		public RenamedEventHandler OnRenamed => new RenamedEventHandler(async (obj, e) =>
		{
			var newEvent = new FWEvent()
			{
				PathId = _pathId,
				Timestamp = DateTime.UtcNow,
				Event = e.ChangeType.ToString(),
				Name = e.Name,
				OldName = e.OldName,
			};
			await _fileWatcherDatabase.Event.AddAsync(newEvent, _token);
			await _fileWatcherDatabase.SaveChangesAsync(_token);
		});
	}
}
