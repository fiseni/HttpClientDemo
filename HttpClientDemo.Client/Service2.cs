namespace HttpClientDemo.Client2;

// Option 2 - Much better approach
internal class Service2
{
    private static readonly HttpClient _client = new HttpClient
    {
        BaseAddress = new Uri(ApiSettings.Instance.ApiUrl),
        DefaultRequestHeaders = { { "accept", "application/json" } }
    };

    public async Task<string> GetStatusCodeAsync()
    {
        try
        {
            var response = await _client.GetAsync(ApiSettings.Instance.ApiGetEndpoint).ConfigureAwait(false);

            return response.StatusCode.ToString();
        }
        catch (Exception)
        {
            return "Exception";
        }
    }
}
