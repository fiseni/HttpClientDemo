using RestSharp;
using System.Threading.Tasks;

namespace HttpClientDemo.ClientNetFX6
{
    public class Service62
    {
        private readonly IRestClientFactory _restClientFactory;

        public Service62()
        {
            _restClientFactory = RestClientFactory.Instance;
        }

        public async Task<string> GetStatusCodeAsync()
        {
            var task1 = RunAsync();
            var task2 = RunAsync();
            var task3 = RunAsync();
            var task4 = RunAsync();

            await Task.WhenAll(task1, task2, task3, task4).ConfigureAwait(false);

            return $"{task1.Result} {task2.Result} {task3.Result} {task4.Result}";
        }
        private async Task<string> RunAsync()
        {
            var client = _restClientFactory.CreateForDemoApi();

            var request = new RestRequest(ApiSettings.Instance.ApiGetEndpointProtected);
            var response = await client.ExecuteGetAsync(request).ConfigureAwait(false);

            return response.StatusCode.ToString();
        }
    }
}
