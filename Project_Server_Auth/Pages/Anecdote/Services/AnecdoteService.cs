using pr_srv_names.Exceptions;
using pr_srv_names.Models;
using AutoMapper;
using DAL.Interfaces;
using DAL.Repositories.Interfaces;
using FluentValidation;
using System.Linq.Expressions;
using pr_srv_names.Pages.Anecdote.Dtos;
using pr_srv_names.Pages.Anecdote.Interfaces;
using pr_srv_names.Pages.Anecdote.Models;
using Microsoft.EntityFrameworkCore;

namespace pr_srv_names.Pages.Anecdote.Services
{
    /// <summary>
    /// Сервис для работы с анекдотами
    /// </summary>
    public class AnecdoteService : IAnecdoteService
    {
        private readonly IAnecdoteRepository _anecdoteRepository;
        private readonly INameMainRepository _nameMainRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AnecdoteService> _logger;
        private readonly IValidator<AnecdoteCreateRequestDto> _createValidator;
        private readonly IValidator<AnecdoteUpdateRequestDto> _updateValidator;
        private readonly IValidator<AnecdotePageRequestDto> _pageRequestValidator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AnecdoteService(
            IAnecdoteRepository anecdoteRepository,
            INameMainRepository nameMainRepository,
            ILanguageRepository languageRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<AnecdoteService> logger,
            IValidator<AnecdoteCreateRequestDto> createValidator,
            IValidator<AnecdoteUpdateRequestDto> updateValidator,
            IValidator<AnecdotePageRequestDto> pageRequestValidator,
            IHttpContextAccessor httpContextAccessor)
        {
            _anecdoteRepository = anecdoteRepository ?? throw new ArgumentNullException(nameof(anecdoteRepository));
            _nameMainRepository = nameMainRepository ?? throw new ArgumentNullException(nameof(nameMainRepository));
            _languageRepository = languageRepository ?? throw new ArgumentNullException(nameof(languageRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
            _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
            _pageRequestValidator =
                pageRequestValidator ?? throw new ArgumentNullException(nameof(pageRequestValidator));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        private string GetCorrelationId() =>
            _httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString();

        private void ValidateId(int id, string operationName)
        {
            if (id <= 0)
            {
                var correlationId = GetCorrelationId();
                _logger.LogWarning("Получен невалидный ID {Id} для {Operation}. CorrelationId: {CorrelationId}",
                    id, operationName, correlationId);
                throw new InvalidParametersException($"Некорректный идентификатор анекдота: {id}.");
            }
        }

        /// <summary>
        /// Получить анекдот по ID или выбросить исключение, если не найден
        /// </summary>
        private async Task<DAL.Models.Anecdote> GetAnecdoteOrThrow(int id)
        {
            var correlationId = GetCorrelationId();
            var entity = await _anecdoteRepository.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogWarning("Анекдот с ID {Id} не найден. CorrelationId: {CorrelationId}", id, correlationId);
                throw new NotFoundException("Anecdote", $"Анекдот с ID {id} не найден.");
            }

            return entity;
        }

        /// <summary>
        /// Валидация существования NameMain
        /// </summary>
        private async Task ValidateNameMainExists(int nameMainId)
        {
            var correlationId = GetCorrelationId();
            var exists = await _nameMainRepository.ExistsAsync(nameMainId);
            if (!exists)
            {
                _logger.LogWarning("NameMain с ID {NameMainId} не найдено. CorrelationId: {CorrelationId}",
                    nameMainId, correlationId);
                throw new NotFoundException("NameMain", $"Имя с ID {nameMainId} не найдено.");
            }
        }

        /// <summary>
        /// Валидация существования Language
        /// </summary>
        private async Task ValidateLanguageExists(int languageId)
        {
            var correlationId = GetCorrelationId();
            var exists = await _languageRepository.ExistsAsync(languageId);
            if (!exists)
            {
                _logger.LogWarning("Language с ID {LanguageId} не найден. CorrelationId: {CorrelationId}",
                    languageId, correlationId);
                throw new NotFoundException("Language", $"Язык с ID {languageId} не найден.");
            }
        }

        public async Task<AnecdoteDetailDto?> GetAnecdoteByIdAsync(int id)
        {
            var correlationId = GetCorrelationId();
            ValidateId(id, nameof(GetAnecdoteByIdAsync));

            try
            {
                var entity = await _anecdoteRepository.GetByIdWithIncludesAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("Анекдот с ID {Id} не найден. CorrelationId: {CorrelationId}", id,
                        correlationId);
                    throw new NotFoundException("Anecdote", $"Анекдот с ID {id} не найден.");
                }

                var result = _mapper.Map<AnecdoteDetailDto>(entity);
                _logger.LogInformation("Анекдот с ID {Id} успешно получен. CorrelationId: {CorrelationId}", id,
                    correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is NotFoundException || ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при получении анекдота с ID {Id}. CorrelationId: {CorrelationId}", id,
                    correlationId);
                throw;
            }
        }

        public async Task<IEnumerable<AnecdoteDetailDto>> GetAllAnecdotesAsync()
        {
            var correlationId = GetCorrelationId();
            try
            {
                var items = await _anecdoteRepository.GetAllWithIncludesAsync();
                var sortedItems = items.OrderBy(c => c.Name);
                var result = _mapper.Map<IEnumerable<AnecdoteDetailDto>>(sortedItems);

                _logger.LogInformation("Получено {Count} анекдотов. CorrelationId: {CorrelationId}",
                    result.Count(), correlationId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении всех анекдотов. CorrelationId: {CorrelationId}",
                    correlationId);
                throw;
            }
        }

        public async Task<AnecdotePagedResponseDto> GetAllAnecdotesAsync(AnecdotePageRequestDto request)
        {
            var correlationId = GetCorrelationId();
            if (request == null)
            {
                _logger.LogWarning("Параметры запроса GetAllAnecdotesAsync отсутствуют. CorrelationId: {CorrelationId}",
                    correlationId);
                throw new InvalidParametersException("Параметры запроса отсутствуют.");
            }

            var validationResult = await _pageRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning(
                    "Валидация AnecdotePageRequestDto не пройдена. Ошибки: {Errors}. CorrelationId: {CorrelationId}",
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), correlationId);
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                var filter = BuildSearchFilter(request.SearchTerm, request.NameMainId, request.LanguageId);
                var orderBy = BuildSortExpression(request.SortBy);
                bool isAscending = request.SortDirection == SortDirection.Asc;

                var (items, total) = await _anecdoteRepository.GetPagedWithIncludesAsync(
                    request.PageNumber,
                    request.PageSize,
                    orderBy,
                    isAscending,
                    filter);

                var anecdoteDtos = _mapper.Map<IEnumerable<AnecdoteDetailDto>>(items);

                _logger.LogInformation(
                    "Список анекдотов успешно получен. Страница: {PageNumber}, Размер: {PageSize}, Всего: {Total}. CorrelationId: {CorrelationId}",
                    request.PageNumber, request.PageSize, total, correlationId);

                return new AnecdotePagedResponseDto
                {
                    Items = anecdoteDtos,
                    Total = total,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }
            catch (Exception ex) when (!(ex is ValidationException || ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при получении списка анекдотов. CorrelationId: {CorrelationId}",
                    correlationId);
                throw;
            }
        }

