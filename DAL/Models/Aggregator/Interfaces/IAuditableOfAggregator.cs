using System;

namespace DAL.Models.Aggregator.Interfaces
{
    /// <summary>
    /// Интерфейс аудита с привязкой к пользователю Identity.
    /// </summary>
    public interface IAuditableOfAggregator
    {
        DateTimeOffset CreatedAt { get; }
        DateTimeOffset? UpdatedAt { get; }
    }

}
