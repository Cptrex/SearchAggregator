using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

    public async Task<List<SearchAggregatorResult>> GetAllAggregatorResults(CancellationToken cancellationToken)
    {
        return await _context.SearchAggregatorResults.ToListAsync(cancellationToken);
    }

    public async Task<List<SearchAggregatorResult>> GetAggregatorResultBySearchText(string searchText, CancellationToken cancellationToken)
    {
        return await _context.SearchAggregatorResults.Where(agg =>
                                SearchContext.SoundLike(agg.SearchText) == SearchContext.SoundLike(searchText)).ToListAsync(cancellationToken);
    }

    public async Task AddSearchAggregatorResult(SearchAggregatorResult aggregatorResult)
    {
        _context.SearchAggregatorResults.Add(aggregatorResult);
        await _context.SaveChangesAsync();
    }

    public async Task AddSearchAggregatorResults(List<SearchAggregatorResult> aggregatorResults)
    {
        _context.SearchAggregatorResults.AddRange(aggregatorResults);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSearchAggregatorResult(SearchAggregatorResult aggregatorResultData, CancellationToken cancellationToken)
    {
        _context.SearchAggregatorResults.Update(aggregatorResultData);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteSearchAggregatorResult(int id, CancellationToken cancellationToken)
    {
        var aggregatorResult = await _context.SearchAggregatorResults.FindAsync(id);
        if (aggregatorResult != null) return;

        _context.SearchAggregatorResults.Remove(aggregatorResult);
        await _context.SaveChangesAsync(cancellationToken);
    }
}