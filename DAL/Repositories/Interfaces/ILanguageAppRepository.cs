using DAL.Models.LocalizationModels;

namespace DAL.Repositories.Interfaces
{
    public interface ILanguageAppRepository : IRepository<LanguageApp>
    {
        Task<bool> IsCodeUniqueAsync(string code, int? excludeId = null);
        Task<bool> IsShortCodeUniqueAsync(string shortCode, int? excludeId = null);
        Task<LanguageApp?> GetDefaultLanguageAsync();
    }
}
