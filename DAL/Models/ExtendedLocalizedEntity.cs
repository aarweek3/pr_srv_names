using DAL.Constants;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    /// <summary>
    /// Базовый класс для расширенных локализованных сущностей
    /// Используется для сущностей с дополнительными полями (например, URL)
    /// </summary>
    public abstract class ExtendedLocalizedEntity : LocalizedEntity
    {
        /// <summary>
        /// URL ссылка на внешний ресурс
        /// </summary>
        [StringLength(StringLengths.Url)]
        [Url]
        public string? Url { get; set; }
    }
}