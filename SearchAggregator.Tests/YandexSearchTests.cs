using Microsoft.Extensions.DependencyInjection;
using Moq.Protected;
using Moq;
using SearchAggregator.Extensions;
using System.Net;
using SearchAggregator.Services;

namespace SearchAggregator.Tests
{
    public class YandexSearchTests
    {
        [Fact]
        public async Task SearchViaYandex_ValidResponse_ReturnsYandexItemModels()
        {
            // Arrange
            var services = new ServiceCollection();

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(@"<yandexsearch> ... </yandexsearch>")
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://www.yandex.com/api/")
            };

            services.AddSingleton<IHttpClientFactory>(new MockHttpClientFactory(httpClient));
            services.AddTransient<SearchEngineService>();
            var serviceProvider = services.BuildServiceProvider();
            var searchService = serviceProvider.GetRequiredService<SearchEngineService>();

            // Act
            var result = await searchService.SearchViaYandex(new MockHttpClientFactory(httpClient), "test search");

            // Assert
            Assert.NotNull(result);

            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task SearchViaYandex_InvalidResponse_ReturnsEmptyList()
        {
            // Arrange
            var services = new ServiceCollection();

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://www.yandex.com/api/")
            };

            services.AddSingleton<IHttpClientFactory>(new MockHttpClientFactory(httpClient));
            services.AddTransient<SearchEngineService>();
            var serviceProvider = services.BuildServiceProvider();
            var searchService = serviceProvider.GetRequiredService<SearchEngineService>();

            // Act
            var result = await searchService.SearchViaYandex(new MockHttpClientFactory(httpClient), "test search");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }
    }
}