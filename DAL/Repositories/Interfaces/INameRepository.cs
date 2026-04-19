using DAL.Enums;
using DAL.Models.NameModels;


namespace DAL.Repositories.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с именами
    /// Предоставляет методы для поиска, фильтрации и управления данными об именах
    /// </summary>
    public interface INameRepository : IRepository<NameMain>
    {
        // ==========================================
        // Базовые методы получения имени
        // ==========================================

        /// <summary>
        /// Получить имя по ключу (английскому названию)
        /// </summary>
        /// <param name="nameKey">Ключ имени (например, "Alexander")</param>
        /// <returns>Объект NameMain или null</returns>
        Task<NameMain?> GetByNameKeyAsync(string nameKey);

        /// <summary>
        /// Получить имя со всеми переводами на все языки
        /// </summary>
        /// <param name="id">Идентификатор имени</param>
        /// <returns>NameMain со всеми деталями и анекдотами</returns>
        Task<NameMain?> GetWithTranslationsAsync(int id);

        /// <summary>
        /// Получить имя с переводом на конкретный язык
        /// </summary>
        /// <param name="id">Идентификатор имени</param>
        /// <param name="languageCode">Код языка (например, "ru", "en")</param>
        /// <returns>NameMain с деталями только на указанном языке</returns>
        Task<NameMain?> GetWithTranslationAsync(int id, string languageCode);

        /// <summary>
        /// Получить имя с выборочным включением связанных данных (оптимизация производительности)
        /// </summary>
        /// <param name="id">Идентификатор имени</param>
        /// <param name="includeDetails">Включить детали имени</param>
        /// <param name="includeAnecdotes">Включить анекдоты</param>
        /// <returns>NameMain с выбранными связанными данными</returns>
        Task<NameMain?> GetByIdWithSelectiveIncludesAsync(
            int id,
            bool includeDetails = true,
            bool includeAnecdotes = false);

        // ==========================================
        // Методы поиска и фильтрации
        // ==========================================

        /// <summary>
        /// Поиск имен по полу
        /// </summary>
        /// <param name="gender">Пол (Male, Female, Unisex)</param>
        /// <param name="languageCode">Опциональный код языка для фильтрации</param>
        /// <returns>Коллекция имен указанного пола</returns>
        Task<IEnumerable<NameMain>> GetByGenderAsync(Gender? gender, string? languageCode = null);

        /// <summary>
        /// Поиск имен по полу с пагинацией
        /// </summary>
        /// <param name="gender">Пол (Male, Female, Unisex)</param>
        /// <param name="languageCode">Код языка</param>
        /// <param name="page">Номер страницы (начиная с 1)</param>
        /// <param name="pageSize">Количество элементов на странице</param>
        /// <returns>Кортеж: коллекция имен и общее количество</returns>
        Task<(IEnumerable<NameMain> Names, int TotalCount)> GetByGenderPagedAsync(
            Gender? gender,
            string? languageCode = null,
            int page = 1,
            int pageSize = 20);

        /// <summary>
        /// Поиск имен по происхождению
        /// </summary>
        /// <param name="origin">Происхождение имени (например, "Greek", "Hebrew")</param>
        /// <param name="languageCode">Опциональный код языка</param>
        /// <returns>Коллекция имен указанного происхождения</returns>
        Task<IEnumerable<NameMain>> GetByOriginAsync(string? origin, string? languageCode = null);

        /// <summary>
        /// Поиск имен по происхождению с пагинацией
        /// </summary>
        /// <param name="origin">Происхождение имени</param>
        /// <param name="languageCode">Код языка</param>
        /// <param name="page">Номер страницы</param>
        /// <param name="pageSize">Количество элементов на странице</param>
        /// <returns>Кортеж: коллекция имен и общее количество</returns>
        Task<(IEnumerable<NameMain> Names, int TotalCount)> GetByOriginPagedAsync(
            string origin,
            string? languageCode = null,
            int page = 1,
            int pageSize = 20);

        /// <summary>
        /// Поиск имен по частичному совпадению
        /// </summary>
        /// <param name="searchTerm">Строка поиска</param>
        /// <param name="languageCode">Код языка для поиска в локализованных полях</param>
        /// <param name="take">Максимальное количество результатов</param>
        /// <returns>Коллекция найденных имен</returns>
        Task<IEnumerable<NameMain>> SearchByNameAsync(
            string searchTerm,
            string? languageCode = null,
            int take = 10);

        /// <summary>
        /// Комплексный поиск с множественными фильтрами
        /// </summary>
        /// <param name="searchTerm">Строка поиска (опционально)</param>
        /// <param name="gender">Фильтр по полу (опционально)</param>
        /// <param name="origin">Фильтр по происхождению (опционально)</param>
        /// <param name="languageCode">Код языка</param>
        /// <param name="page">Номер страницы</param>
        /// <param name="pageSize">Количество элементов на странице</param>
        /// <returns>Кортеж: коллекция имен и общее количество</returns>
        Task<(IEnumerable<NameMain> Names, int TotalCount)> SearchWithFiltersAsync(
            string? searchTerm = null,
            Gender? gender = null,
            string? origin = null,
            string? languageCode = null,
            int page = 1,
            int pageSize = 20);

        // ==========================================
        // Методы для получения популярных и случайных имён
        // ==========================================

        /// <summary>
        /// Получить случайное имя
        /// </summary>
        /// <param name="gender">Опциональный фильтр по полу</param>
        /// <param name="languageCode">Код языка</param>
        /// <returns>Случайное имя или null</returns>
        Task<NameMain?> GetRandomNameAsync(Gender? gender = null, string? languageCode = null);

        /// <summary>
        /// Получить несколько случайных имён
        /// </summary>
        /// <param name="count">Количество имён</param>
        /// <param name="gender">Опциональный фильтр по полу</param>
        /// <param name="languageCode">Код языка</param>
        /// <returns>Коллекция случайных имён</returns>
        Task<IEnumerable<NameMain>> GetRandomNamesAsync(
            int count = 5,
            Gender? gender = null,
            string? languageCode = null);

        // ==========================================
        // Методы проверки и статистики
        // ==========================================

        /// <summary>
        /// Проверить существование имени по ключу
        /// </summary>
        /// <param name="nameKey">Ключ имени</param>
        /// <returns>true, если имя существует</returns>
        Task<bool> ExistsByNameKeyAsync(string nameKey);

        /// <summary>
        /// Получить количество имён по полу
        /// </summary>
        /// <param name="gender">Пол</param>
        /// <param name="languageCode">Код языка</param>
        /// <returns>Количество имён</returns>
        Task<int> GetCountByGenderAsync(Gender? gender, string? languageCode = null);

        /// <summary>
        /// Получить количество имён по происхождению
        /// </summary>
        /// <param name="origin">Происхождение</param>
        /// <param name="languageCode">Код языка</param>
        /// <returns>Количество имён</returns>
        Task<int> GetCountByOriginAsync(string origin, string? languageCode = null);

        /// <summary>
        /// Получить список всех уникальных происхождений имён
        /// </summary>
        /// <param name="languageCode">Код языка</param>
        /// <returns>Список происхождений</returns>
        Task<IEnumerable<string>> GetAllOriginsAsync(string? languageCode = null);

        /// <summary>
        /// Получить общее количество имён в базе
        /// </summary>
        /// <returns>Общее количество имён</returns>
        Task<int> GetTotalCountAsync();
    }
}