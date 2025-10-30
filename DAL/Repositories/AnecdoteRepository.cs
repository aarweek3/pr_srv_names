using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repositories
{
    /// <summary>
    /// Репозиторий для работы с анекдотами
    /// </summary>
    public class AnecdoteRepository : Repository<Anecdote>, IAnecdoteRepository
    {
        private readonly AppDbContext _context;

        public AnecdoteRepository(AppDbContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Получить анекдот по ID с включением связанных данных
        /// </summary>
        public async Task<Anecdote?> GetByIdWithIncludesAsync(int id)
        {
            return await _context.Anecdotes
                .AsNoTracking()
                .Include(a => a.NameMain)
                .Include(a => a.Language)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        /// <summary>
        /// Получить все анекдоты с включением связанных данных
        /// </summary>
        public async Task<IEnumerable<Anecdote>> GetAllWithIncludesAsync()
        {
            return await _context.Anecdotes
                .AsNoTracking()
                .Include(a => a.NameMain)
                .Include(a => a.Language)
                .ToListAsync();
        }

        /// <summary>
        /// Получить анекдоты с пагинацией и включением связанных данных
        /// </summary>
        public async Task<(IEnumerable<Anecdote> Items, int Total)> GetPagedWithIncludesAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<Anecdote, object>>? orderBy = null,
            bool ascending = true,
            Expression<Func<Anecdote, bool>>? filter = null)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            IQueryable<Anecdote> query = _context.Anecdotes
                .AsNoTracking()
                .Include(a => a.NameMain)
                .Include(a => a.Language);

            // Применяем фильтр
            if (filter != null)
                query = query.Where(filter);

            // Подсчитываем общее количество
            var total = await query.CountAsync();

            // Применяем сортировку
            if (orderBy != null)
                query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
            else
                query = query.OrderBy(a => a.Id);

            // Применяем пагинацию
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        /// <summary>
        /// Получить все анекдоты для конкретного имени
        /// </summary>
        public async Task<IEnumerable<Anecdote>> GetByNameMainIdAsync(int nameMainId)
        {
            return await _context.Anecdotes
                .AsNoTracking()
                .Include(a => a.NameMain)
                .Include(a => a.Language)
                .Where(a => a.NameMainId == nameMainId)
                .ToListAsync();
        }

        /// <summary>
        /// Получить все анекдоты на конкретном языке
        /// </summary>
        public async Task<IEnumerable<Anecdote>> GetByLanguageIdAsync(int languageId)
        {
            return await _context.Anecdotes
                .AsNoTracking()
                .Include(a => a.NameMain)
                .Include(a => a.Language)
                .Where(a => a.LanguageId == languageId)
                .ToListAsync();
        }

        /// <summary>
        /// Получить анекдоты по имени и языку
        /// </summary>
        public async Task<IEnumerable<Anecdote>> GetByNameMainIdAndLanguageIdAsync(int nameMainId, int languageId)
        {
            return await _context.Anecdotes
                .AsNoTracking()
                .Include(a => a.NameMain)
                .Include(a => a.Language)
                .Where(a => a.NameMainId == nameMainId && a.LanguageId == languageId)
                .ToListAsync();
        }

        /// <summary>
        /// Поиск анекдотов по названию или описанию
        /// </summary>
        public async Task<IEnumerable<Anecdote>> SearchByTextAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return Enumerable.Empty<Anecdote>();

            var normalizedTerm = searchTerm.Trim().ToLower();

            return await _context.Anecdotes
                .AsNoTracking()
                .Include(a => a.NameMain)
                .Include(a => a.Language)
                .Where(a => (a.Name != null && a.Name.ToLower().Contains(normalizedTerm)) ||
                           (a.Description != null && a.Description.ToLower().Contains(normalizedTerm)))
                .ToListAsync();
        }

        /// <summary>
        /// Получить количество анекдотов для конкретного имени
        /// </summary>
        public async Task<int> GetCountByNameMainIdAsync(int nameMainId)
        {
            return await _context.Anecdotes
                .AsNoTracking()
                .CountAsync(a => a.NameMainId == nameMainId);
        }

        /// <summary>
        /// Получить количество анекдотов на конкретном языке
        /// </summary>
        public async Task<int> GetCountByLanguageIdAsync(int languageId)
        {
            return await _context.Anecdotes
                .AsNoTracking()
                .CountAsync(a => a.LanguageId == languageId);
        }

        /// <summary>
        /// Проверить существование анекдота с заданными NameMainId и LanguageId
        /// </summary>
        public async Task<bool> ExistsByNameAndLanguageAsync(int nameMainId, int languageId, int? excludeId = null)
        {
            var query = _context.Anecdotes
                .AsNoTracking()
                .Where(a => a.NameMainId == nameMainId && a.LanguageId == languageId);

            if (excludeId.HasValue)
                query = query.Where(a => a.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        /// <summary>
        /// Получить последние N анекдотов
        /// </summary>
        public async Task<IEnumerable<Anecdote>> GetLatestAsync(int count)
        {
            return await _context.Anecdotes
                .AsNoTracking()
                .Include(a => a.NameMain)
                .Include(a => a.Language)
                .OrderByDescending(a => a.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        /// <summary>
        /// Получить случайные анекдоты
        /// </summary>
        public async Task<IEnumerable<Anecdote>> GetRandomAnecdotesAsync(int count, int? nameMainId = null, int? languageId = null)
        {
            var query = _context.Anecdotes
                .AsNoTracking()
                .Include(a => a.NameMain)
                .Include(a => a.Language)
                .AsQueryable();

            if (nameMainId.HasValue)
                query = query.Where(a => a.NameMainId == nameMainId.Value);

            if (languageId.HasValue)
                query = query.Where(a => a.LanguageId == languageId.Value);

            return await query
                .OrderBy(a => Guid.NewGuid())
                .Take(count)
                .ToListAsync();
        }

        /// <summary>
        /// Массовое удаление анекдотов по NameMainId
        /// </summary>
        public async Task<int> DeleteByNameMainIdAsync(int nameMainId)
        {
            var anecdotes = await _context.Anecdotes
                .Where(a => a.NameMainId == nameMainId)
                .ToListAsync();

            if (anecdotes.Any())
            {
                _context.Anecdotes.RemoveRange(anecdotes);
            }

            return anecdotes.Count;
        }

        /// <summary>
        /// Массовое удаление анекдотов по LanguageId
        /// </summary>
        public async Task<int> DeleteByLanguageIdAsync(int languageId)
        {
            var anecdotes = await _context.Anecdotes
                .Where(a => a.LanguageId == languageId)
                .ToListAsync();

            if (anecdotes.Any())
            {
                _context.Anecdotes.RemoveRange(anecdotes);
            }

            return anecdotes.Count;
        }
    }
}
