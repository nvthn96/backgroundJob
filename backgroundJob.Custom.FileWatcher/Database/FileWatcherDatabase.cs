using backgroundJob.Custom.FileWatcher.Entity;
using backgroundJob.Database.UnitOfWork;

namespace backgroundJob.Custom.FileWatcher.Database
{
	public class FileWatcherDatabase : BaseUnitOfWork<FileWatcherContext>
	{
		public readonly IRepository<FWPath> Path;
		public readonly IRepository<FWEvent> Event;

		public FileWatcherDatabase()
		{
			Path = new BaseRepository<FWPath>(_context);
			Event = new BaseRepository<FWEvent>(_context);
		}
	}
}
