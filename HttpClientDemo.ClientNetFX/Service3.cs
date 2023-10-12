using RestSharp;
using System.Threading.Tasks;

namespace HttpClientDemo.ClientNetFX3
{
    internal class Service3
    {
        public async Task<string> GetEntries()
        {
            var options = new RestClientOptions(ApiSettings.Instance.ApiUrl);

            var client = new RestClient(options);
            client.AddDefaultHeader("accept", "application/json");

            var request = new RestRequest(ApiSettings.Instance.ApiGetEndpoint);
            var response = await client.ExecuteGetAsync(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
                return response.ResponseStatus.ToString();

            return response.StatusCode.ToString();
        }
    }
}
