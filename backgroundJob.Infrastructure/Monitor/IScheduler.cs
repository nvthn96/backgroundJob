using backgroundJob.Infrastructure.Option;
using backgroundJob.Infrastructure.Service;

namespace backgroundJob.Infrastructure.Monitor
{
	public interface IScheduler
	{
		void AddService(IScopedService service, ServiceOption options);
		SchedulerStatus GetStatus();
	}
}
