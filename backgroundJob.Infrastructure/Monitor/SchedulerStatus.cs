namespace backgroundJob.Infrastructure.Monitor
{
	public class SchedulerStatus
	{
		public int Tracking { get; set; }
		public int Completed { get; set; }
		public int EnQueued { get; set; }
		public int DeQueued { get; set; }
	}
}
