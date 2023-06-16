using backgroundJob.Custom.ApiChecking.Entity;

namespace backgroundJob.Custom.ApiChecking.Flows
{
	public class FlowModel
	{
		public IEnumerable<ACUrl> NewUrls { get; set; } = Enumerable.Empty<ACUrl>();
		public IEnumerable<ACUrl> MovedUrls { get; set; } = Enumerable.Empty<ACUrl>();
	}
}
