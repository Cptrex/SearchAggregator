using SearchAggregator.Models;

namespace SearchAggregator.Interfaces;

public interface ISearchContextRepository
{
    public Task<List<SearchAggregatorResult>> GetAllAggregatorResults(CancellationToken cancellationToken);

    public Task<List<SearchAggregatorResult>> GetAggregatorResultBySearchText(string searchText, CancellationToken cancellationToken);

    public Task AddSearchAggregatorResult(SearchAggregatorResult aggregatorResult);
    public Task AddSearchAggregatorResults(List<SearchAggregatorResult> aggregatorResults);

    public Task UpdateSearchAggregatorResult(SearchAggregatorResult aggregatorResultData, CancellationToken cancellationToken);

    public Task DeleteSearchAggregatorResult(int id, CancellationToken cancellationToken);
}