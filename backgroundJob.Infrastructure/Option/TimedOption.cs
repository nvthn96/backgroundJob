namespace backgroundJob.Infrastructure.Option
{
	public class TimedOption
	{
		//public bool Immediately { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public DateTime DateTime { get; set; }
		public TimeSpan Interval { get; set; }
		public List<DayOfWeek> DaysOfWeek { get; set; } = new();
		public List<int> DaysOfMonth { get; set; } = new();
		public List<DateTime> DaysOfYear { get; set; } = new();
	}
}
