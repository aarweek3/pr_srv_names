using System.ComponentModel.DataAnnotations;
using DAL.Models.Base;

namespace DAL.Models.GeneralModels
{
    /// <summary>
    /// Модель иконки (хранение SVG контента в БД)
    /// </summary>
    public class Icon : BaseEntity
    {
        /// <summary>
        /// Уникальное имя иконки (например, "av_save")
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Полный XML контент SVG иконки
        /// </summary>
        [Required]
        public string SvgContent { get; set; } = string.Empty;

        /// <summary>
        /// Теги для поиска (опционально)
        /// </summary>
        [MaxLength(500)]
        public string? Tags { get; set; }

        /// <summary>
        /// Ссылка на категорию (раздел)
        /// </summary>
        public int CategoryId { get; set; }

        public IconCategory Category { get; set; } = null!;
    }
}
