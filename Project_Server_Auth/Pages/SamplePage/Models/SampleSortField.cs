using System.Text.Json.Serialization;

namespace pr_srv_names.Pages.Sample.Models
{
    /// <summary>
    /// Поля сортировки для sample
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SampleSortField
    {
        /// <summary>
        /// Сортировка по названию
        /// </summary>
        Name,

        /// <summary>
        /// Сортировка по описанию
        /// </summary>
        Description,

        /// <summary>
        /// Сортировка по идентификатору
        /// </summary>
        Id
    }
}
