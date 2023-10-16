using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientDemo.ClientNetFX2
{
    // Option 2 - Much better approach
    // In ASP.NET Core we have well established practices for HttpClient usage (e.g. IHttpClientFactory, typed clients).
    // In older platforms wiring up the IHttpClientFactory is not that straightforward, especially if you're utilizing old technologies like web services (asmx).
    // We can use a static HttpClient instance. This will reuse the same HttpMessageHandler instance and TCP connection.
    // The downside is that the client will remain oblivious to DNS changes. Also, the HttpClient 4.2.0.0 is not based on SocketsHttpHandler, therefore we can't utilize the PooledConnectionLifetime option.
    // Luckily, we still can use the ServicePointManager to control the connection lifetime. We can set the ConnectionLeaseTimeout to a lower value (by default is indefinite).
    // The ServicePointManager also exposes DnsRefreshTimeout setting (by default is 120 seconds).
    internal class Service2
    {
        private static readonly HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri(ApiSettings.Instance.ApiUrl),
            DefaultRequestHeaders = { { "accept", "application/json" } }
        };

        public async Task<string> GetStatusCodeAsync()
        {
            try
            {
                var response = await _client.GetAsync(ApiSettings.Instance.ApiGetEndpoint).ConfigureAwait(false);

                return response.StatusCode.ToString();
            }
            catch (Exception)
            {
                return "Exception";
            }
        }
    }
}
