using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    // =================================================================
    // ПРИМЕР 2: Color - использование SimpleLocalizedEntity
    // =================================================================

    /// <summary>
    /// Модель для хранения цветов, связанных с именем
    /// </summary>
    [Table("Colors")]
    [Index(nameof(NameMainId))]
    [Index(nameof(LanguageId))]
    public class Color : SimpleLocalizedEntity
    {
        // Все поля унаследованы
    }
}