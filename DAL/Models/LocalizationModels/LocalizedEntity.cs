using DAL.Constants;
using DAL.Models.Base;
using DAL.Models.NameModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.LocalizationModels
{
    /// <summary>
    /// Базовый класс для локализованных сущностей
    /// Используется для всех моделей, связанных с NameMain и Language
    /// </summary>
    public abstract class LocalizedEntity : BaseEntity
    {
        /// <summary>
        /// Название сущности
        /// </summary>
        [Required]
        [StringLength(StringLengths.Name)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Описание сущности
        /// </summary>
        [StringLength(StringLengths.Description)]
        public string? Description { get; set; }

        /// <summary>
        /// Внешний ключ на таблицу Names (NameMain)
        /// Указывает, к какому имени относится эта сущность
        /// </summary>
        [Required]
        [ForeignKey("NameMain")]
        public int NameMainId { get; set; }

        /// <summary>
        /// Навигационное свойство - ссылка на основное имя
        /// </summary>
        public virtual NameMain NameMain { get; set; } = null!;

        /// <summary>
        /// Внешний ключ на таблицу Languages
        /// Указывает, на каком языке написаны данные
        /// </summary>
        [Required]
        [ForeignKey("Language")]
        public int LanguageId { get; set; }

        /// <summary>
        /// Навигационное свойство - ссылка на язык
        /// </summary>
        public virtual Language Language { get; set; } = null!;
    }
}