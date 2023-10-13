using RestSharp;
using System.Threading.Tasks;

namespace HttpClientDemo.ClientNetFX3
{
    // Option 3 - Not recommended
    // It instantiates a new RestClient on each call. Internally, it uses HttpClient. That means new HttpMessageHandler instance and new TCP connection.
    // Same issues as Option1.
    // It serves to illustrate the point that just because we're using a 3rd party library, it doesn't mean that we're not prone to same mistakes.
    internal class Service3
    {
        public async Task<string> GetStatusCodeAsync()
        {
            var options = new RestClientOptions(ApiSettings.Instance.ApiUrl);

            var client = new RestClient(options);
            client.AddDefaultHeader("accept", "application/json");

            var request = new RestRequest(ApiSettings.Instance.ApiGetEndpoint);
            var response = await client.ExecuteGetAsync(request).ConfigureAwait(false);

            return response.StatusCode.ToString();
        }
    }
}
