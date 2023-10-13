using RestSharp;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace HttpClientDemo.ClientNetFX6
{
    public class Service64
    {
        private readonly IRestClientFactory _restClientFactory;

        public Service64()
        {
            _restClientFactory = RestClientFactory.Instance;
        }

        public string GetEntries()
        {
            var client = _restClientFactory.CreateForDemoApi();

            var request = new RestRequest(ApiSettings.Instance.ApiGetEndpoint);
            var response = client.ExecuteGet(request);

            return response.StatusCode.ToString();
        }
    }
}
