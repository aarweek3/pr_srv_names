using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Модель для хранения металлов, связанных с именем
    /// </summary>
    [Table("Metals")]
    [Index(nameof(NameMainId))]
    [Index(nameof(LanguageId))]
    public class Metal : SimpleLocalizedEntity
    {
    }
}