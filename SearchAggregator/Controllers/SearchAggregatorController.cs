using Microsoft.AspNetCore.Mvc;

namespace SearchAggregator.Controllers;

[ApiController]
[Route("api/v1/aggregator")]
public class SearchAggregatorController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ISearchContextRepository _searchContextRepository;

    public SearchAggregatorController(IHttpClientFactory httpClientFactory, ISearchContextRepository repository)
    {
        _httpClientFactory = httpClientFactory;
        _searchContextRepository = repository;
    }

    [HttpGet]
    [Route("search")]
    public async Task<IActionResult> SearchInWeb([FromQuery] string searchText, CancellationToken cancellationToken)
    {
        var foundSearchResults = await _searchContextRepository.GetAggregatorResultBySearchText(searchText);
        if (foundSearchResults == null) 
        {
            await SearchEngineManager.SearchViaGoogle(_httpClientFactory, searchText);
            await SearchEngineManager.SearchViaYandex(_httpClientFactory, searchText);
            await SearchEngineManager.SearchViaBing(_httpClientFactory, searchText);
        }
        return Ok();
    }
}