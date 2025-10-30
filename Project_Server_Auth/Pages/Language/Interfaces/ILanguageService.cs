using pr_srv_names.Pages.Language.Dtos;


namespace pr_srv_names.Pages.Language.Intarfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с language
    /// </summary>
    public interface ILanguageService
    {
        /// <summary>
        /// Получить language по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор language</param>
        /// <returns>Детальная информация о language или null, если не найдена</returns>
        Task<LanguageDetailDto?> GetLanguageByIdAsync(int id);

        /// <summary>
        /// Получить список language с пагинацией и фильтрацией
        /// </summary>
        /// <param name="request">Параметры запроса с пагинацией и фильтрацией</param>
        /// <returns>Страница language</returns>
        Task<LanguagePagedResponseDto> GetAllCategoriesAsync(LanguagePageRequestDto request);

        /// <summary>
        /// Создать новую language
        /// </summary>
        /// <param name="request">Данные для создания language</param>
        /// <returns>Созданная languageа</returns>
        Task<LanguageDetailDto> CreateLanguageAsync(LanguageCreateRequestDto request);

        /// <summary>
        /// Обновить существующую language
        /// </summary>
        /// <param name="id">Идентификатор language</param>
        /// <param name="request">Данные для обновления language</param>
        /// <returns>Обновленная languageа</returns>
        Task<LanguageDetailDto> UpdateLanguageAsync(int id, LanguageUpdateRequestDto request);

        /// <summary>
        /// Удалить language по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор language</param>
        /// <returns>True, если languageа была удалена</returns>
        Task<bool> DeleteLanguageAsync(int id);

        /// <summary>
        /// Получить language, у которых есть описание
        /// </summary>
        /// <returns>Список language с описанием</returns>
        Task<IEnumerable<LanguageDetailDto>> GetCategoriesWithDescriptionAsync();

        /// <summary>
        /// Получить language по имени
        /// </summary>
        /// <param name="name">Имя language</param>
        /// <returns>Languageа или null, если не найдена</returns>
        Task<LanguageDetailDto?> GetLanguageByNameAsync(string name);

        /// <summary>
        /// Поиск language по части имени
        /// </summary>
        /// <param name="searchTerm">Поисковый термин</param>
        /// <returns>Список подходящих language</returns>
        Task<IEnumerable<LanguageDetailDto>> SearchCategoriesByNameAsync(string searchTerm);

        /// <summary>
        /// Проверить существование language по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор language</param>
        /// <returns>True, если languageа существует</returns>
        Task<bool> LanguageExistsAsync(int id);

        /// <summary>
        /// Проверить уникальность имени language
        /// </summary>
        /// <param name="name">Имя language</param>
        /// <param name="excludeId">ID language для исключения из проверки</param>
        /// <returns>True, если имя уникально</returns>
        Task<bool> IsLanguageNameUniqueAsync(string name, int? excludeId = null);

        /// <summary>
        /// Получить все language в алфавитном порядке (для селектора)
        /// </summary>
        /// <returns>Список всех language, отсортированных по имени</returns>
        Task<IEnumerable<LanguageDetailDto>> GetAllCategoriesAsync();
    }
}