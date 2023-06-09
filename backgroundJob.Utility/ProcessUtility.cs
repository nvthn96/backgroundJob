using backgroundJob.Infrastructure.View.Process;
using System.Management;

namespace backgroundJob.Utility
{
	public class ProcessUtility
	{
		public static IEnumerable<ProcessInformation> GetProcessesInformation()
		{
			if (OperatingSystem.IsWindows())
			{
				var properties = typeof(ProcessInformation).GetProperties().Select(p => p.Name);

				using (ManagementObjectSearcher searcher =
					new ManagementObjectSearcher($"SELECT {string.Join(", ", properties)} FROM Win32_Process WHERE ExecutablePath IS NOT NULL"))

				using (ManagementObjectCollection objects = searcher.Get())
				{
					var processes = new List<ProcessInformation>();
					try
					{
						foreach (var item in objects)
						{
							processes.Add(new ProcessInformation()
							{
								ProcessId = Convert.ToInt32(item["ProcessId"]),
								Name = (string)item["Name"],
								CommandLine = (string)item["CommandLine"],
								ExecutablePath = (string)item["ExecutablePath"],
							});
						}
					}
					catch (ObjectDisposedException) { }

					return processes;
				}
			}

			throw new PlatformNotSupportedException("Method is only supported on Windows.");
		}
	}
}
