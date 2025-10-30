using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    // =================================================================
    // ПРИМЕР 6: Исправленная модель ZodiacHoroscope
    // =================================================================

    /// <summary>
    /// Модель для хранения гороскопов имени по знакам зодиака
    /// ИСПРАВЛЕНО: теперь связана с NameMain, а не с NameDetail
    /// </summary>
    [Table("ZodiacHoroscopes")]
    [Index(nameof(NameMainId))]
    [Index(nameof(LanguageId))]
    [Index(nameof(ZodiacId))]
    public class ZodiacHoroscope : LocalizedEntity
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
    }
}