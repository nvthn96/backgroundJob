using backgroundJob.Database.Entity;

namespace backgroundJob.Custom.ProcessTracking.Entity
{
	public class PTProcess : BaseEntity
	{
		public string Name { get; set; } = string.Empty;
		public string CommandLine { get; set; } = string.Empty;
		public string ExecutablePath { get; set; } = string.Empty;
	}
}
