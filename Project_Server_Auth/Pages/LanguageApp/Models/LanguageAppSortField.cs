using System.Text.Json.Serialization;

namespace pr_srv_names.Pages.LanguageApp.Models
{
    /// <summary>
    /// Поля сортировки для language
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LanguageAppSortField
    {
        Code,
        Title,
        NativeTitle,
        SortOrder,
        Id
    }
}
