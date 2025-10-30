using pr_srv_names.Pages.NameMain.Dtos;


namespace pr_srv_names.Pages.NameMain.Intarfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с namemain
    /// </summary>
    public interface INameMainService
    {
        /// <summary>
        /// Получить namemain по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор namemain</param>
        /// <returns>Детальная информация о namemain или null, если не найдена</returns>
        Task<NameMainDetailDto?> GetNameMainByIdAsync(int id);

        /// <summary>
        /// Получить список namemain с пагинацией и фильтрацией
        /// </summary>
        /// <param name="request">Параметры запроса с пагинацией и фильтрацией</param>
        /// <returns>Страница namemain</returns>
        Task<NameMainPagedResponseDto> GetAllCategoriesAsync(NameMainPageRequestDto request);

        /// <summary>
        /// Создать новую namemain
        /// </summary>
        /// <param name="request">Данные для создания namemain</param>
        /// <returns>Созданная namemainа</returns>
        Task<NameMainDetailDto> CreateNameMainAsync(NameMainCreateRequestDto request);

        /// <summary>
        /// Обновить существующую namemain
        /// </summary>
        /// <param name="id">Идентификатор namemain</param>
        /// <param name="request">Данные для обновления namemain</param>
        /// <returns>Обновленная namemainа</returns>
        Task<NameMainDetailDto> UpdateNameMainAsync(int id, NameMainUpdateRequestDto request);

        /// <summary>
        /// Удалить namemain по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор namemain</param>
        /// <returns>True, если namemainа была удалена</returns>
        Task<bool> DeleteNameMainAsync(int id);

        /// <summary>
        /// Получить namemain, у которых есть описание
        /// </summary>
        /// <returns>Список namemain с описанием</returns>
        Task<IEnumerable<NameMainDetailDto>> GetCategoriesWithDescriptionAsync();

        /// <summary>
        /// Получить namemain по имени
        /// </summary>
        /// <param name="name">Имя namemain</param>
        /// <returns>NameMainа или null, если не найдена</returns>
        Task<NameMainDetailDto?> GetNameMainByNameAsync(string name);

        /// <summary>
        /// Поиск namemain по части имени
        /// </summary>
        /// <param name="searchTerm">Поисковый термин</param>
        /// <returns>Список подходящих namemain</returns>
        Task<IEnumerable<NameMainDetailDto>> SearchCategoriesByNameAsync(string searchTerm);

        /// <summary>
        /// Проверить существование namemain по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор namemain</param>
        /// <returns>True, если namemainа существует</returns>
        Task<bool> NameMainExistsAsync(int id);

        /// <summary>
        /// Проверить уникальность имени namemain
        /// </summary>
        /// <param name="name">Имя namemain</param>
        /// <param name="excludeId">ID namemain для исключения из проверки</param>
        /// <returns>True, если имя уникально</returns>
        Task<bool> IsNameMainNameUniqueAsync(string name, int? excludeId = null);

        /// <summary>
        /// Получить все namemain в алфавитном порядке (для селектора)
        /// </summary>
        /// <returns>Список всех namemain, отсортированных по имени</returns>
        Task<IEnumerable<NameMainDetailDto>> GetAllCategoriesAsync();
    }
}