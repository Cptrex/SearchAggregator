using Newtonsoft.Json;
using SearchAggregator.SearchJsonModels;

namespace SearchAggregator.Models;

public class SearchAggregatorResult
{
    public int Id { get; set; }
    [JsonProperty("searchText")]
    public string SearchText { get; set; }
    [JsonProperty("SearchResult")]
    public List<SearchItemBaseModel> SearchResult { get; set; }

    public SearchAggregatorResult()
    {
    }

    public SearchAggregatorResult(string searchText, List<SearchItemBaseModel> searchResult)
    {
        SearchText = searchText;
        SearchResult = searchResult;
    }
}