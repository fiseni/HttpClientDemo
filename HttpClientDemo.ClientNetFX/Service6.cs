using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace HttpClientDemo.ClientNetFX6
{
    // Option 6
    // It's an evolved version of Option5. I'm quite fond of this approach.
    // We're utilizing the useClientFactory setting to cache the HttpClient instances, but consumers have flexibility of further customization per RestClient instance.
    // Also tried to mimic the IHttpClientFactory behavior, the named clients feature in particular.
    public class Service6
    {
        private readonly IRestClientFactory _restClientFactory;

        public Service6()
        {
            _restClientFactory = RestClientFactory.Instance;
        }

        public async Task<string> GetStatusCodeAsync()
        {
            var client = _restClientFactory.CreateForDemoApi();

            var request = new RestRequest(ApiSettings.Instance.ApiGetEndpointProtected);
            var response = await client.ExecuteGetAsync(request).ConfigureAwait(false);

            return response.StatusCode.ToString();
        }
    }

    public interface IRestClientFactory
    {
        RestClient Create(string name);
    }

    public class RestClientFactory : IRestClientFactory
    {
        private static readonly Lazy<List<IClientFactory>> _listFactories = new Lazy<List<IClientFactory>>(FindAllClientFactories);
        private static readonly Lazy<RestClientFactory> _instance = new Lazy<RestClientFactory>(() => new RestClientFactory());
        private readonly Dictionary<string, IClientFactory> _factories;

        public static RestClientFactory Instance => _instance.Value;

        public RestClientFactory(IEnumerable<IClientFactory> factories)
        {
            _factories = factories.ToDictionary(x => x.Name, x => x);
        }
        private RestClientFactory()
        {
            _factories = _listFactories.Value.ToDictionary(x => x.Name, x => x);
        }

        public RestClient Create(string name)
        {
            if (_factories.TryGetValue(name, out var factory))
            {
                return factory.Create();
            }

            throw new NotSupportedException($"ClientFactory with name '{name}' not found.");
        }

        private static List<IClientFactory> FindAllClientFactories()
        {
            var factoryInterface = typeof(IClientFactory);
            return factoryInterface.Assembly
                .GetTypes()
                .Where(type => factoryInterface != type && factoryInterface.IsAssignableFrom(type))
                .Select(type => Activator.CreateInstance(type) as IClientFactory)
                .ToList();
        }
    }

    public interface IClientFactory
    {
        string Name { get; }
        RestClient Create();
    }

    public static class DemoApiClientExtensions
    {
        public static RestClient CreateForDemoApi(this IRestClientFactory factory)
            => factory.Create(nameof(DemoApiClient));
    }

    public class DemoApiClient : IClientFactory
    {
        public string Name { get; } = nameof(DemoApiClient);

        public RestClient Create()
        {
            var apiUri = new Uri(ApiSettings.Instance.ApiUrl);

            var options = new RestClientOptions(apiUri)
            {
                Authenticator = new MyAuthenticator(apiUri)
            };

            var client = new RestClient(
               options: options,
               configureDefaultHeaders: (headers) =>
                {
                    headers.Add("accept", "application/json");
                },
               useClientFactory: true);

            return client;
        }

        private class MyAuthenticator : IAuthenticator
        {
            private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
            private static TokenDto _token;
            private readonly Uri _apiUri;

            public MyAuthenticator(Uri apiUri)
            {
                _apiUri = apiUri;
            }

            public async ValueTask Authenticate(IRestClient client, RestRequest request)
            {
                var token = await GetToken(_apiUri).ConfigureAwait(false);

                var headerParameter = new HeaderParameter(KnownHeaders.Authorization, "Bearer " + token.AccessToken);
                request.AddOrUpdateParameter(headerParameter);
            }

            private static bool IsTokenValid => _token?.Expiration > DateTime.UtcNow.Ticks;

            private static async ValueTask<TokenDto> GetToken(Uri apiUri)
            {
                Console.WriteLine($"Checking Token. \tTime: {DateTime.Now:mm:ss.FFFFFFF}");

                if (IsTokenValid) return _token;

                Console.WriteLine($"Waiting for Token. \tTime: {DateTime.Now:mm:ss.FFFFFFF}");

                await _semaphoreSlim.WaitAsync().ConfigureAwait(false);

                try
                {
                    if (IsTokenValid) return _token;

                    Console.WriteLine($"Getting new Token. \tTime: {DateTime.Now:mm:ss.FFFFFFF}");

                    var options = new RestClientOptions(apiUri);

                    using (var client = new RestClient(options))
                    {
                        var request = new RestRequest("identity/authenticate");
                        _token = await client.PostAsync<TokenDto>(request).ConfigureAwait(false);
                    };
                }
                finally
                {
                    _semaphoreSlim.Release();
                }

                return _token;
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
