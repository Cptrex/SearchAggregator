using Newtonsoft.Json;
using SearchAggregator.SearchJsonModels.Google;
using SearchAggregator.SearchJsonModels.Yandex;
using SearchAggregator.SearchJsonModels.Bing;

namespace SearchAggregator.Models;

public class SearchAggregatorResult
{
    public int Id { get; set; }
    [JsonProperty("searchText")]
    public string SearchText { get; set; }
    [JsonProperty("googleResult")]
    public List<GoogleItemModel> GoogleResult { get; set; }
    [JsonProperty("yandexResult")]
    public List<YandexItemModel> YandexResult { get; set; }
    [JsonProperty("bingResult")]
    public List<BingItemModel> BingResult { get; set; }

    public SearchAggregatorResult()
    {
    }

    public SearchAggregatorResult(string searchText, List<GoogleItemModel> googleResults, List<YandexItemModel> yandexResuls, List<BingItemModel> bingResults)
    {
        SearchText = searchText;
        GoogleResult = googleResults;
        YandexResult = yandexResuls;
        BingResult = bingResults;
    }
}