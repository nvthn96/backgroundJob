using backgroundJob.Infrastructure.Service;
using Microsoft.Extensions.Hosting;
using System.Threading.Channels;

namespace backgroundJob.Infrastructure.Queue
{
	public class BaseQueue<T> : BackgroundService, IBaseQueue<T> where T : IScopedService
	{
		private readonly Channel<T> _queue;
		private readonly QueueStatus _status = new();
		private int _enQueuedCount = 0;
		private int _deQueuedCount = 0;

		public BaseQueue()
		{
			var options = new BoundedChannelOptions(100)
			{
				FullMode = BoundedChannelFullMode.Wait,
			};
			_queue = Channel.CreateBounded<T>(options);
		}

		protected override async Task ExecuteAsync(CancellationToken token)
		{
			while (!token.IsCancellationRequested)
			{
				var service = await DeQueue(token);
				_ = service.RunAsync(token).ConfigureAwait(false);
			}
		}

		public async Task EnQueue(T item, CancellationToken token = default)
		{
			Interlocked.Increment(ref _enQueuedCount);
			await _queue.Writer.WriteAsync(item, token);
		}

		public async Task<T> DeQueue(CancellationToken token = default)
		{
			Interlocked.Increment(ref _deQueuedCount);
			return await _queue.Reader.ReadAsync(token);
		}

		public QueueStatus GetStatus()
		{
			_status.EnQueued = _enQueuedCount;
			_status.DeQueued = _deQueuedCount;
			return _status;
		}
	}
}
