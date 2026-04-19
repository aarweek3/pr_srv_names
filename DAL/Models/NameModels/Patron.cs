using DAL.Models.LocalizationModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.NameModels
{
    /// <summary>
    /// Модель для хранения покровителей имени
    /// </summary>
    [Table("Patrons")]
    [Index(nameof(NameMainId))]
    [Index(nameof(LanguageId))]
    public class Patron : SimpleLocalizedEntity
    {
    }
}