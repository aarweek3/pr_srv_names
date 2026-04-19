using System;
using DAL.Models.Aggregator.Interfaces;

namespace DAL.Models.Aggregator.Base
{
    /// <summary>
    /// Полная модель с аудитом и мягким удалением.
    /// </summary>
    public abstract class FullAuditableEntityOfAggregator : AuditableEntityOfAggregator, ISoftDeletableOfAggregator
    {
        public bool IsDeleted { get; protected set; }
        public DateTimeOffset? DeletedAt { get; protected set; }

        protected FullAuditableEntityOfAggregator() { }

        public virtual void Delete()
        {
            if (IsDeleted) return;

            IsDeleted = true;
            DeletedAt = DateTimeOffset.UtcNow;

            SetUpdated();
        }

        public virtual void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
        }
    }

}
