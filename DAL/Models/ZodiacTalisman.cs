using DAL.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Модель для хранения талисманов по знакам зодиака
    /// </summary>
    [Table("ZodiacTalismans")]
    [Index(nameof(ZodiacId))]
    [Index(nameof(NameMainId))]
    [Index(nameof(LanguageId))]
    public class ZodiacTalisman : LocalizedEntity
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
        /// Тип талисмана (камень, металл, растение и т.д.)
        /// </summary>
        [StringLength(StringLengths.ShortName)]
        public string? TalismanType { get; set; }
    }
}