using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DAL.Models.Base
{
    /// <summary>
    /// Базовый класс для всех сущностей с аудитом
    /// Содержит общие поля: Id, CreatedAt, UpdatedAt, IsActive
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Уникальный идентификатор сущности
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Дата создания записи
        /// Автоматически заполняется при создании
        /// </summary>
        [Required]
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Дата последнего обновления записи
        /// Автоматически обновляется при изменении
        /// </summary>
        [Column("UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Флаг активности записи
        /// Используется для мягкого удаления
        /// </summary>
        [Required]
        [Column("IsActive")]
        public bool IsActive { get; set; } = true;
    }
}