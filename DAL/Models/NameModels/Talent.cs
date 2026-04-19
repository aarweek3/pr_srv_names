using DAL.Models.LocalizationModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.NameModels
{
    /// <summary>
    /// Модель для хранения талантов, связанных с именем
    /// </summary>
    [Table("Talents")]
    [Index(nameof(NameMainId))]
    [Index(nameof(LanguageId))]
    public class Talent : SimpleLocalizedEntity
    {
    }
}