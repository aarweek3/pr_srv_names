using pr_srv_names.Exceptions;
using pr_srv_names.Models;
using AutoMapper;
using DAL.Interfaces;
using DAL.Repositories.Interfaces;
using FluentValidation;
using System.Linq.Expressions;
using pr_srv_names.Pages.Language.Dtos;
using pr_srv_names.Pages.Language.Intarfaces;
using pr_srv_names.Pages.Language.Models;

namespace pr_srv_names.Pages.Language.Services
{
    /// <summary>
    /// Сервис для работы с languageами
    /// </summary>
    public class LanguageService : ILanguageService
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<LanguageService> _logger;
        private readonly IValidator<LanguageCreateRequestDto> _createValidator;
        private readonly IValidator<LanguageUpdateRequestDto> _updateValidator;
        private readonly IValidator<LanguagePageRequestDto> _pageRequestValidator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LanguageService(
            ILanguageRepository languageRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<LanguageService> logger,
            IValidator<LanguageCreateRequestDto> createValidator,
            IValidator<LanguageUpdateRequestDto> updateValidator,
            IValidator<LanguagePageRequestDto> pageRequestValidator,
            IHttpContextAccessor httpContextAccessor)
        {
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
                throw new InvalidParametersException($"Некорректный идентификатор language: {id}.");
            }
        }

        /// <summary>
        /// Валидация уникальности имени и кода языка
        /// </summary>
        private async Task ValidateNameAndCodeUniqueness(string name, string code, int? excludeId = null)
        {
            var correlationId = GetCorrelationId();
            var isNameUnique = await _languageRepository.IsLanguageNameUniqueAsync(name, excludeId);
            if (!isNameUnique)
            {
                _logger.LogWarning("Language с именем '{Name}' уже существует. CorrelationId: {CorrelationId}", name,
                    correlationId);
                throw new ConflictException($"Language с именем '{name}' уже существует.", "Language", "Name");
            }

            var isCodeUnique = await _languageRepository.IsLanguageCodeUniqueAsync(code, excludeId);
            if (!isCodeUnique)
            {
                _logger.LogWarning("Language с кодом '{Code}' уже существует. CorrelationId: {CorrelationId}", code,
                    correlationId);
                throw new ConflictException($"Language с кодом '{code}' уже существует.", "Language", "Code");
            }
        }

        /// <summary>
        /// Получить language по ID или выбросить исключение, если не найдена
        /// </summary>
        private async Task<DAL.Models.LocalizationModels.Language> GetLanguageOrThrow(int id)
        {
            var correlationId = GetCorrelationId();
            var entity = await _languageRepository.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogWarning("Language с ID {Id} не найдена. CorrelationId: {CorrelationId}", id, correlationId);
                throw new NotFoundException("Language", $"Language с ID {id} не найдена.");
            }

            return entity;
        }

        public async Task<IEnumerable<LanguageDetailDto>> GetAllCategoriesAsync()
        {
            var correlationId = GetCorrelationId();
            try
            {
                var items = await _languageRepository.GetAllAsync();
                var sortedItems = items.OrderBy(c => c.Name);
                var result = _mapper.Map<IEnumerable<LanguageDetailDto>>(sortedItems);
                _logger.LogInformation("Получено {Count} language в алфавитном порядке. CorrelationId: {CorrelationId}",
                    result.Count(), correlationId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении всех language. CorrelationId: {CorrelationId}",
                    correlationId);
                throw;
            }
        }

        public async Task<LanguageDetailDto?> GetLanguageByIdAsync(int id)
        {
            var correlationId = GetCorrelationId();
            ValidateId(id, nameof(GetLanguageByIdAsync));
            try
            {
                var entity = await _languageRepository.GetByIdAsNoTrackingAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("Language с ID {Id} не найдена. CorrelationId: {CorrelationId}", id,
                        correlationId);
                    throw new NotFoundException("Language", $"Language с ID {id} не найдена.");
                }

                var result = _mapper.Map<LanguageDetailDto>(entity);
                _logger.LogInformation("Language с ID {Id} успешно получена. CorrelationId: {CorrelationId}", id,
                    correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is NotFoundException || ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при получении Language с ID {Id}. CorrelationId: {CorrelationId}", id,
                    correlationId);
                throw;
            }
        }

        public async Task<LanguagePagedResponseDto> GetAllCategoriesAsync(LanguagePageRequestDto request)
        {
            var correlationId = GetCorrelationId();
            if (request == null)
            {
                _logger.LogWarning(
                    "Параметры запроса GetAllCategoriesAsync отсутствуют. CorrelationId: {CorrelationId}",
                    correlationId);
                throw new InvalidParametersException("Параметры запроса отсутствуют.");
            }

            var validationResult = await _pageRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning(
                    "Валидация LanguagePageRequestDto не пройдена. Ошибки: {Errors}. CorrelationId: {CorrelationId}",
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), correlationId);
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                var filter = BuildSearchFilter(request.SearchTerm);
                var orderBy = BuildSortExpression(request.SortBy);
                bool isAscending = request.SortDirection == SortDirection.Asc;
                var (items, total) = await _languageRepository.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    orderBy,
                    isAscending,
                    filter);
                var languageDtos = _mapper.Map<IEnumerable<LanguageDetailDto>>(items);
                _logger.LogInformation(
                    "Список Language успешно получен. Страница: {PageNumber}, Размер: {PageSize}, Всего: {Total}, Сортировка: {SortBy} {SortDirection}. CorrelationId: {CorrelationId}",
                    request.PageNumber, request.PageSize, total, request.SortBy, request.SortDirection, correlationId);
                return new LanguagePagedResponseDto
                {
                    Items = languageDtos,
                    Total = total,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }
            catch (Exception ex) when (!(ex is ValidationException || ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при получении списка Language. CorrelationId: {CorrelationId}",
                    correlationId);
                throw;
            }
        }

        public async Task<LanguageDetailDto> CreateLanguageAsync(LanguageCreateRequestDto request)
        {
            var correlationId = GetCorrelationId();
            if (request == null)
            {
                _logger.LogWarning("Параметры запроса CreateLanguageAsync отсутствуют. CorrelationId: {CorrelationId}",
                    correlationId);
                throw new InvalidParametersException("Параметры запроса отсутствуют.");
            }

            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning(
                    "Валидация LanguageCreateRequestDto не пройдена. Ошибки: {Errors}. CorrelationId: {CorrelationId}",
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), correlationId);
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                await ValidateNameAndCodeUniqueness(request.Name, request.Code);
                var entity = _mapper.Map<DAL.Models.LocalizationModels.Language>(request);
                await _languageRepository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var result = _mapper.Map<LanguageDetailDto>(entity);
                _logger.LogInformation(
                    "Language с ID {Id} успешно создана. Code: {Code}, Name: {Name}. CorrelationId: {CorrelationId}",
                    entity.Id, entity.Code, entity.Name, correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is ConflictException || ex is ValidationException ||
                                         ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при создании Language. CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
        }

        public async Task<LanguageDetailDto> UpdateLanguageAsync(int id, LanguageUpdateRequestDto request)
        {
            var correlationId = GetCorrelationId();
            ValidateId(id, nameof(UpdateLanguageAsync));
            if (request == null)
            {
                _logger.LogWarning("Параметры запроса UpdateLanguageAsync отсутствуют. CorrelationId: {CorrelationId}",
                    correlationId);
                throw new InvalidParametersException("Параметры запроса отсутствуют.");
            }

            request.Id = id;
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning(
                    "Валидация LanguageUpdateRequestDto не пройдена. Ошибки: {Errors}. CorrelationId: {CorrelationId}",
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), correlationId);
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                var entity = await GetLanguageOrThrow(id);
                await ValidateNameAndCodeUniqueness(request.Name, request.Code, id);
                var originalName = entity.Name;
                var originalCode = entity.Code;
                _mapper.Map(request, entity);
                _languageRepository.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var result = _mapper.Map<LanguageDetailDto>(entity);
                _logger.LogInformation(
                    "Language обновлена - ID: {Id}, Code: {OldCode} -> {NewCode}, Name: {OldName} -> {NewName}. CorrelationId: {CorrelationId}",
                    entity.Id, originalCode, entity.Code, originalName, entity.Name, correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is NotFoundException || ex is ConflictException ||
                                         ex is ValidationException || ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при обновлении Language с ID {Id}. CorrelationId: {CorrelationId}", id,
                    correlationId);
                throw;
            }
        }

        public async Task<bool> DeleteLanguageAsync(int id)
        {
            var correlationId = GetCorrelationId();
            ValidateId(id, nameof(DeleteLanguageAsync));
            try
            {
                var entity = await GetLanguageOrThrow(id);
                await _languageRepository.DeleteByIdAsync(id);
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation(
                    "Language с ID {Id} успешно удалена. Code: {Code}, Name: {Name}. CorrelationId: {CorrelationId}",
                    entity.Id, entity.Code, entity.Name, correlationId);
                return true;
            }
            catch (Exception ex) when (!(ex is NotFoundException || ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при удалении Language с ID {Id}. CorrelationId: {CorrelationId}", id,
                    correlationId);
                throw;
            }
        }

        public async Task<IEnumerable<LanguageDetailDto>> GetCategoriesWithDescriptionAsync()
        {
            var correlationId = GetCorrelationId();
            try
            {
                var entities = await _languageRepository.GetLanguagesWithDescriptionAsync();
                var result = _mapper.Map<IEnumerable<LanguageDetailDto>>(entities);
                _logger.LogInformation("Получено {Count} language с описанием. CorrelationId: {CorrelationId}",
                    result.Count(), correlationId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении language с описанием. CorrelationId: {CorrelationId}",
                    correlationId);
                throw;
            }
        }

        public async Task<LanguageDetailDto?> GetLanguageByNameAsync(string name)
        {
            var correlationId = GetCorrelationId();
            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("Передано пустое имя language для поиска. CorrelationId: {CorrelationId}",
                    correlationId);
                throw new InvalidParametersException("Имя language не может быть пустым.");
            }

            try
            {
                var entity = await _languageRepository.GetLanguageByNameAsync(name);
                if (entity == null)
                {
                    _logger.LogWarning("Language с именем '{Name}' не найдена. CorrelationId: {CorrelationId}", name,
                        correlationId);
                    throw new NotFoundException("Language", $"Language с именем '{name}' не найдена.");
                }

                var result = _mapper.Map<LanguageDetailDto>(entity);
                _logger.LogInformation("Language с именем '{Name}' успешно найдена. CorrelationId: {CorrelationId}",
                    name, correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is NotFoundException || ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при поиске Language с именем '{Name}'. CorrelationId: {CorrelationId}",
                    name, correlationId);
                throw;
            }
        }

        public async Task<IEnumerable<LanguageDetailDto>> SearchCategoriesByNameAsync(string searchTerm)
        {
            var correlationId = GetCorrelationId();
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                _logger.LogWarning("Передан пустой поисковый термин. CorrelationId: {CorrelationId}", correlationId);
                throw new InvalidParametersException("Поисковый термин не может быть пустым.");
            }

            try
            {
                var entities = await _languageRepository.SearchCategoriesByNameAsync(searchTerm);
                var result = _mapper.Map<IEnumerable<LanguageDetailDto>>(entities);
                _logger.LogInformation(
                    "Поиск по термину '{SearchTerm}' вернул {Count} результатов. CorrelationId: {CorrelationId}",
                    searchTerm, result.Count(), correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is InvalidParametersException))
            {
                _logger.LogError(ex,
                    "Ошибка при поиске language по термину '{SearchTerm}'. CorrelationId: {CorrelationId}", searchTerm,
                    correlationId);
                throw;
            }
        }

        public async Task<bool> LanguageExistsAsync(int id)
        {
            var correlationId = GetCorrelationId();
            if (id <= 0)
            {
                _logger.LogDebug(
                    "Получен невалидный ID {Id} для LanguageExistsAsync. Возвращаем false. CorrelationId: {CorrelationId}",
                    id, correlationId);
                return false;
            }

            try
            {
                var exists = await _languageRepository.ExistsAsync(id);
                _logger.LogDebug("Проверка существования Language с ID {Id}: {Exists}. CorrelationId: {CorrelationId}",
                    id, exists, correlationId);
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Ошибка при проверке существования Language с ID {Id}. CorrelationId: {CorrelationId}",
                    id, correlationId);
                return false;
            }
        }

        public async Task<bool> IsLanguageNameUniqueAsync(string name, int? excludeId = null)
        {
            var correlationId = GetCorrelationId();
            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("Передано пустое имя для проверки уникальности. CorrelationId: {CorrelationId}",
                    correlationId);
                throw new InvalidParametersException("Имя language не может быть пустым.");
            }

            try
            {
                var isUnique = await _languageRepository.IsLanguageNameUniqueAsync(name, excludeId);
                _logger.LogDebug(
                    "Проверка уникальности имени '{Name}' (исключение ID: {ExcludeId}): {IsUnique}. CorrelationId: {CorrelationId}",
                    name, excludeId, isUnique, correlationId);
                return isUnique;
            }
            catch (Exception ex) when (!(ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при проверке уникальности имени '{Name}'. CorrelationId: {CorrelationId}",
                    name, correlationId);
                throw;
            }
        }

        /// <summary>
        /// Построение фильтра поиска по имени или коду языка
        /// </summary>
        private Expression<Func<DAL.Models.LocalizationModels.Language, bool>>? BuildSearchFilter(string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return null;
            string normalizedSearchTerm = searchTerm.Trim().ToLower();
            return c => (c.Name != null && c.Name.ToLower().Contains(normalizedSearchTerm)) ||
                        (c.Code != null && c.Code.ToLower().Contains(normalizedSearchTerm));
        }

        /// <summary>
        /// Построение выражения сортировки на основе enum поля
        /// </summary>
        private Expression<Func<DAL.Models.LocalizationModels.Language, object>> BuildSortExpression(LanguageSortField sortBy)
        {
            return sortBy switch
            {
                LanguageSortField.Code => c => c.Code ?? string.Empty,
                LanguageSortField.Name => c => c.Name ?? string.Empty,
                LanguageSortField.Description => c => c.Description ?? string.Empty,
                LanguageSortField.Id => c => c.Id,
                _ => c => c.Id // Fallback на Id для неизвестных значений
            };
        }
    }
}