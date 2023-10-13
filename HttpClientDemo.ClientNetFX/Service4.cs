using RestSharp;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HttpClientDemo.ClientNetFX4
{
    // Option 4
    // RestClient offers the option to reuse the internal HttpClient instance by setting useClientFactory to true.
    // It holds a dictionary of HttpClient instances, one per BaseUrl.
    // This is a compelling approach, since no longer we have to capture it as a static state. Also it offers more flexibility and customization per call.
    // It's prone to the same DNS issues. Follow the recommendations from Option2.
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
