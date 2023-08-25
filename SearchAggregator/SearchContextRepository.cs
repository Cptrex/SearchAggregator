using Microsoft.EntityFrameworkCore;
using SearchAggregator.Models;

namespace SearchAggregator;

public class SearchContextRepository : ISearchContextRepository
{
    private readonly SearchContext _context;

    public SearchContextRepository(SearchContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SearchAggregatorResult>> GetAllAggregatorResults()
    {
        return await _context.SearchAggregatorResults.ToListAsync();
    }

    public async Task<IEnumerable<SearchAggregatorResult>> GetAggregatorResultBySearchText(string searchText)
    {
        return await _context.SearchAggregatorResults.Where(agg => 
                                SearchContext.SoundLike(agg.SearchText) == SearchContext.SoundLike(searchText)).ToListAsync();
    }

    public async Task AddSearchAggregatorResult(SearchAggregatorResult product)
    {
        _context.SearchAggregatorResults.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSearchAggregatorResult(SearchAggregatorResult aggregatorResultData)
    {
        _context.SearchAggregatorResults.Update(aggregatorResultData);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSearchAggregatorResult(int id)
    {
        var aggregatorResult = await _context.SearchAggregatorResults.FindAsync(id);
        if (aggregatorResult != null) return;

        _context.SearchAggregatorResults.Remove(aggregatorResult);
        await _context.SaveChangesAsync();
    }
}