        public async Task<AnecdoteDetailDto> CreateAnecdoteAsync(AnecdoteCreateRequestDto request)
        {
            var correlationId = GetCorrelationId();
            if (request == null)
            {
                _logger.LogWarning("Параметры запроса CreateAnecdoteAsync отсутствуют. CorrelationId: {CorrelationId}",
                    correlationId);
                throw new InvalidParametersException("Параметры запроса отсутствуют.");
            }

            // Трансформируем данные перед валидацией
            TransformAnecdoteData(request);

            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning(
                    "Валидация AnecdoteCreateRequestDto не пройдена. Ошибки: {Errors}. CorrelationId: {CorrelationId}",
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), correlationId);
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                await ValidateNameMainExists(request.NameMainId);
                await ValidateLanguageExists(request.LanguageId);

                var entity = _mapper.Map<DAL.Models.Anecdote>(request);
                await _anecdoteRepository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<AnecdoteDetailDto>(entity);
                _logger.LogInformation("Анекдот с ID {Id} успешно создан. Name: {Name}. CorrelationId: {CorrelationId}",
                    entity.Id, entity.Name, correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is NotFoundException || ex is ValidationException ||
                                         ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при создании анекдота. CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
        }

        public async Task<AnecdoteDetailDto> UpdateAnecdoteAsync(int id, AnecdoteUpdateRequestDto request)
        {
            var correlationId = GetCorrelationId();
            ValidateId(id, nameof(UpdateAnecdoteAsync));

            if (request == null)
            {
                _logger.LogWarning("Параметры запроса UpdateAnecdoteAsync отсутствуют. CorrelationId: {CorrelationId}",
                    correlationId);
                throw new InvalidParametersException("Параметры запроса отсутствуют.");
            }

            request.Id = id;

            // Трансформируем данные перед валидацией
            TransformAnecdoteData(request);

            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning(
                    "Валидация AnecdoteUpdateRequestDto не пройдена. Ошибки: {Errors}. CorrelationId: {CorrelationId}",
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), correlationId);
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                var entity = await GetAnecdoteOrThrow(id);

                await ValidateNameMainExists(request.NameMainId);
                await ValidateLanguageExists(request.LanguageId);

