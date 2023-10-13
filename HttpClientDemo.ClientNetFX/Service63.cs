using RestSharp;

namespace HttpClientDemo.ClientNetFX6
{
    // Testing sync calls of RestSharp.
    // It uses AsyncHelper to run sync over async, and I'm curious if there are issues.
    public class Service63
    {
        private readonly IRestClientFactory _restClientFactory;

        public Service63()
        {
            _restClientFactory = RestClientFactory.Instance;
        }

        public string GetStatusCode()
        {
            var client = _restClientFactory.CreateForDemoApi();

            var request = new RestRequest(ApiSettings.Instance.ApiGetEndpointProtected);
            var response = client.ExecuteGet(request);

            return response.StatusCode.ToString();
        }
    }
}
