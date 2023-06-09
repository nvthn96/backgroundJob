namespace backgroundJob.Infrastructure.Option
{
	public enum ServiceRepeat
	{
		None = 0x0,
		Second = 0x1,
		Minute = 0x2,
		Hour = 0x4,
		Day = 0x8,
		Week = 0x16,
		Month = 0x32,
		Year = 0x64,
	}
}
