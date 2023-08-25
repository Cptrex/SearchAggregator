namespace SearchAggregator;

public class SearchEngineManager
{
    public static async Task SearchViaGoogle(IHttpClientFactory httpFactory, string searchText)
    {
        var httpClient = httpFactory.CreateClient();

        string googleAPIKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
        string googleAPIUrl = Environment.GetEnvironmentVariable("GOOGLE_API_SEARCH_URL");
        string searchEngineId = Environment.GetEnvironmentVariable("GOOGLE_API_SEARCH_ENGINE_ID");

        HttpResponseMessage response = await httpClient.GetAsync($"{googleAPIUrl}?q={searchText}&key={googleAPIKey}&cx={searchEngineId}");
        Console.WriteLine(123);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            //var json = JsonConvert.DeserializeObject(responseContent);
            Console.WriteLine(responseContent);
        }
        Console.WriteLine(123);
    }

    public static async Task SearchViaYandex(IHttpClientFactory httpFactory, string searchText) { }

    public static async Task SearchViaBing(IHttpClientFactory httpFactory, string searchText) { }
}