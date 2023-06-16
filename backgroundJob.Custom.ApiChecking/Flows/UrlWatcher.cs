using backgroundJob.Custom.ApiChecking.Database;
using backgroundJob.Custom.ApiChecking.Entity;
using RestSharp;

namespace backgroundJob.Custom.ApiChecking.Flows
{
	public class UrlWatcher
	{
		private readonly ACUrl _url;
		public UrlWatcher(ACUrl url)
		{
			_url = url;
		}

		public async Task RunAsync(ApiCheckingDatabase apiCheckingDatabase, CancellationToken token)
		{
			var enableTimed = _url.IntervalInMinutes != 0;
			var timer = new PeriodicTimer(TimeSpan.FromMinutes(_url.IntervalInMinutes));
			do
			{
				var existingData = await apiCheckingDatabase.Data.FindAsync(api => api.UrlId == _url.Id, token);

				var option = new RestClientOptions($"{_url.Protocol}://{_url.Host}");
				var client = new RestClient(option);
				var request = new RestRequest(_url.URI);

				request.Method = _url.Method;

				if (_url.Headers != null)
				{
					foreach (var header in _url.Headers)
					{
						request.AddHeader(header.Key, header.Value);
					}
				}

				if (_url.Parameters != null)
				{
					foreach (var param in _url.Parameters)
					{
						request.AddQueryParameter(param.Key, param.Value);
					}
				}

				if (_url.BodyJson != null)
				{
					request.AddJsonBody(_url.BodyJson);
				}

				if (!_url.WaitResult)
				{
					_ = client.ExecuteAsync(request, token).ConfigureAwait(false);
				}

				else
				{
                    var response = await client.ExecuteAsync(request, token);
                    var content = response.Content;

                    if (existingData == null || content != existingData.Content)
                    {
                        var pass = content == _url.ExpectedResult;

                        var dataEntity = new ACData()
                        {
                            Content = content,
                            Timestamp = DateTime.UtcNow,
                            UrlId = _url.Id,
                            Pass = pass,
                        };

                        await apiCheckingDatabase.Data.AddAsync(dataEntity);
                    }
                }

			} while (enableTimed == true && await timer.WaitForNextTickAsync(token));
		}
	}
}
