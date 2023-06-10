using backgroundJob.Custom.FileWatcher.Entity;

namespace backgroundJob.Custom.FileWatcher.Flows
{
	public class FlowModel
	{
		public IEnumerable<FWPath> NewPaths { get; set; } = Enumerable.Empty<FWPath>();
		public IEnumerable<FWPath> MovedPaths { get; set; } = Enumerable.Empty<FWPath>();
	}
}
