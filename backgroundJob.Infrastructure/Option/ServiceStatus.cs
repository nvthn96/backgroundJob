namespace backgroundJob.Infrastructure.Option
{
	public enum ServiceStatus
	{
		New = 0x0,
		Added = 0x1,
		Running = 0x2,
		Stopped = 0x4,
		Success = 0x8,
		Completed = 0x16,
	}
}
