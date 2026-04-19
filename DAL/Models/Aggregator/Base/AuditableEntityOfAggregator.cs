using System;
using DAL.Models.Aggregator.Interfaces;

namespace DAL.Models.Aggregator.Base
{
    /// <summary>
    /// Базовая модель с аудитом.
    /// </summary>
    public abstract class AuditableEntityOfAggregator : BaseEntityOfAggregator, IAuditableOfAggregator
    {
        public DateTimeOffset CreatedAt { get; protected set; }
        public DateTimeOffset? UpdatedAt { get; protected set; }

        protected AuditableEntityOfAggregator() { }

        /// <summary>Вызывается из AuditInterceptor при создании сущности</summary>
        protected internal void SetCreated()
        {
            CreatedAt = DateTimeOffset.UtcNow;
        }

        /// <summary>Вызывается из AuditInterceptor и бизнес-логики при обновлении</summary>
        protected internal void SetUpdated()
        {
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }

}
