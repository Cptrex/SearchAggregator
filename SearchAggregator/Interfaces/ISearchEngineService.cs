using SearchAggregator.SearchJsonModels;

namespace SearchAggregator.Interfaces;

public interface ISearchEngineService
{
    public Task<List<SearchItemBaseModel>> SearchViaGoogle(IHttpClientFactory httpFactory, string searchText);
    public Task<List<SearchItemBaseModel>> SearchViaBing(IHttpClientFactory httpFactory, string searchText);
    public Task<List<SearchItemBaseModel>> SearchViaYandex(IHttpClientFactory httpFactory, string searchText);
    public bool IsSearchTextSizeCorrect(string searchText);
}