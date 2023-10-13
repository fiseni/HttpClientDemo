using RestSharp;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace HttpClientDemo.ClientNetFX6
{
    public class Service63
    {
        private readonly IRestClientFactory _restClientFactory;

        public Service63()
        {
            _restClientFactory = RestClientFactory.Instance;
        }

        public async Task<string> GetEntries()
        {
            var client = _restClientFactory.CreateForDemoApi();

            var task1 = RunAsync(client);
            var task2 = RunAsync(client);
            var task3 = RunAsync(client);
            var task4 = RunAsync(client);

            await Task.WhenAll(task1, task2, task3, task4).ConfigureAwait(false);

            return task1.Result + task2.Result + task3.Result + task4.Result;
        }
        private Task<string> RunAsync(RestClient client)
        {
            Console.WriteLine("Running. Thread: " + Thread.CurrentThread.ManagedThreadId + ", Time:" + DateTime.Now.ToString("O"));
            var request = new RestRequest(ApiSettings.Instance.ApiGetEndpoint);
            var response = client.ExecuteGet(request);

            return Task.FromResult(response.StatusCode.ToString());
        }
    }
}
