using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientDemo.ClientNetFX1
{
    // Option 1 - Not recommended
    // It instantiates a new HttpClient on each call. That means new HttpMessageHandler instance and new TCP connection.
    // The least we can do is dispose the HttpClient instance (enclose it in using statement). This will force the connection to TIME_WAIT state (OS will close it after ~2 minutes).
    // It's not efficient and can lead to socket exhaustion. Also, creating a new connection is expensive.
    internal class Service1
    {
        public async Task<string> GetStatusCodeAsync()
        {
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(ApiSettings.Instance.ApiUrl),
                    DefaultRequestHeaders = { { "accept", "application/json" } },
                };

                var response = await client.GetAsync(ApiSettings.Instance.ApiGetEndpoint).ConfigureAwait(false);

                return response.StatusCode.ToString();
            }
            catch (Exception)
            {
                return "Exception";
            }
        }
    }
}
