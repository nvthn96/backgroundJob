using backgroundJob.Database.Entity;

namespace backgroundJob.Custom.ApiChecking.Entity
{
	public class ACUrl : BaseEntity
	{
		public string Protocol { get; set; } = string.Empty;
		public RestSharp.Method Method { get; set; }
		public string Host { get; set; } = string.Empty;
		public string URI { get; set; } = string.Empty;
		public IEnumerable<KeyValuePair<string, string>>? Headers { get; set; } = Enumerable.Empty<KeyValuePair<string, string>>();
		public IEnumerable<KeyValuePair<string, string>>? Parameters { get; set; } = Enumerable.Empty<KeyValuePair<string, string>>();
		public string? BodyJson { get; set; }
		public string Hash { get; set; } = string.Empty;

		public int IntervalInMinutes { get; set; }
		public bool WaitResult { get; set; } = true;
		public string? ExpectedResult { get; set; }
	}
}
