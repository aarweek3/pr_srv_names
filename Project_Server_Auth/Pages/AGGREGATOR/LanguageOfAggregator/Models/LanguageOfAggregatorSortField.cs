using System.Text.Json.Serialization;

namespace pr_srv_names.Pages.Aggregator.LanguageOfAggregator.Models
{
    /// <summary>
    /// Поля для сортировки списка языков агрегатора.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LanguageOfAggregatorSortField
    {
        Id,
        Code,
        ShortCode,
        Title,
        NativeTitle,
        SortOrder,
        Enabled,
        IsDefault
    }
}
