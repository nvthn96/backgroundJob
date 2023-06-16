using backgroundJob.Custom.ApiChecking.Entity;

namespace backgroundJob.Custom.ApiChecking.Extension
{
	public static class UrlExtension
	{
		public static string GetFullURL(this ACUrl url)
		{
			return $"{url.Protocol}://{url.Host}/{url.URI}";
		}
	}
}
