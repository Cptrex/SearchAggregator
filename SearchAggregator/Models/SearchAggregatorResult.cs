using Newtonsoft.Json;
using SearchAggregator.SearchJsonModels.Google;
using SearchAggregator.SearchJsonModels.Yandex;
using SearchAggregator.SearchJsonModels.Bing;

namespace SearchAggregator.Models;

public class SearchAggregatorResult
{
    public int Id { get; set; }
    public string SearchText { get; set; }
    public string GoogleResult { get; set; }
    public string YandexResult { get; set; }
    public string BingResult { get; set; }

    public SearchAggregatorResult()
    {
    }

    public SearchAggregatorResult(string searchText, List<GoogleItemModel> googleResults, List<YandexItemModel> yandexResuls, List<BingItemModel> bingResults)
    {
        SearchText = searchText;
        GoogleResult = JsonConvert.SerializeObject(googleResults);
        YandexResult = JsonConvert.SerializeObject(yandexResuls);
        BingResult = JsonConvert.SerializeObject(bingResults);
    }
}