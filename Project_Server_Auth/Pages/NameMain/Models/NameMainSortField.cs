using System.Text.Json.Serialization;

namespace pr_srv_names.Pages.NameMain.Models
{
    /// <summary>
    /// Поля сортировки для namemain
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum NameMainSortField
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
