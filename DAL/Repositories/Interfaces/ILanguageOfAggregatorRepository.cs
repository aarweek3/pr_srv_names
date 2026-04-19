using DAL.Models.Aggregator;

namespace DAL.Repositories.Interfaces
{
    /// <summary>
    /// Репозиторий для работы с языками агрегатора.
    /// </summary>
    public interface ILanguageOfAggregatorRepository : IRepository<LanguageOfAggregator>
    {
        /// <summary>
        /// Проверка уникальности полного кода языка (например, "ru-RU").
        /// </summary>
        Task<bool> IsCodeUniqueAsync(string code, int? excludeId = null);

        /// <summary>
        /// Проверка уникальности краткого кода языка (например, "ru").
        /// </summary>
        Task<bool> IsShortCodeUniqueAsync(string shortCode, int? excludeId = null);

        /// <summary>
        /// Получение языка, установленного по умолчанию.
        /// </summary>
        Task<LanguageOfAggregator?> GetDefaultLanguageAsync();
    }
}
