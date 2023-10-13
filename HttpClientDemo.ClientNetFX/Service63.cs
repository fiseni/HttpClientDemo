using RestSharp;

namespace HttpClientDemo.ClientNetFX6
{
    // Option 63
    // It's based on Option6, just exploring the sync API of RestSharp.
    // It uses AsyncHelper to run sync over async. That's something that we should avoid at all costs. But, some old technologies like web services (asmx) don't have support for TAP.
    // That's one of the reasons I tend to use RestSharp for older apps, the sync APIs.
    // It's quite compelling implementation. Unlike many implementation that tend to create new thread to avoid deadlock `Task.Run(async () => {await ...}).Result`,
    // this implementation is using a custom SynchronizationContext to avoid deadlock while remaining on the same thread. Quite convenient, you can even access HttpContext.Current.
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
