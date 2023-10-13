using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientDemo.ClientNetFX1
{
    internal class Service11
    {
        public async Task<string> GetStatusCodeAsync()
        {
            try
            {
                var task1 = RunAsync();
                var task2 = RunAsync();
                var task3 = RunAsync();
                var task4 = RunAsync();

                await Task.WhenAll(task1, task2, task3, task4).ConfigureAwait(false);

                return $"{task1.Result} {task2.Result} {task3.Result} {task4.Result}";
            }
            catch (Exception)
            {
                return "Exception";
            }
        }

        private async Task<string> RunAsync()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(ApiSettings.Instance.ApiUrl),
                DefaultRequestHeaders = { { "accept", "application/json" } }
            };

            var response = await client.GetAsync(ApiSettings.Instance.ApiGetEndpoint).ConfigureAwait(false);

            return response.StatusCode.ToString();
        }
    }
}
