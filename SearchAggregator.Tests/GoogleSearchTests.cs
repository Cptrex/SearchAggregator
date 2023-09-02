using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using SearchAggregator.Extensions;
using SearchAggregator.SearchJsonModels.Bing;
using SearchAggregator.SearchJsonModels.Google;
using SearchAggregator.Services;

namespace SearchAggregator.Tests;

public class GoogleSearchTests
{
    [Fact]
    public async Task SearchViaGoogle_ValidResponse_ReturnsGoogleItemModels()
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
                        ""items"": [
                            {
                                ""title"": ""Result 1"",
                                ""link"": ""https://example.com/1"",
                                ""snippet"": ""Snippet 1""
                            }
                        ]
                    }")
            })
            .Verifiable();

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://www.googleapis.com/customsearch/v1/")
        };

        services.AddSingleton<IHttpClientFactory>(new MockHttpClientFactory(httpClient));
        services.AddTransient<SearchEngineService>();
        var serviceProvider = services.BuildServiceProvider();
        var searchService = serviceProvider.GetRequiredService<SearchEngineService>();

        // Act
        var result = await searchService.SearchViaGoogle(new MockHttpClientFactory(httpClient), "test search");
        var firstResult = result[0] as GoogleItemModel;

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Result 1", firstResult.Title);
        Assert.Equal("https://example.com/1", firstResult.Link);
        Assert.Equal("Snippet 1", firstResult.Snippet);

        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task SearchViaGoogle_InvalidResponse_ReturnsEmptyList()
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
            BaseAddress = new Uri("https://www.googleapis.com/customsearch/v1/")
        };

        services.AddSingleton<IHttpClientFactory>(new MockHttpClientFactory(httpClient));
        services.AddTransient<SearchEngineService>();
        var serviceProvider = services.BuildServiceProvider();
        var searchService = serviceProvider.GetRequiredService<SearchEngineService>();

        // Act
        var result = await searchService.SearchViaGoogle(new MockHttpClientFactory(httpClient), "test search");

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