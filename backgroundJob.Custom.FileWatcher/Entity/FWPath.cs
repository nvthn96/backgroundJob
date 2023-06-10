using backgroundJob.Database.Entity;

namespace backgroundJob.Custom.FileWatcher.Entity
{
	public class FWPath : BaseEntity
	{
		public string Path { get; set; } = string.Empty;
		public string Extension { get; set; } = string.Empty;
		public bool IncludeSubFolders { get; set; }
	}
}
