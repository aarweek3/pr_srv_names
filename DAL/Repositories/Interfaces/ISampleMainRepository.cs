using DAL.Models.SampleModels;

namespace DAL.Repositories.Interfaces
{
    /// <summary>
    /// Репозиторий для основной сущности SampleMain
    /// </summary>
    public interface ISampleMainRepository : IRepository<SampleMain>
    {
        /// <summary>
        /// Проверка уникальности технического имени
        /// </summary>
        Task<bool> IsNameUniqueAsync(string name, int? excludeId = null);

        /// <summary>
        /// Проверка уникальности системного кода
        /// </summary>
        Task<bool> IsSystemCodeUniqueAsync(string systemCode, int? excludeId = null);

        /// <summary>
        /// Получить запись со всеми переводами по Id
        /// </summary>
        Task<SampleMain?> GetWithDescriptionsAsync(int id);
    }
}
