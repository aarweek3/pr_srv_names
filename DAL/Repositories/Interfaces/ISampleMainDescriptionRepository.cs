using DAL.Models.SampleModels;

namespace DAL.Repositories.Interfaces
{
    /// <summary>
    /// Репозиторий для переводов SampleMainDescription
    /// </summary>
    public interface ISampleMainDescriptionRepository : IRepository<SampleMainDescription>
    {
        /// <summary>
        /// Получить перевод для конкретной записи на конкретном языке
        /// </summary>
        Task<SampleMainDescription?> GetTranslationAsync(int sampleMainId, int languageAppId);

        /// <summary>
        /// Получить все переводы для конкретной записи
        /// </summary>
        Task<IEnumerable<SampleMainDescription>> GetByMainIdAsync(int sampleMainId);
    }
}
