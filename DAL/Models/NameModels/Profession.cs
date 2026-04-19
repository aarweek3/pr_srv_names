using DAL.Models.LocalizationModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.NameModels
{
    /// <summary>
    /// Модель для хранения профессий, подходящих для имени
    /// </summary>
    [Table("Professions")]
    [Index(nameof(NameMainId))]
    [Index(nameof(LanguageId))]
    public class Profession : SimpleLocalizedEntity
    {
    }
}