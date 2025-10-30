using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Модель для хранения камней-талисманов
    /// </summary>
    [Table("Stones")]
    [Index(nameof(NameMainId))]
    [Index(nameof(LanguageId))]
    public class Stone : SimpleLocalizedEntity
    {
    }
}