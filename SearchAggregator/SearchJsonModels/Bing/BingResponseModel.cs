namespace SearchAggregator.SearchJsonModels.Bing;

public class BingResponseModel
{
    public List<BingItemModel> Items { get; set; }

    public async Task ReduceBingResults(int resultLimit = 10)
    {
        if (Items.Count == 0 || Items.Count <= resultLimit) return;

        Items.RemoveRange(resultLimit, Items.Count - resultLimit - 1);
    }
}