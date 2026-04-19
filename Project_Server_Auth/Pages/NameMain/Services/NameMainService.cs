using pr_srv_names.Exceptions;
using pr_srv_names.Models;
using AutoMapper;
using DAL.Interfaces;
using DAL.Repositories.Interfaces;
using FluentValidation;
using System.Linq.Expressions;
using pr_srv_names.Pages.NameMain.Dtos;
using pr_srv_names.Pages.NameMain.Intarfaces;
using pr_srv_names.Pages.NameMain.Models;

namespace pr_srv_names.Pages.NameMain.Services
{
    /// <summary>
    /// Сервис для работы с namemainами
    /// </summary>
    public class NameMainService : INameMainService
    {
        private readonly INameMainRepository _namemainrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<NameMainService> _logger;
        private readonly IValidator<NameMainCreateRequestDto> _createValidator;
        private readonly IValidator<NameMainUpdateRequestDto> _updateValidator;
        private readonly IValidator<NameMainPageRequestDto> _pageRequestValidator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NameMainService(
            INameMainRepository namemainrepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<NameMainService> logger,
            IValidator<NameMainCreateRequestDto> createValidator,
            IValidator<NameMainUpdateRequestDto> updateValidator,
            IValidator<NameMainPageRequestDto> pageRequestValidator,
            IHttpContextAccessor httpContextAccessor)
        {
            _namemainrepository = namemainrepository ?? throw new ArgumentNullException(nameof(namemainrepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
            _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
            _pageRequestValidator = pageRequestValidator ?? throw new ArgumentNullException(nameof(pageRequestValidator));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        private string GetCorrelationId() => _httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString();

        private void ValidateId(int id, string operationName)
        {
            if (id <= 0)
            {
                var correlationId = GetCorrelationId();
                _logger.LogWarning("Получен невалидный ID {Id} для {Operation}. CorrelationId: {CorrelationId}",
                    id, operationName, correlationId);
                throw new InvalidParametersException($"Некорректный идентификатор namemain: {id}.");
            }
        }

        /// <summary>
        /// Валидация уникальности имени namemain
        /// </summary>
        /// <param name="name">Имя namemain</param>
        /// <param name="excludeId">ID namemain для исключения из проверки</param>
        private async Task ValidateNameUniqueness(string name, int? excludeId = null)
        {
            var correlationId = GetCorrelationId();
            var isUnique = await _namemainrepository.IsNameMainNameUniqueAsync(name, excludeId);
            if (!isUnique)
            {
                _logger.LogWarning("NameMainа с именем '{Name}' уже существует. CorrelationId: {CorrelationId}", name, correlationId);
                throw new ConflictException($"NameMainа с именем '{name}' уже существует.", "NameMain", "Name");
            }
        }

        /// <summary>
        /// Получить namemain по ID или выбросить исключение, если не найдена
        /// </summary>
        /// <param name="id">Идентификатор namemain</param>
        /// <returns>NameMainа</returns>
        private async Task<DAL.Models.NameModels.NameMain> GetNameMainOrThrow(int id)
        {
            var correlationId = GetCorrelationId();
            var entity = await _namemainrepository.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogWarning("NameMain с ID {Id} не найдена. CorrelationId: {CorrelationId}", id, correlationId);
                throw new NotFoundException("NameMain", $"NameMain с ID {id} не найдена.");
            }
            return entity;
        }

        /// <summary>
        /// Получить все namemain в алфавитном порядке (для селектора)
        /// </summary>
        public async Task<IEnumerable<NameMainDetailDto>> GetAllCategoriesAsync()
        {
            var correlationId = GetCorrelationId();

            try
            {
                // Просто получаем все namemain
                var items = await _namemainrepository.GetAllAsync();

                // Сортируем в памяти по алфавиту
                var sortedItems = items.OrderBy(c => c.Name);

                var result = _mapper.Map<IEnumerable<NameMainDetailDto>>(sortedItems);
                _logger.LogInformation("Получено {Count} namemain в алфавитном порядке. CorrelationId: {CorrelationId}",
                    result.Count(), correlationId);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении всех namemain. CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
        }

        public async Task<NameMainDetailDto?> GetNameMainByIdAsync(int id)
        {
            var correlationId = GetCorrelationId();
            ValidateId(id, nameof(GetNameMainByIdAsync));

            try
            {
                var entity = await _namemainrepository.GetByIdAsNoTrackingAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("NameMain с ID {Id} не найдена. CorrelationId: {CorrelationId}", id, correlationId);
                    throw new NotFoundException("NameMain", $"NameMain с ID {id} не найдена.");
                }

                var result = _mapper.Map<NameMainDetailDto>(entity);
                _logger.LogInformation("NameMain с ID {Id} успешно получена. CorrelationId: {CorrelationId}", id, correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is NotFoundException || ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при получении NameMain с ID {Id}. CorrelationId: {CorrelationId}", id, correlationId);
                throw;
            }
        }

        public async Task<NameMainPagedResponseDto> GetAllCategoriesAsync(NameMainPageRequestDto request)
        {
            var correlationId = GetCorrelationId();

            if (request == null)
            {
                _logger.LogWarning("Параметры запроса GetAllCategoriesAsync отсутствуют. CorrelationId: {CorrelationId}", correlationId);
                throw new InvalidParametersException("Параметры запроса отсутствуют.");
            }

            var validationResult = await _pageRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Валидация NameMainPageRequestDto не пройдена. Ошибки: {Errors}. CorrelationId: {CorrelationId}",
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), correlationId);
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                var filter = BuildSearchFilter(request.SearchTerm);
                var orderBy = BuildSortExpression(request.SortBy);
                bool isAscending = request.SortDirection == SortDirection.Asc;

                var (items, total) = await _namemainrepository.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    orderBy,
                    isAscending,
                    filter);

                var namemaindtos = _mapper.Map<IEnumerable<NameMainDetailDto>>(items);
                _logger.LogInformation("Список NameMain успешно получен. Страница: {PageNumber}, Размер: {PageSize}, Всего: {Total}, Сортировка: {SortBy} {SortDirection}. CorrelationId: {CorrelationId}",
                    request.PageNumber, request.PageSize, total, request.SortBy, request.SortDirection, correlationId);

                return new NameMainPagedResponseDto
                {
                    Items = namemaindtos,
                    Total = total,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }
            catch (Exception ex) when (!(ex is ValidationException || ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при получении списка NameMain. CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
        }

        public async Task<NameMainDetailDto> CreateNameMainAsync(NameMainCreateRequestDto request)
        {
            var correlationId = GetCorrelationId();

            if (request == null)
            {
                _logger.LogWarning("Параметры запроса CreateNameMainAsync отсутствуют. CorrelationId: {CorrelationId}", correlationId);
                throw new InvalidParametersException("Параметры запроса отсутствуют.");
            }

            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Валидация NameMainCreateRequestDto не пройдена. Ошибки: {Errors}. CorrelationId: {CorrelationId}",
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), correlationId);
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                // Используем общий метод валидации уникальности
                await ValidateNameUniqueness(request.Name);

                var entity = _mapper.Map<DAL.Models.NameModels.NameMain>(request);
                await _namemainrepository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<NameMainDetailDto>(entity);
                _logger.LogInformation("NameMain с ID {Id} успешно создана. Name: {Name}. CorrelationId: {CorrelationId}",
                    entity.Id, entity.Name, correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is ConflictException || ex is ValidationException || ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при создании NameMain. CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
        }

        public async Task<NameMainDetailDto> UpdateNameMainAsync(int id, NameMainUpdateRequestDto request)
        {
            var correlationId = GetCorrelationId();
            ValidateId(id, nameof(UpdateNameMainAsync));

            if (request == null)
            {
                _logger.LogWarning("Параметры запроса UpdateNameMainAsync отсутствуют. CorrelationId: {CorrelationId}", correlationId);
                throw new InvalidParametersException("Параметры запроса отсутствуют.");
            }

            request.Id = id;
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Валидация NameMainUpdateRequestDto не пройдена. Ошибки: {Errors}. CorrelationId: {CorrelationId}",
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), correlationId);
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                // Используем общий метод для получения namemain
                var entity = await GetNameMainOrThrow(id);

