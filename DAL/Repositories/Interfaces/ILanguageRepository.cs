using DAL.Models;
using DAL.Repositories.Interfaces.DAL.Repositories.Interfaces;


namespace DAL.Repositories.Interfaces
{
    public interface ILanguageRepository : IRepository<Language>
    {
        Task<IEnumerable<Language>> GetAllAsync();
        Task<IEnumerable<Language>> GetLanguagesWithDescriptionAsync();
        Task<Language?> GetLanguageByNameAsync(string name);
        Task<bool> IsLanguageNameUniqueAsync(string name, int? excludeId = null);
        Task<bool> IsLanguageCodeUniqueAsync(string code, int? excludeId = null);
        Task<IEnumerable<Language>> SearchCategoriesByNameAsync(string searchTerm);
    }
}