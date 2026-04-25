using System.Threading.Tasks;
using pr_srv_names.Pages.AGGREGATOR.LicenseTypeOfAggregator.Dtos;

namespace pr_srv_names.Pages.AGGREGATOR.LicenseTypeOfAggregator.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для управления типами лицензий агрегатора.
    /// </summary>
    public interface ILicenseTypeOfAggregatorService
    {
        /// <summary>
        /// Получить страницу списка типов лицензий с фильтрацией.
        /// </summary>
        Task<LicenseTypeOfAggregatorPagedResponseDto> GetPagedAsync(LicenseTypeOfAggregatorPageRequestDto request);

        /// <summary>
        /// Получить детальную информацию о типе лицензии (с SEO и переводами).
        /// </summary>
        Task<LicenseTypeOfAggregatorDetailDto?> GetByIdAsync(int id);

        /// <summary>
        /// Создать новый тип лицензии.
        /// </summary>
        Task<LicenseTypeOfAggregatorDetailDto> CreateAsync(LicenseTypeOfAggregatorCreateDto dto);

        /// <summary>
        /// Обновить существующий тип лицензии.
        /// </summary>
        Task<LicenseTypeOfAggregatorDetailDto> UpdateAsync(LicenseTypeOfAggregatorUpdateDto dto);

        /// <summary>
        /// Мягкое удаление типа лицензии (Soft Delete).
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// Полное физическое удаление (Hard Delete).
        /// </summary>
        Task HardDeleteAsync(int id);

        /// <summary>
        /// Восстановление мягко удаленного типа лицензии.
        /// </summary>
        Task RestoreAsync(int id);

        /// <summary>
        /// Проверить уникальность CanonicalName.
        /// </summary>
        Task<bool> IsCanonicalNameUniqueAsync(string name, int? excludeId = null);

        /// <summary>
        /// Проверить уникальность Slug.
        /// </summary>
        Task<bool> IsSlugUniqueAsync(string slug, int? excludeId = null);

        /// <summary>
        /// Полная очистка таблицы.
        /// </summary>
        Task<int> ClearAllAsync();

        /// <summary>
        /// Инициализация данных из JSON файла.
        /// </summary>
        Task<int> SeedFromJsonAsync();
    }
}
