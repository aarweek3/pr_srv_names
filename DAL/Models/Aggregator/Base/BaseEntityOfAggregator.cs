using System.ComponentModel.DataAnnotations;

namespace DAL.Models.Aggregator.Base
{
    /// <summary>
    /// Единый корень для всех сущностей.
    /// </summary>
    public abstract class BaseEntityOfAggregator
    {
        [Key]
        public int Id { get; protected set; }

        protected BaseEntityOfAggregator() { }
    }
}
