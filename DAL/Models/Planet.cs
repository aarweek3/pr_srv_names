using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Модель для хранения планет-покровителей
    /// </summary>
    [Table("Planets")]
    [Index(nameof(NameMainId))]
    [Index(nameof(LanguageId))]
    public class Planet : SimpleLocalizedEntity
    {
        // Можно добавить специфичные поля
        /// <summary>
        /// Астрономический символ планеты
        /// </summary>
        [StringLength(10)]
        public string? Symbol { get; set; }
    }
}