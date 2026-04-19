using DAL.Models.SampleModels;


namespace DAL.Repositories.Interfaces
{
    public interface ISampleRepository : IRepository<Sample>
    {
        Task<IEnumerable<Sample>> GetAllAsync();
        Task<IEnumerable<Sample>> GetSamplesWithDescriptionAsync();
        Task<Sample?> GetSampleByNameAsync(string name);
        Task<bool> IsSampleNameUniqueAsync(string name, int? excludeId = null);
        Task<IEnumerable<Sample>> SearchCategoriesByNameAsync(string searchTerm);
    }
}