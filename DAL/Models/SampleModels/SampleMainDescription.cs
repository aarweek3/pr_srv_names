using DAL.Models.LocalizationModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models.SampleModels
{
    /// <summary>
    /// Таблица переводов для SampleMain
    /// </summary>
    public class SampleMainDescription
    {
        public int Id { get; set; }

        /// <summary>
        /// Ссылка на родительскую сущность
        /// </summary>
        [Required]
        public int SampleMainId { get; set; }
        public virtual SampleMain SampleMain { get; set; } = null!;

        /// <summary>
        /// Ссылка на язык перевода
        /// </summary>
        [Required]
        public int LanguageAppId { get; set; }
        public virtual LanguageApp LanguageApp { get; set; } = null!;

        /// <summary>
        /// Локализованное название
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Локализованное описание
        /// </summary>
        public string? Description { get; set; } = string.Empty;

        /// <summary>
        /// Локализованное изображение (если пусто, берется UrlPictureMain из SampleMain)
        /// </summary>
        [MaxLength(500)]
        public string? UrlPicture { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
