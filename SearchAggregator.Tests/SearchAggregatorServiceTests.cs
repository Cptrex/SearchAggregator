using Microsoft.EntityFrameworkCore;
using SearchAggregator.Models;
using SearchAggregator.Repository;

namespace SearchAggregator.Tests
{
    public class SearchAggregatorServiceTests
    {
        [Fact]
        public async Task GetAggregatorResultBySearchText_ExistingResult_ReturnsResult()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<SearchContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using var dbContext = new SearchContext(dbContextOptions);
            var searchAggregatorService = new SearchContextRepository(dbContext);

            dbContext.SearchAggregatorResults.Add(new SearchAggregatorResult
            {
                SearchText = "text",
            });
            dbContext.SaveChanges();

            // Act
            var result = await searchAggregatorService.GetAggregatorResultBySearchText("text", CancellationToken.None);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddSearchAggregatorResult_NewResult_AddsResultToDatabase()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<SearchContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using var dbContext = new SearchContext(dbContextOptions);
            var searchAggregatorService = new SearchContextRepository(dbContext);

            var newResult = new SearchAggregatorResult
            {
                SearchText = "что-нибудь найти",
            };

            // Act
            await searchAggregatorService.AddSearchAggregatorResult(newResult);

            // Assert
            Assert.Single(dbContext.SearchAggregatorResults);
            var addedResult = dbContext.SearchAggregatorResults.Single();
            Assert.Equal("что-нибудь найти", addedResult.SearchText);
        }
    }
}
