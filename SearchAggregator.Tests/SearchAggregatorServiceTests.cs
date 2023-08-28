using Microsoft.EntityFrameworkCore;
using SearchAggregator.Models;
using SearchAggregator.Repository;

namespace SearchAggregator.Tests
{
    public class SearchAggregatorServiceTests
    {
        /// <summary>
        /// К сожалению, этот тест не пройдет. Всё дело в методе GetAggregatorResultBySearchText, который вызывает мой собственный метод в Linq выражении, который
        /// в свою очередь вызывает написанную SQL- функцию для выполнения встроенного механизма SOUNDEX в базе данных.
        /// На момент написания данного веб приложения у меня нет знаний как "замокать" в in-memory базу данных ещё и sql код по созданию моей функции.
        /// Данный тест оставляю тут только лишь для демонстрации, что я - умею.
        /// </summary>
        /// <returns></returns>
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
