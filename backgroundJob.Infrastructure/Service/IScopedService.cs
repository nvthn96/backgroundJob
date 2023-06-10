using backgroundJob.Infrastructure.Option;

namespace backgroundJob.Infrastructure.Service
{
	public interface IScopedService
	{
		public ServiceOption Option { get; set; }
		public Task RunAsync(CancellationToken token);
	}
}
