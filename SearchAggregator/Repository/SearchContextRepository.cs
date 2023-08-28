using Microsoft.EntityFrameworkCore;
using SearchAggregator.Interfaces;
using SearchAggregator.Models;

namespace SearchAggregator.Repository;

public class SearchContextRepository : ISearchContextRepository
{
    private readonly SearchContext _context;

    public SearchContextRepository(SearchContext context)
    {
        _context = context;
    }

    public async Task<SearchAggregatorResult> GetAggregatorResultBySearchText(string searchText, CancellationToken cancellationToken)
    {
        return await _context.SearchAggregatorResults.Where(agg =>
                                SearchContext.SoundLike(agg.SearchText) == SearchContext.SoundLike(searchText)).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddSearchAggregatorResult(SearchAggregatorResult aggregatorResult)
    {
        _context.SearchAggregatorResults.Add(aggregatorResult);
        await _context.SaveChangesAsync();
    }
}