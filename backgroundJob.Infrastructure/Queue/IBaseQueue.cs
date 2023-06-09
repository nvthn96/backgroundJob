using backgroundJob.Infrastructure.Service;

namespace backgroundJob.Infrastructure.Queue
{
	public interface IBaseQueue<T> where T : IScopedService
	{
		Task EnQueue(T item, CancellationToken cancellationToken = default);
		Task<T> DeQueue(CancellationToken cancellationToken = default);
		QueueStatus GetStatus();
	}
}
