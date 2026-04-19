using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models.SampleModels
{
    /// <summary>
    /// Основная сущность (заголовок), не зависящая от языка.
    /// Содержит техническое имя для идентификации в админке.
    /// </summary>
    public class SampleMain
    {
        public int Id { get; set; }

        /// <summary>
        /// Техническое название (обычно на английском) для удобства идентификации в списке
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Системный код (не переводится) для использования в логике приложения
        /// </summary>
        [MaxLength(100)]
        public string? SystemCode { get; set; }

        /// <summary>
        /// Главное изображение (по умолчанию для всех языков)
        /// </summary>
        [MaxLength(500)]
        public string? UrlPictureMain { get; set; }

        /// <summary>
        /// Флаг активности
        /// </summary>
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Коллекция переводов на разные языки
        /// </summary>
        public virtual ICollection<SampleMainDescription> Descriptions { get; set; } = new List<SampleMainDescription>();
    }
}
