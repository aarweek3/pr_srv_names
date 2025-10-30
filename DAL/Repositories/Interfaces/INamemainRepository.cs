using DAL.Models;
using DAL.Repositories.Interfaces.DAL.Repositories.Interfaces;

namespace DAL.Repositories.Interfaces
{
    public interface INameMainRepository : IRepository<NameMain>
    {
        Task<IEnumerable<NameMain>> GetAllAsync();
        Task<IEnumerable<NameMain>> GetNameMainsWithDescriptionAsync();
        Task<NameMain?> GetNameMainByNameAsync(string name);
        Task<bool> IsNameMainNameUniqueAsync(string name, int? excludeId = null);
        Task<IEnumerable<NameMain>> SearchCategoriesByNameAsync(string searchTerm);
    }
}