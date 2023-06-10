namespace backgroundJob.Extensions
{
	public static class CancellationTokenExtension
	{
		public static Task WhenCanceled(this CancellationToken cancellationToken)
		{
			var tcs = new TaskCompletionSource<bool>();
			cancellationToken.Register(s => ((TaskCompletionSource<bool>)s!).SetResult(true), tcs);
			return tcs.Task;
		}
	}
}
