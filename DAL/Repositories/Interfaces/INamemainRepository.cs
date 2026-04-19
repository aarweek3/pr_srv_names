using DAL.Models.NameModels;


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