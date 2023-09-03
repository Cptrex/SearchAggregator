using Microsoft.AspNetCore.Mvc;
using SearchAggregator.Interfaces;
using SearchAggregator.Models;
using SearchAggregator.SearchJsonModels;

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

        if (foundSearchResults != null) return Ok(foundSearchResults.SearchResult);

        var taskBing = _searchEngineService.SearchViaBing(_httpClientFactory, searchText);
        var taskGoogle = _searchEngineService.SearchViaGoogle(_httpClientFactory, searchText);
        var taskYandex = _searchEngineService.SearchViaYandex(_httpClientFactory, searchText);
        var searchTasks = new List<Task<List<SearchItemBaseModel>>> { taskBing, taskGoogle, taskYandex };
        var loopTimeout = TimeSpan.FromSeconds(30);
        var startLoopTime = DateTime.Now;

        while (searchTasks.Count > 0 && DateTime.Now - startLoopTime < loopTimeout)
        {
            var completedTask = await Task.WhenAny(searchTasks);

            if (completedTask.Result.Count == 0)
            {
                searchTasks.Remove(completedTask);
            }
            else
            {
                var aggregatorResult = new SearchAggregatorResult(searchText, completedTask.Result);

                await _searchContextRepository.AddSearchAggregatorResult(aggregatorResult);

                return Ok(aggregatorResult.SearchResult);
            }
        }

        return Ok(new List<SearchItemBaseModel>());
    }
}