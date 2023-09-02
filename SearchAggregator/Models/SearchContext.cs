using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SearchAggregator.SearchJsonModels;

namespace SearchAggregator.Models;

public class SearchContext : DbContext
{
    public DbSet<SearchAggregatorResult> SearchAggregatorResults { get; set; }

    public SearchContext(DbContextOptions<SearchContext> options) : base(options)
    {
        Database.EnsureCreated();   // создаем базу данных при первом обращении
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var aggregatorEntity = modelBuilder.Entity<SearchAggregatorResult>();
        aggregatorEntity.ToTable(nameof(SearchAggregatorResult));
        aggregatorEntity.HasIndex(e => e.SearchText);
        aggregatorEntity.Property(e => e.SearchText)
           .IsRequired(true)
           .HasColumnType("nvarchar(2048)");
        aggregatorEntity.Property(e => e.SearchResult)
            .IsRequired(false)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<SearchItemBaseModel>>(v)
            )
            .HasDefaultValue(new List<SearchItemBaseModel>())
            .HasColumnType("nvarchar(MAX)");
    }     

    [DbFunction("MySoundEx", "")]
    public static string SoundLike(string searchText)
    {
        throw new NotSupportedException();
    }
}