using SearchAggregator.Models;

namespace SearchAggregator.Interfaces;

public interface ISearchContextRepository
{
    public Task<SearchAggregatorResult> GetAggregatorResultBySearchText(string searchText, CancellationToken cancellationToken);

    public Task AddSearchAggregatorResult(SearchAggregatorResult aggregatorResult);
}