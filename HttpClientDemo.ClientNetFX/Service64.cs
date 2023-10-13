using RestSharp;
using System.Threading.Tasks;

namespace HttpClientDemo.ClientNetFX6
{
    // Option 64
    // Same as option 63, just running multiple calls.
    // The first impression is that this will run fully synchronously. But, the results are quite surprising.
    public class Service64
    {
        private readonly IRestClientFactory _restClientFactory;

        public Service64()
        {
            _restClientFactory = RestClientFactory.Instance;
        }

        public string GetStatusCode()
        {
            var client = _restClientFactory.CreateForDemoApi();

            var task1 = RunAsync(client);
            var task2 = RunAsync(client);
            var task3 = RunAsync(client);
            var task4 = RunAsync(client);

            Task.WaitAll(task1, task2, task3, task4);

            return $"{task1.Result} {task2.Result} {task3.Result} {task4.Result}";
        }
        private Task<string> RunAsync(RestClient client)
        {
            var request = new RestRequest(ApiSettings.Instance.ApiGetEndpointProtected);
            var response = client.ExecuteGet(request);

            return Task.FromResult(response.StatusCode.ToString());
        }
    }
}
