using RestSharp;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HttpClientDemo.ClientNetFX4
{
    internal class Service4
    {
        public async Task<string> GetStatusCodeAsync()
        {
            var client = CreateClient();

            var request = new RestRequest(ApiSettings.Instance.ApiGetEndpoint);
            var response = await client.ExecuteGetAsync(request).ConfigureAwait(false);

            return response.StatusCode.ToString();
        }

        private RestClient CreateClient()
        {
            var options = new RestClientOptions(ApiSettings.Instance.ApiUrl);

            var client = new RestClient(options, ConfigureHeaders, useClientFactory: true);

            return client;
        }

        private void ConfigureHeaders(HttpRequestHeaders headers)
        {
            headers.Add("accept", "application/json");
        }
    }
}
