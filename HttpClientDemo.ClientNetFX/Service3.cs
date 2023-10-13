using RestSharp;
using System.Threading.Tasks;

namespace HttpClientDemo.ClientNetFX3
{
    internal class Service3
    {
        public async Task<string> GetStatusCode()
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
