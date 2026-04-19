using DAL.Models.LocalizationModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.NameModels
{
    // =================================================================
    // ПРИМЕР 3: ForeignVariant - использование ExtendedLocalizedEntity
    // =================================================================

    /// <summary>
    /// Модель для хранения вариантов имени на других языках
    /// </summary>
    [Table("ForeignVariants")]
    [Index(nameof(NameMainId))]
    [Index(nameof(LanguageId))]
    public class ForeignVariant : ExtendedLocalizedEntity
    {
        // Наследует все поля включая Url от ExtendedLocalizedEntity
    }
}