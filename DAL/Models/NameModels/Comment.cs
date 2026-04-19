using DAL.Constants;
using DAL.Models.LocalizationModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.NameModels
{
    // =================================================================
    // ПРИМЕР 5: Comment - кастомизация базового класса
    // =================================================================

    /// <summary>
    /// Модель для хранения комментариев пользователей об именах
    /// </summary>
    [Table("Comments")]
    [Index(nameof(NameMainId))]
    [Index(nameof(LanguageId))]
    [Index(nameof(CreatedAt))] // Дополнительный индекс для сортировки
    public class Comment : LocalizedEntity
    {
        /// <summary>
        /// Email автора комментария (опционально)
        /// </summary>
        [StringLength(StringLengths.Email)]
        [EmailAddress]
        public string? AuthorEmail { get; set; }

        /// <summary>
        /// Рейтинг комментария (количество лайков)
        /// </summary>
        public int Rating { get; set; } = 0;

        /// <summary>
        /// Флаг модерации - одобрен ли комментарий
        /// </summary>
        public bool IsApproved { get; set; } = false;

        /// <summary>
        /// ID родительского комментария (для вложенных комментариев)
        /// </summary>
        public int? ParentCommentId { get; set; }

        /// <summary>
        /// Навигационное свойство для родительского комментария
        /// </summary>
        [ForeignKey(nameof(ParentCommentId))]
        public virtual Comment? ParentComment { get; set; }

        /// <summary>
        /// Навигационное свойство для дочерних комментариев
        /// </summary>
        public virtual ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }
}