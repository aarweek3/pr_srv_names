using System.Text.Json.Serialization;

namespace pr_srv_names.Models
{
    /// <summary>
    /// Направления сортировки
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortDirection
    {
        /// <summary>
        /// По возрастанию
        /// </summary>
        Asc,

        /// <summary>
        /// По убыванию
        /// </summary>
        Desc
    }
}
