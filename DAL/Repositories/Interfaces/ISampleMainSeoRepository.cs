using DAL.Models.SampleModels;

namespace DAL.Repositories.Interfaces
{
    /// <summary>
    /// Репозиторий для SampleMainSeo
    /// </summary>
    public interface ISampleMainSeoRepository : IRepository<SampleMainSeo>
    {
        Task<bool> IsNameUniqueAsync(string name, int? excludeId = null);
        Task<bool> IsSystemCodeUniqueAsync(string systemCode, int? excludeId = null);
        Task<SampleMainSeo?> GetWithDescriptionsAndSeoAsync(int id);
    }
}
