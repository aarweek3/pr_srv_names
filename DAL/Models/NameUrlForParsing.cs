using DAL.Constants;
using DAL.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

/// <summary>
/// Модель для хранения URL для парсинга информации об именах
/// </summary>
[Table("NameUrlForParsings")]
[Index(nameof(NameMainId))]
[Index(nameof(LanguageId))]
[Index(nameof(Status))]
[Index(nameof(NameMainId), nameof(LanguageId), IsUnique = true)]
public class NameUrlForParsing : ExtendedLocalizedEntity
{
    // Унаследовано от ExtendedLocalizedEntity:
    // - Id, CreatedAt, UpdatedAt, IsActive
    // - Name, Description
    // - NameMainId + NameMain, LanguageId + Language
    // - Url ⭐ (это ключевое поле для парсинга!)

    /// <summary>
    /// Статус парсинга URL
    /// </summary>
    [Required]
    public ParsingStatus Status { get; set; } = ParsingStatus.NotReady;

    /// <summary>
    /// Дата последнего успешного парсинга
    /// </summary>
    public DateTime? LastParsedAt { get; set; }

    /// <summary>
    /// Количество попыток парсинга
    /// Используется для отслеживания проблемных URL
    /// </summary>
    public int ParseAttempts { get; set; } = 0;

    /// <summary>
    /// Сообщение об ошибке при неудачном парсинге
    /// </summary>
    [StringLength(StringLengths.Description)]
    public string? ErrorMessage { get; set; }

    // Вычисляемые свойства

    /// <summary>
    /// Проверяет, готов ли URL к парсингу
    /// </summary>
    [NotMapped]
    public bool IsReadyForParsing => Status == ParsingStatus.NotReady || Status == ParsingStatus.Failed;

    /// <summary>
    /// Проверяет, выполняется ли парсинг в данный момент
    /// </summary>
    [NotMapped]
    public bool IsProcessing => Status == ParsingStatus.Processing;

    /// <summary>
    /// Проверяет, успешно ли завершён парсинг
    /// </summary>
    [NotMapped]
    public bool IsCompleted => Status == ParsingStatus.Ready;

    /// <summary>
    /// Проверяет, провалился ли парсинг
    /// </summary>
    [NotMapped]
    public bool HasFailed => Status == ParsingStatus.Failed;
}