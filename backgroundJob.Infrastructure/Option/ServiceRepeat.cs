namespace backgroundJob.Infrastructure.Option
{
	public enum ServiceRepeat
	{
		None = 0x1,
		Second = 0x2,
		Minute = 0x4,
		Hour = 0x8,
		Day = 0x16,
		Week = 0x32,
		Month = 0x64,
		Year = 0x128,
	}
}
