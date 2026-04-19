using System.Threading.Tasks;
using pr_srv_names.Pages.AGGREGATOR.PlatformOfAggregator.Dtos;

namespace pr_srv_names.Pages.AGGREGATOR.PlatformOfAggregator.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для управления платформами агрегатора.
    /// </summary>
    public interface IPlatformOfAggregatorService
    {
        /// <summary>
        /// Получить страницу списка платформ с фильтрацией.
        /// </summary>
        Task<PlatformOfAggregatorPagedResponseDto> GetPagedAsync(PlatformOfAggregatorPageRequestDto request);

        /// <summary>
        /// Получить детальную информацию о платформе (с SEO и переводами).
        /// </summary>
        Task<PlatformOfAggregatorDetailDto?> GetByIdAsync(int id);

        /// <summary>
        /// Создать новую платформу.
        /// </summary>
        Task<PlatformOfAggregatorDetailDto> CreateAsync(PlatformOfAggregatorCreateDto dto);

        /// <summary>
        /// Обновить существующую платформу.
        /// </summary>
        Task<PlatformOfAggregatorDetailDto> UpdateAsync(PlatformOfAggregatorUpdateDto dto);

        /// <summary>
        /// Мягкое удаление платформы (Soft Delete).
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// Полное физическое удаление платформы и всех зависимостей (Hard Delete).
        /// </summary>
        Task HardDeleteAsync(int id);

        /// <summary>
        /// Восстановление мягко удаленной платформы.
        /// </summary>
        Task RestoreAsync(int id);

        /// <summary>
        /// Проверить уникальность системного названия (Name).
        /// </summary>
        Task<bool> IsNameUniqueAsync(string name, int? excludeId = null);

        /// <summary>
        /// Проверить уникальность системного кода (SystemCode).
        /// </summary>
        Task<bool> IsSystemCodeUniqueAsync(string code, int? excludeId = null);

        /// <summary>
        /// Полная очистка таблицы платформ (с TRUNCATE CASCADE для Postgres).
        /// </summary>
        Task<int> ClearAllAsync();

        /// <summary>
        /// Инициализация данных из JSON файла.
        /// </summary>
        Task<int> SeedFromJsonAsync();
    }
}
