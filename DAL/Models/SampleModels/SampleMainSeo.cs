using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models.SampleModels
{
    /// <summary>
    /// Технический корень для образцовой модели с универсальным SEO
    /// </summary>
    public class SampleMainSeo
    {
        public int Id { get; set; }

        /// <summary>
        /// Техническое название для идентификации
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Системный код
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
        /// Коллекция локализованных данных (включая SEO)
        /// </summary>
        public virtual ICollection<SampleMainDescriptionSeo> Descriptions { get; set; } = new List<SampleMainDescriptionSeo>();
    }
}
