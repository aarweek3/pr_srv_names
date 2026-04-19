using System;

namespace DAL.Models.Aggregator.Interfaces
{
    /// <summary>
    /// Интерфейс мягкого удаления.
    /// </summary>
    public interface ISoftDeletableOfAggregator
    {
        bool IsDeleted { get; }
        DateTimeOffset? DeletedAt { get; }

        void Delete();
        void Restore();
    }

}
