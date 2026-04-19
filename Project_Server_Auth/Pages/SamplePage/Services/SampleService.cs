using FluentValidation;
using pr_srv_names.Exceptions;
using pr_srv_names.Models;
using AutoMapper;
using DAL.Interfaces;
using DAL.Repositories.Interfaces;
using System.Linq.Expressions;
using pr_srv_names.Pages.Sample.Dtos;
using pr_srv_names.Pages.Sample.Interfaces;
using pr_srv_names.Pages.Sample.Models;

namespace pr_srv_names.Pages.Sample.Services
{
    /// <summary>
    /// Сервис для работы с Sample
    /// </summary>
    public class SampleService : ISampleService
    {
        private readonly ISampleRepository _sampleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<SampleService> _logger;
        private readonly IValidator<SampleCreateRequestDto> _createValidator;
        private readonly IValidator<SampleUpdateRequestDto> _updateValidator;
        private readonly IValidator<SamplePageRequestDto> _pageRequestValidator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SampleService(
            ISampleRepository sampleRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<SampleService> logger,
            IValidator<SampleCreateRequestDto> createValidator,
            IValidator<SampleUpdateRequestDto> updateValidator,
            IValidator<SamplePageRequestDto> pageRequestValidator,
            IHttpContextAccessor httpContextAccessor)
        {
            _sampleRepository = sampleRepository ?? throw new ArgumentNullException(nameof(sampleRepository));
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
                throw new InvalidParametersException($"Некорректный идентификатор Sample: {id}.");
            }
        }

        /// <summary>
        /// Валидация уникальности имени Sample
        /// </summary>
        /// <param name="name">Имя Sample</param>
        /// <param name="excludeId">ID Sample для исключения из проверки</param>
        private async Task ValidateNameUniqueness(string name, int? excludeId = null)
        {
            var correlationId = GetCorrelationId();
            var isUnique = await _sampleRepository.IsSampleNameUniqueAsync(name, excludeId);
            if (!isUnique)
            {
                _logger.LogWarning("Sample с именем '{Name}' уже существует. CorrelationId: {CorrelationId}", name,
                    correlationId);
                throw new ConflictException($"Sample с именем '{name}' уже существует.", "Sample", "Name");
            }
        }

        /// <summary>
        /// Получить Sample по ID или выбросить исключение, если не найдена
        /// </summary>
        /// <param name="id">Идентификатор Sample</param>
        /// <returns>Sample</returns>
        private async Task<DAL.Models.SampleModels.Sample> GetSampleOrThrow(int id)
        {
            var correlationId = GetCorrelationId();
            var entity = await _sampleRepository.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogWarning("Sample с ID {Id} не найден. CorrelationId: {CorrelationId}", id, correlationId);
                throw new NotFoundException("Sample", $"Sample с ID {id} не найден.");
            }

