using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientDemo.ClientNetFX2
{
    // Option 21 - Same as option 2.
    // We're initiating 4 requests to demonstrate the effect of concurrent calls.
    // Since we're using the same HttpClient instance, we might expect that a single connection will be created and used for all calls.
    // But, that's not the case for concurrent calls. It will try to create new connections so it can serve the requests concurrently.
    // So, how many connections? For a single client instance, the max number of connections is defined by ServicePoint.ConnectionLimit setting (by default is 2, or 10 for web apps).
    // In our case, 2 connections will be created and used for all 4 calls. On the next method calls, the same 2 connections are reused.
    // Note: the limit is per client instance. That's why we end up with way more connections in Option11.
    internal class Service21
    {
        private static readonly HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri(ApiSettings.Instance.ApiUrl),
            DefaultRequestHeaders = { { "accept", "application/json" } }
        };

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
            var response = await _client.GetAsync(ApiSettings.Instance.ApiGetEndpoint).ConfigureAwait(false);

            return response.StatusCode.ToString();
        }
    }
}
