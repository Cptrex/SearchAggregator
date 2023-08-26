namespace SearchAggregator.SearchJsonModels.Google;

public class GoogleResponseModel
{
    public List<GoogleItemModel> Items { get; set; }

    public async Task ReduceGoogleResults(int resultLimit = 10)
    {
        if (Items.Count == 0 || Items.Count <= resultLimit) return;

        Items.RemoveRange(resultLimit, Items.Count - resultLimit - 1);
    }
}