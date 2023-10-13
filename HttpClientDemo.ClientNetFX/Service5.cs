using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Net;
using System.Threading.Tasks;

namespace HttpClientDemo.ClientNetFX5
{
    // Option 5
    // We've defined a factory that creates a RestClient instance (returns the same client instance). 
    // Also, we've defined a wrapper around the RestClient that configures the client, handles the authentication and exposes a singleton instance.
    // Note we're not utilizing the useClientFactory option, since we hold a single instance anyway.
    // It's prone to the same DNS issues. Follow the recommendations from Option2.
    public class Service5
    {
        private readonly IRestClientFactory _restClientFactory;

        public Service5()
        {
            _restClientFactory = RestClientFactory.Instance;
        }

        public async Task<string> GetStatusCodeAsync()
        {
            var client = _restClientFactory.CreateForDemoServer();

            var request = new RestRequest(ApiSettings.Instance.ApiGetEndpointProtected);
            var response = await client.ExecuteGetAsync(request).ConfigureAwait(false);

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
                    _token = await GetToken().ConfigureAwait(false);
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
                    var result = await client.PostAsync<TokenDto>(request).ConfigureAwait(false);
                    return result;
                };
            }
        }
    }

    public class TokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public long Expiration { get; set; }
    }
}
