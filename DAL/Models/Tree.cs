using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Модель для хранения деревьев, связанных с именем
    /// </summary>
    [Table("Trees")]
    [Index(nameof(NameMainId))]
    [Index(nameof(LanguageId))]
    public class Tree : SimpleLocalizedEntity
    {
    }
}