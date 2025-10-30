using pr_srv_names.Pages.Sample.Dtos;


namespace pr_srv_names.Pages.Sample.Intarfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с sample
    /// </summary>
    public interface ISampleService
    {
        /// <summary>
        /// Получить sample по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор sample</param>
        /// <returns>Детальная информация о sample или null, если не найдена</returns>
        Task<SampleDetailDto?> GetSampleByIdAsync(int id);

        /// <summary>
        /// Получить список sample с пагинацией и фильтрацией
        /// </summary>
        /// <param name="request">Параметры запроса с пагинацией и фильтрацией</param>
        /// <returns>Страница sample</returns>
        Task<SamplePagedResponseDto> GetAllCategoriesAsync(SamplePageRequestDto request);

        /// <summary>
        /// Создать новую sample
        /// </summary>
        /// <param name="request">Данные для создания sample</param>
        /// <returns>Созданная sampleа</returns>
        Task<SampleDetailDto> CreateSampleAsync(SampleCreateRequestDto request);

        /// <summary>
        /// Обновить существующую sample
        /// </summary>
        /// <param name="id">Идентификатор sample</param>
        /// <param name="request">Данные для обновления sample</param>
        /// <returns>Обновленная sampleа</returns>
        Task<SampleDetailDto> UpdateSampleAsync(int id, SampleUpdateRequestDto request);

        /// <summary>
        /// Удалить sample по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор sample</param>
        /// <returns>True, если sampleа была удалена</returns>
        Task<bool> DeleteSampleAsync(int id);

        /// <summary>
        /// Получить sample, у которых есть описание
        /// </summary>
        /// <returns>Список sample с описанием</returns>
        Task<IEnumerable<SampleDetailDto>> GetCategoriesWithDescriptionAsync();

        /// <summary>
        /// Получить sample по имени
        /// </summary>
        /// <param name="name">Имя sample</param>
        /// <returns>Sampleа или null, если не найдена</returns>
        Task<SampleDetailDto?> GetSampleByNameAsync(string name);

        /// <summary>
        /// Поиск sample по части имени
        /// </summary>
        /// <param name="searchTerm">Поисковый термин</param>
        /// <returns>Список подходящих sample</returns>
        Task<IEnumerable<SampleDetailDto>> SearchCategoriesByNameAsync(string searchTerm);

        /// <summary>
        /// Проверить существование sample по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор sample</param>
        /// <returns>True, если sampleа существует</returns>
        Task<bool> SampleExistsAsync(int id);

        /// <summary>
        /// Проверить уникальность имени sample
        /// </summary>
        /// <param name="name">Имя sample</param>
        /// <param name="excludeId">ID sample для исключения из проверки</param>
        /// <returns>True, если имя уникально</returns>
        Task<bool> IsSampleNameUniqueAsync(string name, int? excludeId = null);

        /// <summary>
        /// Получить все sample в алфавитном порядке (для селектора)
        /// </summary>
        /// <returns>Список всех sample, отсортированных по имени</returns>
        Task<IEnumerable<SampleDetailDto>> GetAllCategoriesAsync();
    }
}