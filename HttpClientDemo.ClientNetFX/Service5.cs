using HttpClientDemo.ClientNetFX;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Net;
using System.Threading.Tasks;

namespace HttpClientDemo.ClientNetFX5
{
    public class Service5
    {
        private readonly IRestClientFactory _restClientFactory;

        public Service5()
        {
            _restClientFactory = RestClientFactory.Instance;
        }

        public async Task<string> GetEntries()
        {
            var client = _restClientFactory.CreateForDemoServer();

            var request = new RestRequest(ApiSettings.Instance.ApiGetEndpoint);
            var response = await client.ExecuteGetAsync(request);

            return response.StatusCode.ToString();
        }
    }

    public interface IRestClientFactory
    {
        RestClient CreateForDemoServer();
    }

    public class RestClientFactory : IRestClientFactory
    {
        public static RestClientFactory Instance = new RestClientFactory();
        private RestClientFactory() { }

        public RestClient CreateForDemoServer() => DemoApiClient.Instance.CreateClient();
    }

    public class DemoApiClient
    {
        private static readonly Lazy<DemoApiClient> _instance = new Lazy<DemoApiClient>(() => new DemoApiClient());
        public static DemoApiClient Instance => _instance.Value;


        private readonly RestClient _client;
        private readonly Uri _apiUri;

        private DemoApiClient()
        {
            _apiUri = new Uri(ApiSettings.Instance.ApiUrl);

            var servicePoint = ServicePointManager.FindServicePoint(_apiUri);
            servicePoint.ConnectionLeaseTimeout = 15 * 60 * 1000;
            servicePoint.ConnectionLimit = ServicePointManager.DefaultPersistentConnectionLimit;

            var options = new RestClientOptions(_apiUri)
            {
                Authenticator = new MyAuthenticator(_apiUri)
            };

            _client = new RestClient(options, (headers) =>
            {
                headers.Add("accept", "application/json");
            });
        }

        public RestClient CreateClient() => _client;

        private class MyAuthenticator : IAuthenticator
        {
            private readonly Uri _apiUri;
            private TokenDto _token;

            public MyAuthenticator(Uri apiUri)
            {
                _apiUri = apiUri;
            }

            public async ValueTask Authenticate(IRestClient client, RestRequest request)
            {
                if (_token == null || string.IsNullOrEmpty(_token.AccessToken) || _token.Expiration < DateTime.UtcNow.Ticks)
                {
                    _token = await GetToken();
                }

                var headerParameter = new HeaderParameter(KnownHeaders.Authorization, "Bearer " + _token.AccessToken);
                request.AddOrUpdateParameter(headerParameter);
            }

            private async Task<TokenDto> GetToken()
            {
                var options = new RestClientOptions(_apiUri);

                using (var client = new RestClient(options))
                {
                    var request = new RestRequest("identity/authenticate");
                    var result = await client.PostAsync<TokenDto>(request);
                    return result;
                };
            }
        }
    }
}
