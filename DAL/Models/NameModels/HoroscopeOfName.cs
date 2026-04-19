using DAL.Constants;
using DAL.Models.LocalizationModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.NameModels
{
    /// <summary>
    /// Модель для хранения гороскопов имени
    /// УЛУЧШЕНО: добавлен ZodiacId для связи со знаком зодиака
    /// </summary>
    [Table("HoroscopesOfNames")]
    [Index(nameof(NameMainId))]
    [Index(nameof(LanguageId))]
    [Index(nameof(ZodiacId))]
    public class HoroscopeOfName : LocalizedEntity
    {
        /// <summary>
        /// Внешний ключ на знак зодиака
        /// </summary>
        [Required]
        [ForeignKey("Zodiac")]
        public int ZodiacId { get; set; }

        /// <summary>
        /// Навигационное свойство - связь со знаком зодиака
        /// </summary>
        public virtual Zodiac Zodiac { get; set; } = null!;

        /// <summary>
        /// Дата гороскопа (опционально, для ежедневных/еженедельных гороскопов)
        /// </summary>
        public DateTime? HoroscopeDate { get; set; }

        /// <summary>
        /// Тип гороскопа (ежедневный, еженедельный, годовой)
        /// </summary>
        [StringLength(StringLengths.ShortName)]
        public string? HoroscopeType { get; set; }
    }
}