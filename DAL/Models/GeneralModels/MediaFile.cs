using DAL.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.GeneralModels
{
    /// <summary>
    /// Сущность для хранения метаданных медиафайлов (изображений)
    /// </summary>
    [Table("MediaFiles")]
    public class MediaFile : BaseEntity
    {
        /// <summary>
        /// Уникальный GUID изображения (соответствует имени файла на диске)
        /// </summary>
        [Required]
        public Guid ImageId { get; set; }

        /// <summary>
        /// Оригинальное имя файла при загрузке
        /// </summary>
        [MaxLength(255)]
        public string OriginalName { get; set; }

        /// <summary>
        /// Относительный путь к файлу (напр. /uploads/images/2024/01/...)
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string RelativePath { get; set; }

        /// <summary>
        /// MIME-тип контента
        /// </summary>
        [MaxLength(100)]
        public string ContentType { get; set; }

        /// <summary>
        /// Размер файла в байтах
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Ширина изображения в пикселях
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Высота изображения в пикселях
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// ID пользователя, загрузившего файл
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Назначение файла (напр. "editor", "avatar", "preview")
        /// </summary>
        [MaxLength(50)]
        public string Purpose { get; set; } = "editor";
    }
}
