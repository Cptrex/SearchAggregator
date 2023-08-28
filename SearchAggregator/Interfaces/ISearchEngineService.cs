using SearchAggregator.SearchJsonModels.Google;
using SearchAggregator.SearchJsonModels.Yandex;
using SearchAggregator.SearchJsonModels.Bing;

namespace SearchAggregator.Interfaces;

public interface ISearchEngineService
{
    public Task<List<GoogleItemModel>> SearchViaGoogle(IHttpClientFactory httpFactory, string searchText);
    public Task<List<YandexItemModel>> SearchViaYandex(IHttpClientFactory httpFactory, string searchText);
    public Task<List<BingItemModel>> SearchViaBing(IHttpClientFactory httpFactory, string searchText);
    public bool IsSearchTextSizeCorrect(string searchText);
}