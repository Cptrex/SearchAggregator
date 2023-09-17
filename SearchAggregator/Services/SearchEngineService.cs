using Newtonsoft.Json;
using SearchAggregator.Interfaces;
using SearchAggregator.SearchJsonModels;
using SearchAggregator.SearchJsonModels.Bing;
using SearchAggregator.SearchJsonModels.Google;
using SearchAggregator.SearchJsonModels.Yandex;
using System.Xml;

namespace SearchAggregator.Services;

public class SearchEngineService : ISearchEngineService
{
    public async Task<List<SearchItemBaseModel>> SearchViaGoogle(IHttpClientFactory httpFactory, string searchText)
    {
        Console.WriteLine("[GOOGLE SEARCH] Init...");
        if (IsSearchTextSizeCorrect(searchText) == false) return new List<SearchItemBaseModel>();

        var httpClient = httpFactory.CreateClient();

        string googleAPIKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
        string googleAPIUrl = Environment.GetEnvironmentVariable("GOOGLE_API_SEARCH_URL");
        string searchEngineId = Environment.GetEnvironmentVariable("GOOGLE_API_SEARCH_ENGINE_ID");

        HttpResponseMessage response = await httpClient.GetAsync($"{googleAPIUrl}?q={searchText}&key={googleAPIKey}&cx={searchEngineId}&num=10");
      
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var googleResults = JsonConvert.DeserializeObject<GoogleBaseModel>(responseContent);
            if (googleResults != null)
            {
                Console.WriteLine("[GOOGLE SEARCH] Completed!");

                return googleResults.Items.ConvertAll(i=> (SearchItemBaseModel)i);
            }
        }

        return new List<SearchItemBaseModel>();
    }
    public async Task<List<SearchItemBaseModel>> SearchViaBing(IHttpClientFactory httpFactory, string searchText)
    {
        Console.WriteLine("[BING SEARCH] Init...");

        if (IsSearchTextSizeCorrect(searchText) == false) return new List<SearchItemBaseModel>();

        string bingAPIKey = Environment.GetEnvironmentVariable("BING_API_KEY");
        string bingAPIUrl = Environment.GetEnvironmentVariable("BING_SEARCH_V7_ENDPOINT");

        var httpClient = httpFactory.CreateClient();

        httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", bingAPIKey);

        // count = 11 потому что веб поиск возвращает -1 значение от count
        HttpResponseMessage response = await httpClient.GetAsync($"{bingAPIUrl}?q={Uri.EscapeDataString(searchText)}&=count=11");

        if (response.IsSuccessStatusCode)
        {
            string bingData = await response.Content.ReadAsStringAsync();
            var bingResults = JsonConvert.DeserializeObject<BingBaseModel>(bingData);

            Console.WriteLine("[BING SEARCH] Completed!");

            return bingResults.WebPages.Value.ConvertAll(i => (SearchItemBaseModel)i);
        }


        return new List<SearchItemBaseModel>();
    }
    public async Task<List<SearchItemBaseModel>> SearchViaYandex(IHttpClientFactory httpFactory, string searchText)
    {
        Console.WriteLine("[YANDEX SEARCH] Init...");

        if (IsSearchTextSizeCorrect(searchText) == false) return new List<SearchItemBaseModel>();

        string yandexAPIUser = Environment.GetEnvironmentVariable("YANDEX_API_USER");
        string yandexAPIKey = Environment.GetEnvironmentVariable("YANDEX_API_KEY");
        string yandexAPIUrl = Environment.GetEnvironmentVariable("YANDEX_API_SEARCH_URL");

        var httpClient = httpFactory.CreateClient();

        HttpResponseMessage response = await httpClient.GetAsync($"{yandexAPIUrl}?user={yandexAPIUser}&key={yandexAPIKey}" +
            $"&q={Uri.EscapeDataString(searchText)}" +
            $"&l10n=en&sortby=tm.order%3Dascending" +
            $"&filter=strict" +
            $"&groupby=attr%3D%22%22.mode%3Dflat.groups-on-page%3D10.docs-in-group%3D1");
        
        var yandexSearchResultList = new List<SearchItemBaseModel>();

        if (response.IsSuccessStatusCode)
        {
            string yandexData = await response.Content.ReadAsStringAsync();
            XmlDocument xmlDoc = new();
            xmlDoc.LoadXml(yandexData);
            XmlElement root = xmlDoc.DocumentElement;
            XmlNodeList docNodes = root.SelectNodes("/yandexsearch/response/results/grouping/group/doc");

            foreach (XmlNode docNode in docNodes)
            {
                var yandexItemModel = new YandexItemModel()
                {
                    Title = docNode.SelectSingleNode("title")?.InnerText,
                    Url = docNode.SelectSingleNode("url")?.InnerText,
                    Headline = docNode.SelectSingleNode("headline")?.InnerText
                };

                yandexSearchResultList.Add(yandexItemModel);
            }
        }

        List<SearchItemBaseModel> searchResults = yandexSearchResultList;
        
        Console.WriteLine($"[YANDEX SEARCH] Completed!");

        return searchResults;
    }

    public bool IsSearchTextSizeCorrect(string searchText)
    {
        const int maxSearchTextSize = 2048;

        return searchText.Length <= maxSearchTextSize ? true : false;
    }
}