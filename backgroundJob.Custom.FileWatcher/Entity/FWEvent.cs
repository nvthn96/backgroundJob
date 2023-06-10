using backgroundJob.Database.Entity;

namespace backgroundJob.Custom.FileWatcher.Entity
{
	public class FWEvent : BaseEntity
	{
		public Guid PathId { get; set; }
		public DateTime? Timestamp { get; set; }
		public string? Event { get; set; }
		public string? Name { get; set; }
		public string? OldName { get; set; }
	}
}
