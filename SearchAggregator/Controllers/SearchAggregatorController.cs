using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SearchAggregator.DTOs;
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
    private readonly IMapper _mapper;


    public SearchAggregatorController(IMapper mapper, ISearchEngineService searchEngineService, IHttpClientFactory httpClientFactory, ISearchContextRepository repository)
    {
        _httpClientFactory = httpClientFactory;
        _searchContextRepository = repository;
        _searchEngineService = searchEngineService;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("search")]
    public async Task<IActionResult> SearchInWeb([FromQuery] string searchText, CancellationToken cancellationToken)
    {
        var foundSearchResults = await _searchContextRepository.GetAggregatorResultBySearchText(searchText, cancellationToken);

        if (foundSearchResults == null) 
        {
            var googleItems = await _searchEngineService.SearchViaGoogle(_httpClientFactory, searchText);
            var bingItems = await _searchEngineService.SearchViaBing(_httpClientFactory, searchText);
            var yandexItems = await _searchEngineService.SearchViaYandex(_httpClientFactory, searchText);

            var aggregatorResult = new SearchAggregatorResult(searchText, googleItems, yandexItems, bingItems);

            await _searchContextRepository.AddSearchAggregatorResult(aggregatorResult);

            foundSearchResults = aggregatorResult;
        }

        var webSearchDto = new WebSearchDto
        {
            GoogleResults = _mapper.Map<List<GoogleDto>>(foundSearchResults.GoogleResult),
            BingResults = _mapper.Map<List<BingDto>>(foundSearchResults.BingResult),
            YandexResults = _mapper.Map<List<YandexDto>>(foundSearchResults.YandexResult),
        };

        return Ok(webSearchDto);
    }
}