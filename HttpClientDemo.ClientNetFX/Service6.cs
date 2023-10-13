using HttpClientDemo.ClientNetFX;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace HttpClientDemo.ClientNetFX6
{
    public class Service6
    {
        private readonly IRestClientFactory _restClientFactory;

        public Service6()
        {
            _restClientFactory = RestClientFactory.Instance;
        }

        public async Task<string> GetEntries()
        {
            var client = _restClientFactory.CreateForDemoApi();

            var request = new RestRequest(ApiSettings.Instance.ApiGetEndpoint);
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
        public static RestClientFactory Instance = new RestClientFactory();
        private RestClientFactory() { }

        private readonly Dictionary<string, IClientFactory> _factories = new Dictionary<string, IClientFactory>()
        {
            { nameof(DemoApiClient) , new DemoApiClient() }
        };

        public RestClient Create(string name)
        {
            if (_factories.TryGetValue(name, out var factory))
            {
                return factory.Create();
            }

            throw new NotSupportedException($"ClientFactory with name '{name}' not found.");
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
            private static readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
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

            private static async Task<TokenDto> GetToken(Uri apiUri)
            {
                Console.WriteLine("Checking Token, Thread:" + Thread.CurrentThread.ManagedThreadId + ", Time:" + DateTime.Now.ToString("O") + " - " + HttpContext.Current == null);
                
                if (IsTokenValid) return _token;

                Console.WriteLine("Waiting for Token, Thread:" + Thread.CurrentThread.ManagedThreadId + ", Time:" + DateTime.Now.ToString("O"));

                await semaphoreSlim.WaitAsync().ConfigureAwait(false);

                try
                {
                    if (IsTokenValid) return _token;

                    Console.WriteLine("Getting new Token, Thread:" + Thread.CurrentThread.ManagedThreadId + ", Time:" + DateTime.Now.ToString("O"));

                    var options = new RestClientOptions(apiUri);

                    using (var client = new RestClient(options))
                    {
                        var request = new RestRequest("identity/authenticate");
                        _token = await client.PostAsync<TokenDto>(request).ConfigureAwait(false);
                    };
                }
                finally
                {
                    semaphoreSlim.Release();
                }

                return _token;
            }
        }
    }
}