                var originalName = entity.Name;
                _mapper.Map(request, entity);
                _anecdoteRepository.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<AnecdoteDetailDto>(entity);
                _logger.LogInformation(
                    "Анекдот обновлен - ID: {Id}, Name: {OldName} -> {NewName}. CorrelationId: {CorrelationId}",
                    entity.Id, originalName, entity.Name, correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is NotFoundException || ex is ValidationException ||
                                         ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при обновлении анекдота с ID {Id}. CorrelationId: {CorrelationId}", id,
                    correlationId);
                throw;
            }
        }

        public async Task<bool> DeleteAnecdoteAsync(int id)
        {
            var correlationId = GetCorrelationId();
            ValidateId(id, nameof(DeleteAnecdoteAsync));

            try
            {
                var entity = await GetAnecdoteOrThrow(id);
                await _anecdoteRepository.DeleteByIdAsync(id);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Анекдот с ID {Id} успешно удален. Name: {Name}. CorrelationId: {CorrelationId}",
                    entity.Id, entity.Name, correlationId);
                return true;
            }
            catch (Exception ex) when (!(ex is NotFoundException || ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при удалении анекдота с ID {Id}. CorrelationId: {CorrelationId}", id,
                    correlationId);
                throw;
            }
        }

        public async Task<IEnumerable<AnecdoteDetailDto>> GetAnecdotesByNameMainIdAsync(int nameMainId)
        {
            var correlationId = GetCorrelationId();
            ValidateId(nameMainId, nameof(GetAnecdotesByNameMainIdAsync));

            try
            {
                var entities = await _anecdoteRepository.GetByNameMainIdAsync(nameMainId);
                var result = _mapper.Map<IEnumerable<AnecdoteDetailDto>>(entities);

                _logger.LogInformation(
                    "Получено {Count} анекдотов для имени {NameMainId}. CorrelationId: {CorrelationId}",
                    result.Count(), nameMainId, correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is InvalidParametersException))
            {
                _logger.LogError(ex,
                    "Ошибка при получении анекдотов для имени {NameMainId}. CorrelationId: {CorrelationId}",
                    nameMainId, correlationId);
                throw;
            }
        }

        public async Task<IEnumerable<AnecdoteDetailDto>> GetAnecdotesByLanguageIdAsync(int languageId)
        {
            var correlationId = GetCorrelationId();
            ValidateId(languageId, nameof(GetAnecdotesByLanguageIdAsync));

            try
            {
                var entities = await _anecdoteRepository.GetByLanguageIdAsync(languageId);
                var result = _mapper.Map<IEnumerable<AnecdoteDetailDto>>(entities);

                _logger.LogInformation(
                    "Получено {Count} анекдотов для языка {LanguageId}. CorrelationId: {CorrelationId}",
                    result.Count(), languageId, correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is InvalidParametersException))
            {
                _logger.LogError(ex,
                    "Ошибка при получении анекдотов для языка {LanguageId}. CorrelationId: {CorrelationId}",
                    languageId, correlationId);
                throw;
            }
        }

        public async Task<IEnumerable<AnecdoteDetailDto>> GetAnecdotesByNameAndLanguageAsync(int nameMainId,
            int languageId)
        {
            var correlationId = GetCorrelationId();
            ValidateId(nameMainId, nameof(GetAnecdotesByNameAndLanguageAsync));
            ValidateId(languageId, nameof(GetAnecdotesByNameAndLanguageAsync));

            try
            {
                var entities = await _anecdoteRepository.GetByNameMainIdAndLanguageIdAsync(nameMainId, languageId);
                var result = _mapper.Map<IEnumerable<AnecdoteDetailDto>>(entities);

                _logger.LogInformation(
                    "Получено {Count} анекдотов для имени {NameMainId} и языка {LanguageId}. CorrelationId: {CorrelationId}",
                    result.Count(), nameMainId, languageId, correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is InvalidParametersException))
            {
                _logger.LogError(ex,
                    "Ошибка при получении анекдотов для имени {NameMainId} и языка {LanguageId}. CorrelationId: {CorrelationId}",
                    nameMainId, languageId, correlationId);
                throw;
            }
        }

        public async Task<IEnumerable<AnecdoteDetailDto>> SearchAnecdotesByNameAsync(string searchTerm)
        {
            var correlationId = GetCorrelationId();
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                _logger.LogWarning("Передан пустой поисковый термин. CorrelationId: {CorrelationId}", correlationId);
                throw new InvalidParametersException("Поисковый термин не может быть пустым.");
            }

            try
            {
                var filter = BuildSearchFilter(searchTerm, null, null);
                var entities = await _anecdoteRepository.FindAsync(filter);
                var result = _mapper.Map<IEnumerable<AnecdoteDetailDto>>(entities);

                _logger.LogInformation(
                    "Поиск по термину '{SearchTerm}' вернул {Count} результатов. CorrelationId: {CorrelationId}",
                    searchTerm, result.Count(), correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is InvalidParametersException))
            {
                _logger.LogError(ex,
                    "Ошибка при поиске анекдотов по термину '{SearchTerm}'. CorrelationId: {CorrelationId}",
                    searchTerm, correlationId);
                throw;
            }
        }

        public async Task<bool> AnecdoteExistsAsync(int id)
        {
            var correlationId = GetCorrelationId();
            if (id <= 0)
            {
                _logger.LogDebug(
                    "Получен невалидный ID {Id} для AnecdoteExistsAsync. Возвращаем false. CorrelationId: {CorrelationId}",
                    id, correlationId);
                return false;
            }

            try
            {
                var exists = await _anecdoteRepository.ExistsAsync(id);
                _logger.LogDebug("Проверка существования анекдота с ID {Id}: {Exists}. CorrelationId: {CorrelationId}",
                    id, exists, correlationId);
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Ошибка при проверке существования анекдота с ID {Id}. CorrelationId: {CorrelationId}",
                    id, correlationId);
                return false;
            }
        }

        /// <summary>
        /// Построение фильтра поиска
        /// </summary>
        private Expression<Func<DAL.Models.Anecdote, bool>>? BuildSearchFilter(string? searchTerm, int? nameMainId,
            int? languageId)
        {
            Expression<Func<DAL.Models.Anecdote, bool>>? filter = null;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string normalizedSearchTerm = searchTerm.Trim().ToLower();
                filter = a => (a.Name != null && a.Name.ToLower().Contains(normalizedSearchTerm)) ||
                              (a.Description != null && a.Description.ToLower().Contains(normalizedSearchTerm));
            }

            if (nameMainId.HasValue)
            {
                var nameFilter = (Expression<Func<DAL.Models.Anecdote, bool>>)(a => a.NameMainId == nameMainId.Value);
                filter = filter == null ? nameFilter : CombineFilters(filter, nameFilter);
            }

            if (languageId.HasValue)
            {
                var langFilter = (Expression<Func<DAL.Models.Anecdote, bool>>)(a => a.LanguageId == languageId.Value);
                filter = filter == null ? langFilter : CombineFilters(filter, langFilter);
            }

            return filter;
        }

        /// <summary>
        /// Комбинирование фильтров через AND
        /// </summary>
        private Expression<Func<DAL.Models.Anecdote, bool>> CombineFilters(
            Expression<Func<DAL.Models.Anecdote, bool>> first,
            Expression<Func<DAL.Models.Anecdote, bool>> second)
        {
            var parameter = Expression.Parameter(typeof(DAL.Models.Anecdote));
            var combined = Expression.AndAlso(
                Expression.Invoke(first, parameter),
                Expression.Invoke(second, parameter));
            return Expression.Lambda<Func<DAL.Models.Anecdote, bool>>(combined, parameter);
        }

        /// <summary>
        /// Построение выражения сортировки
        /// </summary>
        private Expression<Func<DAL.Models.Anecdote, object>> BuildSortExpression(AnecdoteSortField sortBy)
        {
            return sortBy switch
            {
                AnecdoteSortField.Name => a => a.Name ?? string.Empty,
                AnecdoteSortField.Description => a => a.Description ?? string.Empty,
                AnecdoteSortField.Id => a => a.Id,
                AnecdoteSortField.NameMain => a => a.NameMain != null ? a.NameMain.Name : string.Empty,
                AnecdoteSortField.Language => a => a.Language != null ? a.Language.Name : string.Empty,
                _ => a => a.Id
            };
        }

        // Добавить приватный метод для трансформации данных:
        private void TransformAnecdoteData<T>(T dto) where T : class
        {
            if (dto is AnecdoteCreateRequestDto createDto)
            {
                // Обрезаем имя полностью
                if (!string.IsNullOrEmpty(createDto.Name))
                {
                    createDto.Name = createDto.Name.Trim();
                }

                // Обрезаем только конечные пробелы у описания
                if (!string.IsNullOrEmpty(createDto.Description))
                {
                    createDto.Description = createDto.Description.TrimEnd();
                }
            }
            else if (dto is AnecdoteUpdateRequestDto updateDto)
            {
                // Обрезаем имя полностью
                if (!string.IsNullOrEmpty(updateDto.Name))
                {
                    updateDto.Name = updateDto.Name.Trim();
                }

                // Обрезаем только конечные пробелы у описания
                if (!string.IsNullOrEmpty(updateDto.Description))
                {
                    updateDto.Description = updateDto.Description.TrimEnd();
                }
            }
        }
    }
}