using Microsoft.AspNetCore.Mvc;
using SearchAggregator.Interfaces;
using SearchAggregator.Models;

namespace SearchAggregator.Controllers;

[ApiController]
[Route("api/v1/aggregator")]
public class SearchAggregatorController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ISearchContextRepository _searchContextRepository;
    private readonly ISearchEngineService _searchEngineService;

    public SearchAggregatorController(ISearchEngineService searchEngineService, IHttpClientFactory httpClientFactory, ISearchContextRepository repository)
    {
        _httpClientFactory = httpClientFactory;
        _searchContextRepository = repository;
        _searchEngineService = searchEngineService;
    }

    [HttpGet]
    [Route("search")]
    public async Task<IActionResult> SearchInWeb([FromQuery] string searchText, CancellationToken cancellationToken)
    {
        var foundSearchResults = await _searchContextRepository.GetAggregatorResultBySearchText(searchText, cancellationToken);

        if (foundSearchResults == null || foundSearchResults.Count == 0) 
        {
            var googleItems = await _searchEngineService.SearchViaGoogle(_httpClientFactory, searchText);
            var bingItems = await _searchEngineService.SearchViaBing(_httpClientFactory, searchText);
            var yandexItems = await _searchEngineService.SearchViaYandex(_httpClientFactory, searchText);

            var aggregatorResult = new SearchAggregatorResult(searchText, googleItems, yandexItems, bingItems);

            await _searchContextRepository.AddSearchAggregatorResult(aggregatorResult);

            /*await Task.WhenAll(
            _searchEngineService.SearchViaGoogle(_httpClientFactory, searchText),
            _searchEngineService.SearchViaYandex(_httpClientFactory, searchText),
            _searchEngineService.SearchViaBing(_httpClientFactory, searchText)
            );*/
        }

        return Ok();
    }
}