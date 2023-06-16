using backgroundJob.Database.Entity;

namespace backgroundJob.Custom.ApiChecking.Entity
{
	public class ACData : BaseEntity
	{
		public Guid UrlId { get; set; }
		public DateTime? Timestamp { get; set; }
		public string? Content { get; set; }
		public bool? Pass { get; set; }
	}
}
