using System.ComponentModel.DataAnnotations;

namespace DAL.Enums;

/// <summary>
/// Статусы парсинга URL для имён
/// </summary>
public enum ParsingStatus
{
    /// <summary>
    /// Парсинг ещё не начат
    /// </summary>
    [Display(Name = "Не готово")] NotReady = 0,

    /// <summary>
    /// Парсинг выполняется в данный момент
    /// </summary>
    [Display(Name = "В процессе")] Processing = 1,

    /// <summary>
    /// Парсинг успешно завершён
    /// </summary>
    [Display(Name = "Готово")] Ready = 2,

    /// <summary>
    /// Парсинг завершился с ошибкой
    /// </summary>
    [Display(Name = "Ошибка")] Failed = 3
}