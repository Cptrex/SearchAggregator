using Newtonsoft.Json;
using SearchAggregator.Interfaces;
using SearchAggregator.SearchJsonModels.Bing;
using SearchAggregator.SearchJsonModels.Google;
using SearchAggregator.SearchJsonModels.Yandex;
using System.Xml;

namespace SearchAggregator.Services;

public class SearchEngineService : ISearchEngineService
{
    public async Task<List<GoogleItemModel>> SearchViaGoogle(IHttpClientFactory httpFactory, string searchText)
    {
        var httpClient = httpFactory.CreateClient();

        string googleAPIKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
        string googleAPIUrl = Environment.GetEnvironmentVariable("GOOGLE_API_SEARCH_URL");
        string searchEngineId = Environment.GetEnvironmentVariable("GOOGLE_API_SEARCH_ENGINE_ID");

        HttpResponseMessage response = await httpClient.GetAsync($"{googleAPIUrl}?q={searchText}&key={googleAPIKey}&cx={searchEngineId}&num=10");
      
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var googleResults = JsonConvert.DeserializeObject<GoogleResponseModel>(responseContent);
            if (googleResults != null)
            {
                return googleResults.Items;
            }
        }

        return new List<GoogleItemModel>();
    }
    public async Task<List<BingItemModel>> SearchViaBing(IHttpClientFactory httpFactory, string searchText)
    {
        string bingAPIKey = Environment.GetEnvironmentVariable("BING_API_KEY");
        string bingAPIUrl = Environment.GetEnvironmentVariable("BING_SEARCH_V7_ENDPOINT");

        var httpClient = httpFactory.CreateClient();

        httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", bingAPIKey);

        HttpResponseMessage response = await httpClient.GetAsync($"{bingAPIUrl}?q={Uri.EscapeDataString(searchText)}&=count=10");

        if (response.IsSuccessStatusCode)
        {
            string bingData = await response.Content.ReadAsStringAsync();
            var bingResults = JsonConvert.DeserializeObject<BingBaseModel>(bingData);
            return bingResults.WebPages.Value;
        }

        return new List<BingItemModel>();
    }
    public async Task<List<YandexItemModel>> SearchViaYandex(IHttpClientFactory httpFactory, string searchText)
    {
        string yandexAPIUser = Environment.GetEnvironmentVariable("YANDEX_API_USER");
        string yandexAPIKey = Environment.GetEnvironmentVariable("YANDEX_API_KEY");
        string yandexAPIUrl = Environment.GetEnvironmentVariable("YANDEX_API_SEARCH_URL");

        var httpClient = httpFactory.CreateClient();

        HttpResponseMessage response = await httpClient.GetAsync($"{yandexAPIUrl}?user={yandexAPIUser}&key={yandexAPIKey}" +
            $"&q={Uri.EscapeDataString(searchText)}" +
            $"&l10n=en&sortby=tm.order%3Dascending" +
            $"&filter=strict" +
            $"&groupby=attr%3D%22%22.mode%3Dflat.groups-on-page%3D10.docs-in-group%3D1");
        
        var yandexSearchResultList = new List<YandexItemModel>();

        if (response.IsSuccessStatusCode)
        {
            string yandexData = await response.Content.ReadAsStringAsync();
            Console.WriteLine(yandexData);
            XmlDocument xmlDoc = new();
            xmlDoc.LoadXml(yandexData);
            XmlElement root = xmlDoc.DocumentElement;
            XmlNodeList docNodes = root.SelectNodes("/yandexsearch/response/results/grouping/group/doc");

            Console.WriteLine(docNodes.Count);
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

        Console.WriteLine(JsonConvert.SerializeObject(yandexSearchResultList));

        return yandexSearchResultList;
    }
}