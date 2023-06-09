using System.Diagnostics;
using System.Management;

namespace backgroundJob.Extensions
{
	public static class ProcessExtension
	{
		public static string GetCommandLine(this Process process)
		{
			if (OperatingSystem.IsWindows())
			{
				using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process.Id))
				using (ManagementObjectCollection objects = searcher.Get())
				{
					return objects.Cast<ManagementBaseObject>()
						.SingleOrDefault()?["CommandLine"]?.ToString() ?? "";
				}
			}
			else if (OperatingSystem.IsLinux())
			{
				string cmdline = File.ReadAllText($"/proc/{process.Id}/cmdline");
				return cmdline;
			}

			throw new PlatformNotSupportedException("Method is only supported on Windows and Linux.");
		}
	}
}
