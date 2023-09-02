namespace SearchAggregator.SearchJsonModels.Bing;

public class BingItemModel : SearchItemBaseModel
{
    public string Name { get; set; }
    public string Url { get; set; }
    public string Snippet { get; set; }
}