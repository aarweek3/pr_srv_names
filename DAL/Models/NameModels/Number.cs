using DAL.Models.LocalizationModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.NameModels
{
    /// <summary>
    /// Модель для хранения чисел, связанных с именем
    /// </summary>
    [Table("Numbers")]
    [Index(nameof(NameMainId))]
    [Index(nameof(LanguageId))]
    public class Number : SimpleLocalizedEntity
    {
    }
}