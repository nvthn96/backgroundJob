namespace backgroundJob.Infrastructure.Option
{
	public enum ServiceStatus
	{
		New = 0x1,
		Added = 0x2,
		Ready = 0x4,
		Running = 0x8,
		Stopped = 0x16,
		Exception = 0x32,
		Success = 0x64,
		Completed = 0x128,
	}
}
