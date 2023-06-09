using backgroundJob.Custom.ProcessTracking.Entity;
using backgroundJob.Database.UnitOfWork;

namespace backgroundJob.Custom.ProcessTracking.Database
{
	public class ProcessDatabase : BaseUnitOfWork<ProcessContext>
	{
		public readonly IRepository<PTProcess> Process;
		public readonly IRepository<PTTime> Time;

		public ProcessDatabase()
		{
			Process = new BaseRepository<PTProcess>(_context);
			Time = new BaseRepository<PTTime>(_context);
		}
	}
}
