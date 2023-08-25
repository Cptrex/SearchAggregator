namespace SearchAggregator.Models;

public class SearchAggregatorResult
{
    public int Id { get; set; }
    public string SearchText { get; set; }
    public string GoogleResult { get; set; }
    public string YandexResult { get; set; }
    public string BingResult { get; set; }
}