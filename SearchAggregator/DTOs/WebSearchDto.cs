namespace SearchAggregator.DTOs;

public class WebSearchDto
{
    public List<GoogleDto> GoogleResults { get; set; }
    public List<YandexDto> YandexResults { get; set; }
    public List<BingDto> BingResults { get; set; }
}