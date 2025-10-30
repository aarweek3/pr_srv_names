using System.Text.Json.Serialization;

namespace pr_srv_names.Pages.Anecdote.Models
{
    /// <summary>
    /// Поля сортировки для анекдотов
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AnecdoteSortField
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
        Id,

        /// <summary>
        /// Сортировка по имени (NameMain)
        /// </summary>
        NameMain,

        /// <summary>
        /// Сортировка по языку
        /// </summary>
        Language
    }
}
