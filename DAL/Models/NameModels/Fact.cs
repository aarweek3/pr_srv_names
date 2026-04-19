using DAL.Models.LocalizationModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.NameModels
{
    // =================================================================
    // ПРИМЕР 4: Fact - использование SimpleLocalizedEntity
    // =================================================================

    /// <summary>
    /// Модель для хранения фактов об имени
    /// </summary>
    [Table("Facts")]
    [Index(nameof(NameMainId))]
    [Index(nameof(LanguageId))]
    public class Fact : SimpleLocalizedEntity
    {
        // Все поля унаследованы
    }
}