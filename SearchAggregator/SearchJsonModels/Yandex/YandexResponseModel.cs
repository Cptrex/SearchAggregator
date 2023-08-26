namespace SearchAggregator.SearchJsonModels.Yandex;

public class YandexResponseModel
{
    public List<YandexItemModel> Items { get; set; }

    public async Task ReduceYandexResults(int resultLimit = 10)
    {
        if (Items.Count == 0 || Items.Count <= resultLimit) return;

        Items.RemoveRange(resultLimit, Items.Count - resultLimit - 1);
    }
}