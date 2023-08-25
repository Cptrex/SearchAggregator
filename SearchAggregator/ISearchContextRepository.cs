using SearchAggregator.Models;

namespace SearchAggregator;

public interface ISearchContextRepository
{
    public Task<IEnumerable<SearchAggregatorResult>> GetAllAggregatorResults();

    public Task<IEnumerable<SearchAggregatorResult>> GetAggregatorResultBySearchText(string searchText);

    public Task AddSearchAggregatorResult(SearchAggregatorResult product);

    public Task UpdateSearchAggregatorResult(SearchAggregatorResult aggregatorResultData);

    public Task DeleteSearchAggregatorResult(int id);
}