// =================================================================
// модель Language
// =================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Constants;
using DAL.Models.Base;
using DAL.Models.NameModels;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models.LocalizationModels;

/// <summary>
/// Модель языка для многоязычной поддержки сайта
/// </summary>
[Table("Languages")]
[Index(nameof(Code), IsUnique = true)]
[Index(nameof(IsActive))]
[Index(nameof(DisplayOrder))]
public class Language : BaseEntity
{
    /// <summary>
    /// Название языка на родном языке
    /// Примеры: "English", "Русский", "Español"
    /// </summary>
    [Required]
    [StringLength(StringLengths.Name)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание языка (опционально)
    /// </summary>
    [StringLength(StringLengths.Description)]
    public string? Description { get; set; }

    /// <summary>
    /// Код языка по стандарту ISO 639-1 (двухбуквенный код)
    /// Примеры: "en", "ru", "es", "fr", "de", "it", "pt"
    /// </summary>
    [Required]
    [StringLength(2, MinimumLength = 2)]
    [RegularExpression(@"^[a-z]{2}$", ErrorMessage = "Код языка должен состоять из 2 строчных букв")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Код флага (ISO 3166-1 alpha-2)
    /// Примеры: "gb", "ru", "es"
    /// Используется для отображения иконки флага на фронтенде
    /// </summary>
    [StringLength(2)]
    [RegularExpression(@"^[a-z]{2}$")]
    public string? FlagCode { get; set; }

    /// <summary>
    /// Порядок отображения языка в списке
    /// Меньшее значение = выше в списке
    /// </summary>
    public int DisplayOrder { get; set; } = 999;

    /// <summary>
    /// Является ли язык языком по умолчанию
    /// </summary>
    public bool IsDefault { get; set; } = false;

    /// <summary>
    /// Направление текста (LTR или RTL)
    /// </summary>
    [StringLength(3)]
    public string TextDirection { get; set; } = "ltr";

    /// <summary>
    /// Навигационное свойство - коллекция всех описаний имён на этом языке
    /// </summary>
    public virtual ICollection<NameDetail> NameDetails { get; set; } = new List<NameDetail>();
}