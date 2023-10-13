using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientDemo.ClientNetFX1
{
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
