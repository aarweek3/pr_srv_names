using pr_srv_names.Pages.Sample.Dtos;

namespace pr_srv_names.Pages.Sample.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с Sample
    /// </summary>
    public interface ISampleService
    {
        /// <summary>
        /// Получить Sample по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор Sample</param>
        /// <returns>Детальная информация о Sample или null, если не найдена</returns>
        Task<SampleDetailDto?> GetSampleByIdAsync(int id);

        /// <summary>
        /// Получить список Sample с пагинацией и фильтрацией
        /// </summary>
        /// <param name="request">Параметры запроса с пагинацией и фильтрацией</param>
        /// <returns>Страница Sample</returns>
        Task<SamplePagedResponseDto> GetAllSamplesAsync(SamplePageRequestDto request);

        /// <summary>
        /// Создать новую Sample
        /// </summary>
        /// <param name="request">Данные для создания Sample</param>
        /// <returns>Созданная Sample</returns>
        Task<SampleDetailDto> CreateSampleAsync(SampleCreateRequestDto request);

        /// <summary>
        /// Обновить существующую Sample
        /// </summary>
        /// <param name="id">Идентификатор Sample</param>
        /// <param name="request">Данные для обновления Sample</param>
        /// <returns>Обновленная Sample</returns>
        Task<SampleDetailDto> UpdateSampleAsync(int id, SampleUpdateRequestDto request);

        /// <summary>
        /// Удалить Sample по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор Sample</param>
        /// <returns>True, если Sample была удалена</returns>
        Task<bool> DeleteSampleAsync(int id);

        /// <summary>
        /// Получить Sample, у которых есть описание
        /// </summary>
        /// <returns>Список Sample с описанием</returns>
        Task<IEnumerable<SampleDetailDto>> GetSamplesWithDescriptionAsync();

        /// <summary>
        /// Получить Sample по имени
        /// </summary>
        /// <param name="name">Имя Sample</param>
        /// <returns>Sample или null, если не найдена</returns>
        Task<SampleDetailDto?> GetSampleByNameAsync(string name);

        /// <summary>
        /// Поиск Sample по части имени
        /// </summary>
        /// <param name="searchTerm">Поисковый термин</param>
        /// <returns>Список подходящих Sample</returns>
        Task<IEnumerable<SampleDetailDto>> SearchSamplesByNameAsync(string searchTerm);

        /// <summary>
        /// Проверить существование Sample по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор Sample</param>
        /// <returns>True, если Sample существует</returns>
        Task<bool> SampleExistsAsync(int id);

        /// <summary>
        /// Проверить уникальность имени Sample
        /// </summary>
        /// <param name="name">Имя Sample</param>
        /// <param name="excludeId">ID Sample для исключения из проверки</param>
        /// <returns>True, если имя уникально</returns>
        Task<bool> IsSampleNameUniqueAsync(string name, int? excludeId = null);

        /// <summary>
        /// Получить все Sample в алфавитном порядке (для селектора)
        /// </summary>
        /// <returns>Список всех Sample, отсортированных по имени</returns>
        Task<IEnumerable<SampleDetailDto>> GetAllSamplesAsync();

        // --- Control Methods (Simplified) ---

        /// <summary>
        /// Получить все Sample для контрола (упрощенная версия)
        /// </summary>
        Task<IEnumerable<SampleControlDto>> GetAllControlAsync();

        /// <summary>
        /// Создать Sample (упрощенная версия для контрола)
        /// </summary>
        Task<SampleControlDto> CreateControlAsync(SampleControlCreateDto dto);
    }
}