            return entity;
        }

        /// <summary>
        /// Получить все Sample в алфавитном порядке (для селектора)
        /// </summary>
        public async Task<IEnumerable<SampleDetailDto>> GetAllSamplesAsync()
        {
            var correlationId = GetCorrelationId();

            try
            {
                var items = await _sampleRepository.GetAllAsync();
                var sortedItems = items.OrderBy(c => c.Name);

                var result = _mapper.Map<IEnumerable<SampleDetailDto>>(sortedItems);
                _logger.LogInformation("Получено {Count} Sample в алфавитном порядке. CorrelationId: {CorrelationId}",
                    result.Count(), correlationId);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении всех Sample. CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
        }

        public async Task<SampleDetailDto?> GetSampleByIdAsync(int id)
        {
            var correlationId = GetCorrelationId();
            ValidateId(id, nameof(GetSampleByIdAsync));

            try
            {
                var entity = await _sampleRepository.GetByIdAsNoTrackingAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("Sample с ID {Id} не найден. CorrelationId: {CorrelationId}", id,
                        correlationId);
                    throw new NotFoundException("Sample", $"Sample с ID {id} не найден.");
                }

                var result = _mapper.Map<SampleDetailDto>(entity);
                _logger.LogInformation("Sample с ID {Id} успешно получен. CorrelationId: {CorrelationId}", id,
                    correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is NotFoundException || ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при получении Sample с ID {Id}. CorrelationId: {CorrelationId}", id,
                    correlationId);
                throw;
            }
        }

        public async Task<SamplePagedResponseDto> GetAllSamplesAsync(SamplePageRequestDto request)
        {
            var correlationId = GetCorrelationId();

            if (request == null)
            {
                _logger.LogWarning(
                    "Параметры запроса GetAllSamplesAsync отсутствуют. CorrelationId: {CorrelationId}",
                    correlationId);
                throw new InvalidParametersException("Параметры запроса отсутствуют.");
            }

            var validationResult = await _pageRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning(
                    "Валидация SamplePageRequestDto не пройдена. Ошибки: {Errors}. CorrelationId: {CorrelationId}",
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), correlationId);
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                var filter = BuildSearchFilter(request.SearchTerm);
                var orderBy = BuildSortExpression(request.SortBy);
                bool isAscending = request.SortDirection == SortDirection.Asc;

                var (items, total) = await _sampleRepository.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    orderBy,
                    isAscending,
                    filter);

                var sampleDtos = _mapper.Map<IEnumerable<SampleDetailDto>>(items);
                _logger.LogInformation(
                    "Список Sample успешно получен. Страница: {PageNumber}, Размер: {PageSize}, Всего: {Total}, Сортировка: {SortBy} {SortDirection}. CorrelationId: {CorrelationId}",
                    request.PageNumber, request.PageSize, total, request.SortBy, request.SortDirection, correlationId);

                return new SamplePagedResponseDto
                {
                    Items = sampleDtos,
                    Total = total,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }
            catch (Exception ex) when (!(ex is ValidationException || ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при получении списка Sample. CorrelationId: {CorrelationId}",
                    correlationId);
                throw;
            }
        }

        public async Task<SampleDetailDto> CreateSampleAsync(SampleCreateRequestDto request)
        {
            var correlationId = GetCorrelationId();

            if (request == null)
            {
                _logger.LogWarning("Параметры запроса CreateSampleAsync отсутствуют. CorrelationId: {CorrelationId}",
                    correlationId);
                throw new InvalidParametersException("Параметры запроса отсутствуют.");
            }

            if (request.Description != null)
            {
                request.Description = request.Description.TrimEnd();
            }

            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning(
                    "Валидация SampleCreateRequestDto не пройдена. Ошибки: {Errors}. CorrelationId: {CorrelationId}",
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), correlationId);
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                await ValidateNameUniqueness(request.Name);

                var entity = _mapper.Map<DAL.Models.SampleModels.Sample>(request);
                await _sampleRepository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<SampleDetailDto>(entity);
                _logger.LogInformation("Sample с ID {Id} успешно создан. Name: {Name}. CorrelationId: {CorrelationId}",
                    entity.Id, entity.Name, correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is ConflictException || ex is ValidationException ||
                                         ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при создании Sample. CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
        }

        public async Task<SampleDetailDto> UpdateSampleAsync(int id, SampleUpdateRequestDto request)
        {
            var correlationId = GetCorrelationId();
            ValidateId(id, nameof(UpdateSampleAsync));

            if (request == null)
            {
                _logger.LogWarning("Параметры запроса UpdateSampleAsync отсутствуют. CorrelationId: {CorrelationId}",
                    correlationId);
                throw new InvalidParametersException("Параметры запроса отсутствуют.");
            }

            request.Id = id;

            if (request.Description != null)
            {
                request.Description = request.Description.TrimEnd();
            }

            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning(
                    "Валидация SampleUpdateRequestDto не пройдена. Ошибки: {Errors}. CorrelationId: {CorrelationId}",
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), correlationId);
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                var entity = await GetSampleOrThrow(id);
                await ValidateNameUniqueness(request.Name, id);

                var originalName = entity.Name;
                _mapper.Map(request, entity);
                _sampleRepository.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<SampleDetailDto>(entity);
                _logger.LogInformation(
                    "Sample обновлен - ID: {Id}, Name: {OldName} -> {NewName}. CorrelationId: {CorrelationId}",
                    entity.Id, originalName, entity.Name, correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is NotFoundException || ex is ConflictException ||
                                         ex is ValidationException || ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при обновлении Sample с ID {Id}. CorrelationId: {CorrelationId}", id,
                    correlationId);
                throw;
            }
        }

        public async Task<bool> DeleteSampleAsync(int id)
        {
            var correlationId = GetCorrelationId();
            ValidateId(id, nameof(DeleteSampleAsync));

            try
            {
                var entity = await GetSampleOrThrow(id);

                await _sampleRepository.DeleteByIdAsync(id);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Sample с ID {Id} успешно удален. Name: {Name}. CorrelationId: {CorrelationId}",
                    entity.Id, entity.Name, correlationId);
                return true;
            }
            catch (Exception ex) when (!(ex is NotFoundException || ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при удалении Sample с ID {Id}. CorrelationId: {CorrelationId}", id,
                    correlationId);
                throw;
            }
        }

        public async Task<IEnumerable<SampleDetailDto>> GetSamplesWithDescriptionAsync()
        {
            var correlationId = GetCorrelationId();

            try
            {
                var entities = await _sampleRepository.GetSamplesWithDescriptionAsync();
                var result = _mapper.Map<IEnumerable<SampleDetailDto>>(entities);
                _logger.LogInformation("Получено {Count} Sample с описанием. CorrelationId: {CorrelationId}",
                    result.Count(), correlationId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении Sample с описанием. CorrelationId: {CorrelationId}",
                    correlationId);
                throw;
            }
        }

        public async Task<SampleDetailDto?> GetSampleByNameAsync(string name)
        {
            var correlationId = GetCorrelationId();

            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("Передано пустое имя Sample для поиска. CorrelationId: {CorrelationId}",
                    correlationId);
                throw new InvalidParametersException("Имя Sample не может быть пустым.");
            }

            try
            {
                var entity = await _sampleRepository.GetSampleByNameAsync(name);
                if (entity == null)
                {
                    _logger.LogWarning("Sample с именем '{Name}' не найден. CorrelationId: {CorrelationId}", name,
                        correlationId);
                    throw new NotFoundException("Sample", $"Sample с именем '{name}' не найден.");
                }

                var result = _mapper.Map<SampleDetailDto>(entity);
                _logger.LogInformation("Sample с именем '{Name}' успешно найден. CorrelationId: {CorrelationId}", name,
                    correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is NotFoundException || ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при поиске Sample с именем '{Name}'. CorrelationId: {CorrelationId}", name,
                    correlationId);
                throw;
            }
        }

        public async Task<IEnumerable<SampleDetailDto>> SearchSamplesByNameAsync(string searchTerm)
        {
            var correlationId = GetCorrelationId();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                _logger.LogWarning("Передан пустой поисковый термин. CorrelationId: {CorrelationId}", correlationId);
                throw new InvalidParametersException("Поисковый термин не может быть пустым.");
            }

            try
            {
                var entities = await _sampleRepository.SearchCategoriesByNameAsync(searchTerm);
                var result = _mapper.Map<IEnumerable<SampleDetailDto>>(entities);
                _logger.LogInformation(
                    "Поиск по термину '{SearchTerm}' вернул {Count} результатов. CorrelationId: {CorrelationId}",
                    searchTerm, result.Count(), correlationId);
                return result;
            }
            catch (Exception ex) when (!(ex is InvalidParametersException))
            {
                _logger.LogError(ex,
                    "Ошибка при поиске Sample по термину '{SearchTerm}'. CorrelationId: {CorrelationId}", searchTerm,
                    correlationId);
                throw;
            }
        }

        public async Task<bool> SampleExistsAsync(int id)
        {
            var correlationId = GetCorrelationId();

            if (id <= 0)
            {
                _logger.LogDebug(
                    "Получен невалидный ID {Id} для SampleExistsAsync. Возвращаем false. CorrelationId: {CorrelationId}",
                    id, correlationId);
                return false;
            }

            try
            {
                var exists = await _sampleRepository.ExistsAsync(id);
                _logger.LogDebug("Проверка существования Sample с ID {Id}: {Exists}. CorrelationId: {CorrelationId}",
                    id, exists, correlationId);
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Ошибка при проверке существования Sample с ID {Id}. CorrelationId: {CorrelationId}",
                    id, correlationId);
                return false;
            }
        }

        public async Task<bool> IsSampleNameUniqueAsync(string name, int? excludeId = null)
        {
            var correlationId = GetCorrelationId();

            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("Передано пустое имя для проверки уникальности. CorrelationId: {CorrelationId}",
                    correlationId);
                throw new InvalidParametersException("Имя Sample не может быть пустым.");
            }

            try
            {
                var isUnique = await _sampleRepository.IsSampleNameUniqueAsync(name, excludeId);
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

        // --- Implementation of Control Methods ---

        public async Task<IEnumerable<SampleControlDto>> GetAllControlAsync()
        {
            var correlationId = GetCorrelationId();
            try
            {
                var samples = await _sampleRepository.GetAllAsync();
                var result = samples.Select(r => new SampleControlDto { Id = r.Id, Name = r.Name }).ToList();
                _logger.LogInformation("Получены все Sample для контрола. Количество: {Count}. CorrelationId: {CorrelationId}",
                    result.Count, correlationId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении Sample для контрола. CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
        }

        public async Task<SampleControlDto> CreateControlAsync(SampleControlCreateDto dto)
        {
            var correlationId = GetCorrelationId();
            
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new InvalidParametersException("Имя Sample не может быть пустым.");
            }

            try
            {
                await ValidateNameUniqueness(dto.Name);

                var newSample = new DAL.Models.SampleModels.Sample { Name = dto.Name };
                await _sampleRepository.AddAsync(newSample);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Sample для контрола с ID {Id} успешно создан. CorrelationId: {CorrelationId}", 
                    newSample.Id, correlationId);

                return new SampleControlDto { Id = newSample.Id, Name = newSample.Name };
            }
            catch (Exception ex) when (!(ex is ConflictException || ex is InvalidParametersException))
            {
                _logger.LogError(ex, "Ошибка при создании Sample для контрола. CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
        }

        private Expression<Func<DAL.Models.SampleModels.Sample, bool>>? BuildSearchFilter(string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return null;

            string normalizedSearchTerm = searchTerm.Trim().ToLower();
            return c => c.Name != null && c.Name.ToLower().Contains(normalizedSearchTerm);
        }

        private Expression<Func<DAL.Models.SampleModels.Sample, object>> BuildSortExpression(SampleSortField sortBy)
        {
            return sortBy switch
            {
                SampleSortField.Name => c => c.Name ?? string.Empty,
                SampleSortField.Description => c => c.Description ?? string.Empty,
                SampleSortField.Id => c => c.Id,
                _ => c => c.Id
            };
        }
    }
}
