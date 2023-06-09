namespace backgroundJob.Infrastructure.Option
{
	public class ServiceOption
	{
		public ServiceStatus Status { get; set; }
		public ServiceRepeat Repeat { get; set; }
		public TimedOption Timed { get; set; }

		public ServiceOption()
		{
			Timed = new();
		}
	}
}
