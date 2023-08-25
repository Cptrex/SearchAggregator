using Microsoft.EntityFrameworkCore;

namespace SearchAggregator.Models;

public class SearchContext : DbContext
{
    public DbSet<SearchAggregatorResult> SearchAggregatorResults { get; set; }

    public SearchContext(DbContextOptions<SearchContext> options) : base(options)
    {
        Database.EnsureCreated();   // создаем базу данных при первом обращении
    }

    [DbFunction(Name = "SoundEx", Schema = "")]
    public static string SoundLike(string searchText)
    {
        throw new NotImplementedException();
    }
}