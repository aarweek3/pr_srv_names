using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Модель для хранения синонимов имени
    /// </summary>
    [Table("Synonyms")]
    [Index(nameof(NameMainId))]
    [Index(nameof(LanguageId))]
    public class Synonym : SimpleLocalizedEntity
    {
    }
}