using DAL.Models.LocalizationModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.NameModels
{
    /// <summary>
    /// Модель для хранения растений, связанных с именем
    /// </summary>
    [Table("Plants")]
    [Index(nameof(NameMainId))]
    [Index(nameof(LanguageId))]
    public class Plant : SimpleLocalizedEntity
    {
    }
}