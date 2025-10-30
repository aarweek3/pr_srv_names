using DAL.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    // =================================================================
    // ПРИМЕР 8: Улучшенная модель Declension
    // =================================================================

    /// <summary>
    /// Модель для хранения склонений имени по падежам
    /// </summary>
    [Table("Declensions")]
    [Index(nameof(NameMainId))]
    [Index(nameof(LanguageId))]
    public class Declension : LocalizedEntity
    {
        /// <summary>
        /// Именительный падеж: кто? что?
        /// </summary>
        [StringLength(StringLengths.Name)]
        public string? Nominative { get; set; }

        /// <summary>
        /// Родительный падеж: кого? чего?
        /// </summary>
        [StringLength(StringLengths.Name)]
        public string? Genitive { get; set; }

        /// <summary>
        /// Дательный падеж: кому? чему?
        /// </summary>
        [StringLength(StringLengths.Name)]
        public string? Dative { get; set; }

        /// <summary>
        /// Винительный падеж: кого? что?
        /// </summary>
        [StringLength(StringLengths.Name)]
        public string? Accusative { get; set; }

        /// <summary>
        /// Творительный падеж: кем? чем?
        /// </summary>
        [StringLength(StringLengths.Name)]
        public string? Instrumental { get; set; }

        /// <summary>
        /// Предложный падеж: о ком? о чём?
        /// </summary>
        [StringLength(StringLengths.Name)]
        public string? Prepositional { get; set; }
    }
}