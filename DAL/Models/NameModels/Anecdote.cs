using DAL.Models.LocalizationModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.NameModels;

/// <summary>
/// Модель для хранения анекдотов и интересных историй об имени
/// </summary>
[Table("Anecdotes")]
[Index(nameof(NameMainId))]
[Index(nameof(LanguageId))]
public class Anecdote : SimpleLocalizedEntity
{
    // Все поля унаследованы от SimpleLocalizedEntity
}