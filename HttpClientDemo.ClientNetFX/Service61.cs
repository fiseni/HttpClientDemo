﻿using RestSharp;
using System.Threading.Tasks;

namespace HttpClientDemo.ClientNetFX6
{
    // Option 61
    // It's based on Option6, just exploring the effect of concurrent calls.
    public class Service61
    {
        private readonly IRestClientFactory _restClientFactory;

        public Service61()
        {
            _restClientFactory = RestClientFactory.Instance;
        }

        public async Task<string> GetStatusCodeAsync()
        {
            var client = _restClientFactory.CreateForDemoApi();

            var task1 = RunAsync(client);
            var task2 = RunAsync(client);
            var task3 = RunAsync(client);
            var task4 = RunAsync(client);

            await Task.WhenAll(task1, task2, task3, task4).ConfigureAwait(false);

            return $"{task1.Result} {task2.Result} {task3.Result} {task4.Result}";
        }
        private async Task<string> RunAsync(RestClient client)
        {
            var request = new RestRequest(ApiSettings.Instance.ApiGetEndpointProtected);
            var response = await client.ExecuteGetAsync(request).ConfigureAwait(false);

            return response.StatusCode.ToString();
        }
    }
}
