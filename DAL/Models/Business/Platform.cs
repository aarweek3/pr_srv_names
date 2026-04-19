using DAL.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Business
{
    [Table("platform")]
    public class Platform
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Техническое название (для идентификации в коде и админке, не переводится)
        /// </summary>
        [Required]
        [MaxLength(PlatformConstants.NameMaxLength)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Системный код (windows, macos, android и т.д.)
        /// </summary>
        [Required]
        [MaxLength(PlatformConstants.CodeMaxLength)]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Семейство платформ (desktop, mobile, web)
        /// </summary>
        [MaxLength(PlatformConstants.FamilyMaxLength)]
        public string? Family { get; set; }

        /// <summary>
        /// Главное изображение (по умолчанию для всех языков)
        /// </summary>
        [MaxLength(500)]
        public string? UrlPictureMain { get; set; }

        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 100;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Коллекция переводов на разные языки и SEO данные
        /// </summary>
        public virtual ICollection<PlatformTranslation> Translations { get; set; } = new List<PlatformTranslation>();

        public virtual ICollection<SoftwareVersionPlatform> VersionPlatforms { get; set; } = new List<SoftwareVersionPlatform>();
    }


}
