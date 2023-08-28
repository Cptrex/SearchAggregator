namespace SearchAggregator.Extensions;

public class MockHttpClientFactory : IHttpClientFactory
{
    private readonly HttpClient _httpClient;

    public MockHttpClientFactory(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public HttpClient CreateClient(string name)
    {
        return _httpClient;
    }
}