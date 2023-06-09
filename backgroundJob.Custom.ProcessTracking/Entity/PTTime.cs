using backgroundJob.Database.Entity;

namespace backgroundJob.Custom.ProcessTracking.Entity
{
	public class PTTime : BaseEntity
	{
		public Guid ProcessId { get; set; }
		public DateTime FirstDetect { get; set; }
		public DateTime LastDetect { get; set; }
	}
}
