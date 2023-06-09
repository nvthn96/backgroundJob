namespace backgroundJob.Infrastructure.View.Process
{
	/// <summary>
	/// Properties name are taken from https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-process
	/// </summary>
	public class ProcessInformation
	{
		public int ProcessId { get; set; }
		public string Name { get; set; } = string.Empty;
		public string CommandLine { get; set; } = string.Empty;
		public string ExecutablePath { get; set; } = string.Empty;
	}
}
