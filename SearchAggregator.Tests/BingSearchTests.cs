using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using SearchAggregator.Extensions;
using SearchAggregator.Services;

namespace SearchAggregator.Tests;

public class BingSearchTests
{
    [Fact]
    public async Task SearchViaBing_ValidResponse_ReturnsBingItemModels()
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
                Content = new StringContent(@"{
                        ""webPages"": {
                            ""value"": [
                                {
                                    ""name"": ""Result 1"",
                                    ""url"": ""https://example.com/1"",
                                    ""snippet"": ""Snippet 1""
                                }
                            ]
                        }
                    }")
            })
            .Verifiable();

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://api.cognitive.microsoft.com/bing/v7.0/")
        };

        services.AddSingleton<IHttpClientFactory>(new MockHttpClientFactory(httpClient));
        services.AddTransient<SearchEngineService>();
        var serviceProvider = services.BuildServiceProvider();
        var searchService = serviceProvider.GetRequiredService<SearchEngineService>();

        // Act
        var result = await searchService.SearchViaBing(new MockHttpClientFactory(httpClient), "test search");

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Result 1", result[0].Name);
        Assert.Equal("https://example.com/1", result[0].Url);
        Assert.Equal("Snippet 1", result[0].Snippet);

        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task SearchViaBing_InvalidResponse_ReturnsEmptyList()
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
            BaseAddress = new Uri("https://api.bing.microsoft.com/v7.0/search")
        };

        services.AddSingleton<IHttpClientFactory>(new MockHttpClientFactory(httpClient));
        services.AddTransient<SearchEngineService>();
        var serviceProvider = services.BuildServiceProvider();
        var searchService = serviceProvider.GetRequiredService<SearchEngineService>();

        // Act
        var result = await searchService.SearchViaBing(new MockHttpClientFactory(httpClient), "test search");

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