using DAL.Enums;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    /// <summary>
    /// Репозиторий для работы с именами
    /// Предоставляет полный набор методов для работы с данными об именах
    /// </summary>
    public class NameRepository : Repository<NameMain>, INameRepository
    {
        private readonly AppDbContext _context;

        public NameRepository(AppDbContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // ==========================================
        // Базовые методы получения имени
        // ==========================================

        /// <summary>
        /// Получить имя по его ключу (английскому названию)
        /// </summary>
        /// <param name="nameKey">Ключ имени на английском (например, "Alexander")</param>
        /// <returns>Объект NameMain или null, если не найдено</returns>
        /// <exception cref="ArgumentException">Выбрасывается, если nameKey пустой</exception>
        public async Task<NameMain?> GetByNameKeyAsync(string nameKey)
        {
            if (string.IsNullOrWhiteSpace(nameKey))
                throw new ArgumentException("Ключ имени не может быть пустым.", nameof(nameKey));

            return await _context.Names
                .FirstOrDefaultAsync(n => n.Name == nameKey);
        }

        /// <summary>
        /// Получить имя со всеми переводами и связанными данными
        /// Включает: детали имени на всех языках, информацию о языках и анекдоты
        /// </summary>
        /// <param name="id">Идентификатор имени</param>
        /// <returns>Объект NameMain со всеми связанными данными или null</returns>
        public async Task<NameMain?> GetWithTranslationsAsync(int id)
        {
            return await _context.Names
                .Include(n => n.NameDetail)
                .ThenInclude(d => d.Language)
                .Include(n => n.Anecdotes)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        /// <summary>
        /// Получить имя с переводом на конкретный язык
        /// Фильтрует детали имени по коду языка
        /// </summary>
        /// <param name="id">Идентификатор имени</param>
        /// <param name="languageCode">Код языка (например, "en", "ru")</param>
        /// <returns>Объект NameMain с деталями только на указанном языке или null</returns>
        /// <exception cref="ArgumentException">Выбрасывается, если languageCode пустой</exception>
        public async Task<NameMain?> GetWithTranslationAsync(int id, string languageCode)
        {
            if (string.IsNullOrWhiteSpace(languageCode))
                throw new ArgumentException("Код языка не может быть пустым.", nameof(languageCode));

            return await _context.Names
                .Include(n => n.NameDetail.Where(d => d.Language.Code == languageCode))
                .ThenInclude(d => d.Language)
                .Include(n => n.Anecdotes)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        /// <summary>
        /// Получить имя с выборочным включением связанных данных (оптимизация производительности)
        /// </summary>
        /// <param name="id">Идентификатор имени</param>
        /// <param name="includeDetails">Включить детали имени</param>
        /// <param name="includeAnecdotes">Включить анекдоты</param>
        /// <returns>NameMain с выбранными связанными данными</returns>
        public async Task<NameMain?> GetByIdWithSelectiveIncludesAsync(
            int id,
            bool includeDetails = true,
            bool includeAnecdotes = false)
        {
            var query = _context.Names.AsQueryable();

            if (includeDetails)
            {
                query = query.Include(n => n.NameDetail)
                    .ThenInclude(d => d.Language);
            }

            if (includeAnecdotes)
            {
                query = query.Include(n => n.Anecdotes);
            }

            return await query.FirstOrDefaultAsync(n => n.Id == id);
        }

        // ==========================================
        // Методы поиска и фильтрации
        // ==========================================

        /// <summary>
        /// Поиск имен по полу
        /// Использует информацию из NameDetail.Gender
        /// </summary>
        /// <param name="gender">Пол (Male, Female, Unisex или null)</param>
        /// <param name="languageCode">Опциональный код языка для фильтрации</param>
        /// <returns>Коллекция имен, соответствующих указанному полу</returns>
        public async Task<IEnumerable<NameMain>> GetByGenderAsync(Gender? gender, string? languageCode = null)
        {
            // Базовый запрос с включением деталей и языков
            var query = _context.Names
                .Include(n => n.NameDetail)
                .ThenInclude(d => d.Language)
                .AsQueryable();

            // Если указан код языка, фильтруем по языку и полу
            if (!string.IsNullOrWhiteSpace(languageCode))
            {
                query = query.Where(n => n.NameDetail.Any(d =>
                    d.Language.Code == languageCode &&
                    d.Gender == gender));
            }
            else
            {
                // Фильтруем только по полу
                query = query.Where(n => n.NameDetail.Any(d => d.Gender == gender));
            }

            // Убираем дубликаты и возвращаем результат
            return await query
                .Distinct()
                .ToListAsync();
        }

        /// <summary>
        /// Поиск имен по полу с пагинацией
        /// </summary>
        /// <param name="gender">Пол (Male, Female, Unisex)</param>
        /// <param name="languageCode">Код языка</param>
        /// <param name="page">Номер страницы (начиная с 1)</param>
        /// <param name="pageSize">Количество элементов на странице</param>
        /// <returns>Кортеж: коллекция имен и общее количество</returns>
        public async Task<(IEnumerable<NameMain> Names, int TotalCount)> GetByGenderPagedAsync(
            Gender? gender,
            string? languageCode = null,
            int page = 1,
            int pageSize = 20)
        {
            // Валидация параметров пагинации
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 100) pageSize = 100; // Ограничение максимального размера страницы

            var query = _context.Names
                .Include(n => n.NameDetail)
                .ThenInclude(d => d.Language)
                .AsQueryable();

            // Применяем фильтры
            if (!string.IsNullOrWhiteSpace(languageCode))
            {
                query = query.Where(n => n.NameDetail.Any(d =>
                    d.Language.Code == languageCode &&
                    d.Gender == gender));
            }
            else
            {
                query = query.Where(n => n.NameDetail.Any(d => d.Gender == gender));
            }

            // Убираем дубликаты
            query = query.Distinct();

            // Получаем общее количество
            var totalCount = await query.CountAsync();

            // Применяем пагинацию
            var names = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (names, totalCount);
        }

        /// <summary>
        /// Поиск имен по происхождению
        /// Использует информацию из NameDetail.Origin
        /// </summary>
        /// <param name="origin">Происхождение имени (например, "Greek", "Hebrew", "Slavic")</param>
        /// <param name="languageCode">Опциональный код языка для фильтрации</param>
        /// <returns>Коллекция имен указанного происхождения</returns>
        /// <exception cref="ArgumentException">Выбрасывается, если origin пустой</exception>
        public async Task<IEnumerable<NameMain>> GetByOriginAsync(string? origin, string? languageCode = null)
        {
            if (string.IsNullOrWhiteSpace(origin))
                throw new ArgumentException("Происхождение не может быть пустым.", nameof(origin));

            // Базовый запрос с включением деталей и языков
            var query = _context.Names
                .Include(n => n.NameDetail)
                .ThenInclude(d => d.Language)
                .AsQueryable();

            // Если указан код языка, фильтруем по языку и происхождению
            if (!string.IsNullOrWhiteSpace(languageCode))
            {
                query = query.Where(n => n.NameDetail.Any(d =>
                    d.Language.Code == languageCode &&
                    d.Origin == origin));
            }
            else
            {
                // Фильтруем только по происхождению
                query = query.Where(n => n.NameDetail.Any(d => d.Origin == origin));
            }

            // Убираем дубликаты и возвращаем результат
            return await query
                .Distinct()
                .ToListAsync();
        }

        /// <summary>
        /// Поиск имен по происхождению с пагинацией
        /// </summary>
        /// <param name="origin">Происхождение имени</param>
        /// <param name="languageCode">Код языка</param>
        /// <param name="page">Номер страницы</param>
        /// <param name="pageSize">Количество элементов на странице</param>
        /// <returns>Кортеж: коллекция имен и общее количество</returns>
        public async Task<(IEnumerable<NameMain> Names, int TotalCount)> GetByOriginPagedAsync(
            string origin,
            string? languageCode = null,
            int page = 1,
            int pageSize = 20)
        {
            if (string.IsNullOrWhiteSpace(origin))
                throw new ArgumentException("Происхождение не может быть пустым.", nameof(origin));

            // Валидация параметров пагинации
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 100) pageSize = 100;

            var query = _context.Names
                .Include(n => n.NameDetail)
                .ThenInclude(d => d.Language)
                .AsQueryable();

            // Применяем фильтры
            if (!string.IsNullOrWhiteSpace(languageCode))
            {
                query = query.Where(n => n.NameDetail.Any(d =>
                    d.Language.Code == languageCode &&
                    d.Origin == origin));
            }
            else
            {
                query = query.Where(n => n.NameDetail.Any(d => d.Origin == origin));
            }

            query = query.Distinct();

            var totalCount = await query.CountAsync();

            var names = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (names, totalCount);
        }

        /// <summary>
        /// Поиск имен по частичному совпадению
        /// </summary>
        /// <param name="searchTerm">Строка поиска</param>
        /// <param name="languageCode">Код языка для поиска в локализованных полях</param>
        /// <param name="take">Максимальное количество результатов</param>
        /// <returns>Коллекция найденных имен</returns>
        public async Task<IEnumerable<NameMain>> SearchByNameAsync(
            string searchTerm,
            string? languageCode = null,
            int take = 10)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentException("Строка поиска не может быть пустой.", nameof(searchTerm));

            if (take < 1) take = 10;
            if (take > 50) take = 50; // Ограничение для производительности

            var query = _context.Names
                .Include(n => n.NameDetail)
                .ThenInclude(d => d.Language)
                .AsQueryable();

            // Поиск по основному имени (английскому)
            var searchLower = searchTerm.ToLower();
            query = query.Where(n => n.Name.ToLower().Contains(searchLower));

            // Если указан язык, также ищем в локализованных полях
            if (!string.IsNullOrWhiteSpace(languageCode))
            {
                query = query.Where(n =>
                    n.Name.ToLower().Contains(searchLower) ||
                    n.NameDetail.Any(d =>
                        d.Language.Code == languageCode &&
                        (d.Name != null && d.Name.ToLower().Contains(searchLower) ||
                         d.FullName != null && d.FullName.ToLower().Contains(searchLower))));
            }

            return await query
                .Take(take)
                .ToListAsync();
        }

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
        public async Task<(IEnumerable<NameMain> Names, int TotalCount)> SearchWithFiltersAsync(
            string? searchTerm = null,
            Gender? gender = null,
            string? origin = null,
            string? languageCode = null,
            int page = 1,
            int pageSize = 20)
        {
            // Валидация параметров пагинации
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 100) pageSize = 100;

            var query = _context.Names
                .Include(n => n.NameDetail)
                .ThenInclude(d => d.Language)
                .AsQueryable();

            // Фильтр по строке поиска
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchLower = searchTerm.ToLower();
                query = query.Where(n => n.Name.ToLower().Contains(searchLower));
            }

            // Фильтр по полу
            if (gender.HasValue)
            {
                if (!string.IsNullOrWhiteSpace(languageCode))
                {
                    query = query.Where(n => n.NameDetail.Any(d =>
                        d.Language.Code == languageCode &&
                        d.Gender == gender));
                }
                else
                {
                    query = query.Where(n => n.NameDetail.Any(d => d.Gender == gender));
                }
            }

            // Фильтр по происхождению
            if (!string.IsNullOrWhiteSpace(origin))
            {
                if (!string.IsNullOrWhiteSpace(languageCode))
                {
                    query = query.Where(n => n.NameDetail.Any(d =>
                        d.Language.Code == languageCode &&
                        d.Origin == origin));
                }
                else
                {
                    query = query.Where(n => n.NameDetail.Any(d => d.Origin == origin));
                }
            }

            query = query.Distinct();

            var totalCount = await query.CountAsync();

            var names = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (names, totalCount);
        }

        // ==========================================
        // Методы для получения популярных и случайных имён
        // ==========================================

        /// <summary>
        /// Получить случайное имя
        /// </summary>
        /// <param name="gender">Опциональный фильтр по полу</param>
        /// <param name="languageCode">Код языка</param>
        /// <returns>Случайное имя или null</returns>
        public async Task<NameMain?> GetRandomNameAsync(Gender? gender = null, string? languageCode = null)
        {
            var query = _context.Names
                .Include(n => n.NameDetail)
                .ThenInclude(d => d.Language)
                .AsQueryable();

            // Применяем фильтры если указаны
            if (gender.HasValue)
            {
                if (!string.IsNullOrWhiteSpace(languageCode))
                {
                    query = query.Where(n => n.NameDetail.Any(d =>
                        d.Language.Code == languageCode &&
                        d.Gender == gender));
                }
                else
                {
                    query = query.Where(n => n.NameDetail.Any(d => d.Gender == gender));
                }
            }

            // Получаем общее количество
            var count = await query.CountAsync();
            if (count == 0) return null;

            // Генерируем случайный индекс
            var random = new Random();
            var randomIndex = random.Next(0, count);

            // Возвращаем случайное имя
            return await query
                .Skip(randomIndex)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Получить несколько случайных имён
        /// </summary>
        /// <param name="count">Количество имён</param>
        /// <param name="gender">Опциональный фильтр по полу</param>
        /// <param name="languageCode">Код языка</param>
        /// <returns>Коллекция случайных имён</returns>
        public async Task<IEnumerable<NameMain>> GetRandomNamesAsync(
            int count = 5,
            Gender? gender = null,
            string? languageCode = null)
        {
            if (count < 1) count = 5;
            if (count > 50) count = 50; // Ограничение

            var query = _context.Names
                .Include(n => n.NameDetail)
                .ThenInclude(d => d.Language)
                .AsQueryable();

            // Применяем фильтры
            if (gender.HasValue)
            {
                if (!string.IsNullOrWhiteSpace(languageCode))
                {
                    query = query.Where(n => n.NameDetail.Any(d =>
                        d.Language.Code == languageCode &&
                        d.Gender == gender));
                }
                else
                {
                    query = query.Where(n => n.NameDetail.Any(d => d.Gender == gender));
                }
            }

            // Получаем все ID
            var allIds = await query.Select(n => n.Id).ToListAsync();
            if (allIds.Count == 0) return new List<NameMain>();

            // Выбираем случайные ID
            var random = new Random();
            var randomIds = allIds
                .OrderBy(x => random.Next())
                .Take(count)
                .ToList();

            // Получаем имена по выбранным ID
            return await query
                .Where(n => randomIds.Contains(n.Id))
                .ToListAsync();
        }

        // ==========================================
        // Методы проверки и статистики
        // ==========================================

        /// <summary>
        /// Проверить существование имени по его ключу
        /// Полезно для валидации перед добавлением новых имен
        /// </summary>
        /// <param name="nameKey">Ключ имени на английском</param>
        /// <returns>true, если имя существует; false в противном случае</returns>
        /// <exception cref="ArgumentException">Выбрасывается, если nameKey пустой</exception>
        public async Task<bool> ExistsByNameKeyAsync(string nameKey)
        {
            if (string.IsNullOrWhiteSpace(nameKey))
                throw new ArgumentException("Ключ имени не может быть пустым.", nameof(nameKey));

            return await _context.Names
                .AnyAsync(n => n.Name == nameKey);
        }

        /// <summary>
        /// Получить количество имён по полу
        /// </summary>
        /// <param name="gender">Пол</param>
        /// <param name="languageCode">Код языка</param>
        /// <returns>Количество имён</returns>
        public async Task<int> GetCountByGenderAsync(Gender? gender, string? languageCode = null)
        {
            var query = _context.Names.AsQueryable();

            if (!string.IsNullOrWhiteSpace(languageCode))
            {
                query = query.Where(n => n.NameDetail.Any(d =>
                    d.Language.Code == languageCode &&
                    d.Gender == gender));
            }
            else
            {
                query = query.Where(n => n.NameDetail.Any(d => d.Gender == gender));
            }

            return await query.Distinct().CountAsync();
        }

        /// <summary>
        /// Получить количество имён по происхождению
        /// </summary>
        /// <param name="origin">Происхождение</param>
        /// <param name="languageCode">Код языка</param>
        /// <returns>Количество имён</returns>
        public async Task<int> GetCountByOriginAsync(string origin, string? languageCode = null)
        {
            if (string.IsNullOrWhiteSpace(origin))
                throw new ArgumentException("Происхождение не может быть пустым.", nameof(origin));

            var query = _context.Names.AsQueryable();

            if (!string.IsNullOrWhiteSpace(languageCode))
            {
                query = query.Where(n => n.NameDetail.Any(d =>
                    d.Language.Code == languageCode &&
                    d.Origin == origin));
            }
            else
            {
                query = query.Where(n => n.NameDetail.Any(d => d.Origin == origin));
            }

            return await query.Distinct().CountAsync();
        }

        /// <summary>
        /// Получить список всех уникальных происхождений имён
        /// </summary>
        /// <param name="languageCode">Код языка</param>
        /// <returns>Список происхождений</returns>
        public async Task<IEnumerable<string>> GetAllOriginsAsync(string? languageCode = null)
        {
            IQueryable<string?> query;

            if (!string.IsNullOrWhiteSpace(languageCode))
            {
                query = _context.Set<NameDetail>()
                    .Where(d => d.Language.Code == languageCode && d.Origin != null)
                    .Select(d => d.Origin);
            }
            else
            {
                query = _context.Set<NameDetail>()
                    .Where(d => d.Origin != null)
                    .Select(d => d.Origin);
            }

            var origins = await query
                .Distinct()
                .ToListAsync();

            // Фильтруем null значения и возвращаем
            return origins.Where(o => !string.IsNullOrWhiteSpace(o))!;
        }

        /// <summary>
        /// Получить общее количество имён в базе
        /// </summary>
        /// <returns>Общее количество имён</returns>
        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Names.CountAsync();
        }
    }
}