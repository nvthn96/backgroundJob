using backgroundJob.Infrastructure.Option;

namespace backgroundJob.Custom.ProcessTracking.Flows
{
	public class StatusFlow
	{
		public void SetStartStatus(ServiceOption option)
		{
			option.Status = ServiceStatus.New;
		}

		public void SetEndStatus(ServiceOption option)
		{
			if (option.Repeat == ServiceRepeat.None)
			{
				option.Status = ServiceStatus.Completed;
			}
			else
			{
				option.Status = ServiceStatus.Success;
			}
		}
	}
}
