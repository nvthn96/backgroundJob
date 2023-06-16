using backgroundJob.Custom.ApiChecking.Entity;
using backgroundJob.Database.UnitOfWork;

namespace backgroundJob.Custom.ApiChecking.Database
{
	public class ApiCheckingDatabase : BaseUnitOfWork<ApiCheckingContext>
	{
		public readonly IRepository<ACUrl> Url;
		public readonly IRepository<ACData> Data;

		public ApiCheckingDatabase()
		{
			Url = new BaseRepository<ACUrl>(_context);
			Data = new BaseRepository<ACData>(_context);
		}
	}
}
