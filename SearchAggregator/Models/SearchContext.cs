using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SearchAggregator.SearchJsonModels.Bing;
using SearchAggregator.SearchJsonModels.Google;
using SearchAggregator.SearchJsonModels.Yandex;

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
        aggregatorEntity.Property(e => e.BingResult)
            .IsRequired(false)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<BingItemModel>>(v)
            )
            .HasDefaultValue(new List<BingItemModel>())
            .HasColumnType("nvarchar(MAX)");
        aggregatorEntity.Property(e => e.GoogleResult)
            .IsRequired(false)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<GoogleItemModel>>(v)
            )
            .HasDefaultValue(new List<GoogleItemModel>())
            .HasColumnType("nvarchar(MAX)");
        aggregatorEntity.Property(e => e.YandexResult)
            .IsRequired(false)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<YandexItemModel>>(v)
            )
            .HasDefaultValue(new List<YandexItemModel>())
            .HasColumnType("nvarchar(MAX)");
    }     

    [DbFunction("MySoundEx", "")]
    public static string SoundLike(string searchText)
    {
        throw new NotSupportedException();
    }
}