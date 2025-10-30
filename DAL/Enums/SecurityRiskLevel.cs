using System.ComponentModel.DataAnnotations;

namespace DAL.Enums;

/// <summary>
/// Уровни риска безопасности
/// Используется для оценки серьёзности угроз
/// </summary>
public enum SecurityRiskLevel
{
    /// <summary>
    /// Низкий уровень риска
    /// </summary>
    [Display(Name = "Низкий")] Low = 0,

    /// <summary>
    /// Средний уровень риска
    /// </summary>
    [Display(Name = "Средний")] Medium = 1,

    /// <summary>
    /// Высокий уровень риска
    /// </summary>
    [Display(Name = "Высокий")] High = 2,

    /// <summary>
    /// Критический уровень риска
    /// </summary>
    [Display(Name = "Критический")] Critical = 3
}