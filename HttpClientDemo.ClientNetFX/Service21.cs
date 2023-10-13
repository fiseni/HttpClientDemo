using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientDemo.ClientNetFX2
{
    internal class Service21
    {
        private static readonly HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri(ApiSettings.Instance.ApiUrl),
            DefaultRequestHeaders = { { "accept", "application/json" } }
        };

        public async Task<string> GetEntries()
        {
            try
            {
                var task1 = RunAsync();
                var task2 = RunAsync();
                var task3 = RunAsync();
                var task4 = RunAsync();

                await Task.WhenAll(task1, task2, task3, task4);

                return task1.Result + task2.Result + task3.Result + task4.Result;
            }
            catch (Exception)
            {
                return "Exception";
            }
        }

        private async Task<string> RunAsync()
        {
            var response = await _client.GetAsync(ApiSettings.Instance.ApiGetEndpoint);

            return response.StatusCode.ToString();
        }
    }
}