                // Используем общий метод валидации уникальности с исключением текущего ID
                await ValidateNameUniqueness(request.Name, id);

                var originalName = entity.Name;
                _mapper.Map(request, entity);
                _namemainrepository.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<NameMainDetailDto>(entity);
                _logger.LogInformation("NameMain обновлена - ID: {Id}, Name: {OldName} -> {NewName}. CorrelationId: {CorrelationId}",
                    entity.Id, originalName, entity.Name, correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is NotFoundException || ex is ConflictException || ex is ValidationException || ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при обновлении NameMain с ID {Id}. CorrelationId: {CorrelationId}", id, correlationId);
                throw;
            }
        }

        public async Task<bool> DeleteNameMainAsync(int id)
        {
            var correlationId = GetCorrelationId();
            ValidateId(id, nameof(DeleteNameMainAsync));

            try
            {
                // Используем общий метод для получения namemain
                var entity = await GetNameMainOrThrow(id);

                await _namemainrepository.DeleteByIdAsync(id);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("NameMain с ID {Id} успешно удалена. Name: {Name}. CorrelationId: {CorrelationId}",
                    entity.Id, entity.Name, correlationId);
                return true;
            }
            catch (Exception ex) when (!(ex is NotFoundException || ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при удалении NameMain с ID {Id}. CorrelationId: {CorrelationId}", id, correlationId);
                throw;
            }
        }

        public async Task<IEnumerable<NameMainDetailDto>> GetCategoriesWithDescriptionAsync()
        {
            var correlationId = GetCorrelationId();

            try
            {
                var entities = await _namemainrepository.GetNameMainsWithDescriptionAsync();
                var result = _mapper.Map<IEnumerable<NameMainDetailDto>>(entities);
                _logger.LogInformation("Получено {Count} namemain с описанием. CorrelationId: {CorrelationId}",
                    result.Count(), correlationId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении namemain с описанием. CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
        }

        public async Task<NameMainDetailDto?> GetNameMainByNameAsync(string name)
        {
            var correlationId = GetCorrelationId();

            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("Передано пустое имя namemain для поиска. CorrelationId: {CorrelationId}", correlationId);
                throw new InvalidParametersException("Имя namemain не может быть пустым.");
            }

            try
            {
                var entity = await _namemainrepository.GetNameMainByNameAsync(name);
                if (entity == null)
                {
                    _logger.LogWarning("NameMain с именем '{Name}' не найдена. CorrelationId: {CorrelationId}", name, correlationId);
                    throw new NotFoundException("NameMain", $"NameMain с именем '{name}' не найдена.");
                }

                var result = _mapper.Map<NameMainDetailDto>(entity);
                _logger.LogInformation("NameMain с именем '{Name}' успешно найдена. CorrelationId: {CorrelationId}", name, correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is NotFoundException || ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при поиске NameMain с именем '{Name}'. CorrelationId: {CorrelationId}", name, correlationId);
                throw;
            }
        }

        public async Task<IEnumerable<NameMainDetailDto>> SearchCategoriesByNameAsync(string searchTerm)
        {
            var correlationId = GetCorrelationId();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                _logger.LogWarning("Передан пустой поисковый термин. CorrelationId: {CorrelationId}", correlationId);
                throw new InvalidParametersException("Поисковый термин не может быть пустым.");
            }

            try
            {
                var entities = await _namemainrepository.SearchCategoriesByNameAsync(searchTerm);
                var result = _mapper.Map<IEnumerable<NameMainDetailDto>>(entities);
                _logger.LogInformation("Поиск по термину '{SearchTerm}' вернул {Count} результатов. CorrelationId: {CorrelationId}",
                    searchTerm, result.Count(), correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при поиске namemain по термину '{SearchTerm}'. CorrelationId: {CorrelationId}", searchTerm, correlationId);
                throw;
            }
        }

        public async Task<bool> NameMainExistsAsync(int id)
        {
            var correlationId = GetCorrelationId();

            if (id <= 0)
            {
                _logger.LogDebug("Получен невалидный ID {Id} для NameMainExistsAsync. Возвращаем false. CorrelationId: {CorrelationId}",
                    id, correlationId);
                return false;
            }

            try
            {
                var exists = await _namemainrepository.ExistsAsync(id);
                _logger.LogDebug("Проверка существования NameMain с ID {Id}: {Exists}. CorrelationId: {CorrelationId}",
                    id, exists, correlationId);
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при проверке существования NameMain с ID {Id}. CorrelationId: {CorrelationId}",
                    id, correlationId);
                return false;
            }
        }

        public async Task<bool> IsNameMainNameUniqueAsync(string name, int? excludeId = null)
        {
            var correlationId = GetCorrelationId();

            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("Передано пустое имя для проверки уникальности. CorrelationId: {CorrelationId}", correlationId);
                throw new InvalidParametersException("Имя namemain не может быть пустым.");
            }

            try
            {
                var isUnique = await _namemainrepository.IsNameMainNameUniqueAsync(name, excludeId);
                _logger.LogDebug("Проверка уникальности имени '{Name}' (исключение ID: {ExcludeId}): {IsUnique}. CorrelationId: {CorrelationId}",
                    name, excludeId, isUnique, correlationId);
                return isUnique;
            }
            catch (Exception ex) when (!(ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при проверке уникальности имени '{Name}'. CorrelationId: {CorrelationId}", name, correlationId);
                throw;
            }
        }

        /// <summary>
        /// Построение фильтра поиска по имени namemain
        /// </summary>
        /// <param name="searchTerm">Поисковый термин</param>
        /// <returns>Expression для фильтрации или null</returns>
        private Expression<Func<DAL.Models.NameModels.NameMain, bool>>? BuildSearchFilter(string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return null;

            string normalizedSearchTerm = searchTerm.Trim().ToLower();
            return c => c.Name != null && c.Name.ToLower().Contains(normalizedSearchTerm);
        }

        /// <summary>
        /// Построение выражения сортировки на основе enum поля
        /// </summary>
        /// <param name="sortBy">Поле сортировки</param>
        /// <returns>Expression для сортировки</returns>
        private Expression<Func<DAL.Models.NameModels.NameMain, object>> BuildSortExpression(NameMainSortField sortBy)
        {
            return sortBy switch
            {
                NameMainSortField.Name => c => c.Name ?? string.Empty,
                NameMainSortField.Description => c => c.Description ?? string.Empty,
                NameMainSortField.Id => c => c.Id,
                _ => c => c.Id // Fallback на Id для неизвестных значений
            };
        }
    }
}