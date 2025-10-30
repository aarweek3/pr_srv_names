using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

/// <summary>
/// Модель для хранения тотемных животных
/// </summary>
[Table("Animals")]
[Index(nameof(NameMainId))]
[Index(nameof(LanguageId))]
public class Animal : SimpleLocalizedEntity
{
}