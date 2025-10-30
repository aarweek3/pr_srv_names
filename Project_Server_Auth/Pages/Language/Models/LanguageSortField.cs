using System.Text.Json.Serialization;

namespace pr_srv_names.Pages.Language.Models
{
    /// <summary>
    /// Поля сортировки для language
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LanguageSortField
    {
        /// <summary>
        /// Сортировка по коду языка
        /// </summary>
        Code,
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