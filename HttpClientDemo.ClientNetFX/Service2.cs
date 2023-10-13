using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientDemo.ClientNetFX2
{
    internal class Service2
    {
        private static readonly HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri(ApiSettings.Instance.ApiUrl),
            DefaultRequestHeaders = { { "accept", "application/json" } }
        };

        public async Task<string> GetStatusCode()
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
