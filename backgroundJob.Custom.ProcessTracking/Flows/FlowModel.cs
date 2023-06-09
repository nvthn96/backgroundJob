namespace backgroundJob.Custom.ProcessTracking.Flows
{
	public class FlowModel
	{
		public IEnumerable<string> Items { get; set; } = Enumerable.Empty<string>();
		public IEnumerable<string> OnChecking { get; set; } = Enumerable.Empty<string>();
		public IEnumerable<Guid> OnCheckingId { get; set; } = Enumerable.Empty<Guid>();
		public IEnumerable<string> OnCreating { get; set; } = Enumerable.Empty<string>();
		public IEnumerable<string> OnUpdating { get; set; } = Enumerable.Empty<string>();
	}
}
