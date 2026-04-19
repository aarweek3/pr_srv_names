using AutoMapper;
using DAL.Interfaces;
using DAL.Models.SampleModels;
using DAL.Repositories.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using pr_srv_names.Exceptions;
using pr_srv_names.Models;
using pr_srv_names.Pages.SampleMain.Dtos;
using pr_srv_names.Pages.SampleMain.Interfaces;
using pr_srv_names.Pages.SampleMain.Models;
using System.Linq.Expressions;

namespace pr_srv_names.Pages.SampleMain.Services
{
    public class SampleMainService : ISampleMainService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<SampleMainService> _logger;
        private readonly IValidator<SampleMainCreateRequestDto> _createValidator;
        private readonly IValidator<SampleMainUpdateRequestDto> _updateValidator;
        private readonly IValidator<SampleMainPageRequestDto> _pageRequestValidator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SampleMainService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<SampleMainService> logger,
            IValidator<SampleMainCreateRequestDto> createValidator,
            IValidator<SampleMainUpdateRequestDto> updateValidator,
            IValidator<SampleMainPageRequestDto> pageRequestValidator,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
            _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
            _pageRequestValidator = pageRequestValidator ?? throw new ArgumentNullException(nameof(pageRequestValidator));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        private string GetCorrelationId() =>
            _httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString();

        public async Task<SampleMainPagedResponseDto> GetPagedAsync(SampleMainPageRequestDto request)
        {
            var correlationId = GetCorrelationId();
            var validationResult = await _pageRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            try
            {
                var query = _unitOfWork.SamplesMain.GetQueryable();

                // 1. Фильтрация по языку (если выбран, показываем только записи с этим переводом)
                if (request.LanguageId.HasValue)
                {
                    query = query.Where(x => x.Descriptions.Any(d => d.LanguageAppId == request.LanguageId.Value));
                }

                // 2. Поиск (по тех. имени, коду и локализованным названиям)
                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    var search = request.SearchTerm.ToLower();
                    
                    if (request.LanguageId.HasValue)
                    {
                        // Ищем в тех. имени, коде или в конкретном языке
                        query = query.Where(x => 
                            x.Name.ToLower().Contains(search) || 
                            (x.SystemCode != null && x.SystemCode.ToLower().Contains(search)) ||
                            x.Descriptions.Any(d => d.LanguageAppId == request.LanguageId.Value && d.Name.ToLower().Contains(search))
                        );
                    }
                    else
                    {
                        // Ищем в тех. имени, коде или в ЛЮБОМ из имеющихся переводов
                        query = query.Where(x => 
                            x.Name.ToLower().Contains(search) || 
                            (x.SystemCode != null && x.SystemCode.ToLower().Contains(search)) ||
                            x.Descriptions.Any(d => d.Name.ToLower().Contains(search))
                        );
                    }
                }

                var total = await query.CountAsync();
                
                // Сортировка
                query = BuildSortQuery(query, request);

                var items = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Include(x => x.Descriptions.Where(d => request.LanguageId == null || d.LanguageAppId == request.LanguageId))
                    .ToListAsync();

                var dtos = items.Select(item => {
                    var dto = _mapper.Map<SampleMainItemDto>(item);
                    // Выбираем локализованное имя: по LanguageId или первое доступное
                    var desc = item.Descriptions.FirstOrDefault(d => d.LanguageAppId == request.LanguageId) 
                               ?? item.Descriptions.FirstOrDefault();
                    dto.LocalizedName = desc?.Name;
                    return dto;
                });

                return new SampleMainPagedResponseDto
                {
                    Items = dtos,
                    Total = total,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка SampleMain. CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
        }

        public async Task<SampleMainDetailDto?> GetByIdAsync(int id)
        {
            var correlationId = GetCorrelationId();
            try
            {
                var entity = await _unitOfWork.SamplesMain.GetWithDescriptionsAsync(id);
                if (entity == null) throw new NotFoundException("SampleMain", $"Запись с ID {id} не найдена.");

                return _mapper.Map<SampleMainDetailDto>(entity);
            }
            catch (Exception ex) when (ex is not NotFoundException)
            {
                _logger.LogError(ex, "Ошибка при получении SampleMain ID {Id}. CorrelationId: {CorrelationId}", id, correlationId);
                throw;
            }
        }

        public async Task<SampleMainDetailDto> CreateAsync(SampleMainCreateRequestDto dto)
        {
            var correlationId = GetCorrelationId();
            _logger.LogInformation("Попытка создания SampleMain: {@Dto}. CorrelationId: {CorrelationId}", dto, correlationId);
            
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) 
            {
                _logger.LogWarning("Ошибка валидации при создании SampleMain: {Errors}. CorrelationId: {CorrelationId}", 
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), correlationId);
                throw new ValidationException(validationResult.Errors);
            }

            await CheckUniqueness(dto.Name, dto.SystemCode);

            try
            {
                var entity = _mapper.Map<DAL.Models.SampleModels.SampleMain>(dto);
                await _unitOfWork.SamplesMain.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                // Перечитываем из базы со всеми связанными данными (чтобы заполнились LanguageCode и т.д.)
                var createdEntity = await _unitOfWork.SamplesMain.GetWithDescriptionsAsync(entity.Id);

                _logger.LogInformation("SampleMain успешно создана. ID: {Id}, Name: {Name}. CorrelationId: {CorrelationId}", entity.Id, entity.Name, correlationId);
                return _mapper.Map<SampleMainDetailDto>(createdEntity!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании SampleMain. DTO: {@Dto}. CorrelationId: {CorrelationId}", dto, correlationId);
                throw;
            }
        }

        public async Task<SampleMainDetailDto> UpdateAsync(SampleMainUpdateRequestDto dto)
        {
            var correlationId = GetCorrelationId();
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            await CheckUniqueness(dto.Name, dto.SystemCode, dto.Id);

            var entity = await _unitOfWork.SamplesMain.GetWithDescriptionsAsync(dto.Id);
            if (entity == null) throw new NotFoundException("SampleMain", $"Запись с ID {dto.Id} не найдена.");

            try
            {
                _logger.LogInformation("Попытка обновления SampleMain ID {Id}: {@Dto}. CorrelationId: {CorrelationId}", dto.Id, dto, correlationId);

                // Обновляем основные поля
                _mapper.Map(dto, entity);

                // Синхронизация переводов
                SyncDescriptions(entity, dto.Descriptions);

                _unitOfWork.SamplesMain.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                // Перечитываем актуальное состояние
                var updatedEntity = await _unitOfWork.SamplesMain.GetWithDescriptionsAsync(entity.Id);

                _logger.LogInformation("SampleMain ID {Id} успешно обновлена. CorrelationId: {CorrelationId}", entity.Id, correlationId);
                return _mapper.Map<SampleMainDetailDto>(updatedEntity!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении SampleMain ID {Id}. CorrelationId: {CorrelationId}", dto.Id, correlationId);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            var correlationId = GetCorrelationId();
            var entity = await _unitOfWork.SamplesMain.GetByIdAsync(id);
            if (entity == null) throw new NotFoundException("SampleMain", $"Запись с ID {id} не найдена.");

            try
            {
                _unitOfWork.SamplesMain.Delete(entity);
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("SampleMain удалена. ID: {Id}. CorrelationId: {CorrelationId}", id, correlationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении SampleMain ID {Id}. CorrelationId: {CorrelationId}", id, correlationId);
                throw;
            }
        }

        public async Task<bool> IsNameUniqueAsync(string name, int? excludeId = null) =>
            await _unitOfWork.SamplesMain.IsNameUniqueAsync(name, excludeId);

        public async Task<bool> IsSystemCodeUniqueAsync(string code, int? excludeId = null) =>
            await _unitOfWork.SamplesMain.IsSystemCodeUniqueAsync(code, excludeId);

        private async Task CheckUniqueness(string name, string? systemCode, int? excludeId = null)
        {
            if (!await IsNameUniqueAsync(name, excludeId))
                throw new ConflictException("Запись с таким техническим именем уже существует.", "SampleMain", "Name");

            if (!string.IsNullOrWhiteSpace(systemCode) && !await IsSystemCodeUniqueAsync(systemCode, excludeId))
                throw new ConflictException("Запись с таким системным кодом уже существует.", "SampleMain", "SystemCode");
        }

        private void SyncDescriptions(DAL.Models.SampleModels.SampleMain entity, List<SampleMainDescriptionDto> dtos)
        {
            // 1. Обновляем существующие или добавляем новые
            foreach (var dto in dtos)
            {
                var existing = entity.Descriptions.FirstOrDefault(d => d.LanguageAppId == dto.LanguageAppId);
                if (existing != null)
                {
                    _mapper.Map(dto, existing);
                    existing.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    var newDesc = _mapper.Map<SampleMainDescription>(dto);
                    newDesc.CreatedAt = DateTime.UtcNow;
                    newDesc.UpdatedAt = DateTime.UtcNow;
                    entity.Descriptions.Add(newDesc);
                }
            }

            // 2. Удаляем те, которых нет в DTO (если это требуется бизнес-логикой)
            var dtoLangIds = dtos.Select(d => d.LanguageAppId).ToList();
            var toRemove = entity.Descriptions.Where(d => !dtoLangIds.Contains(d.LanguageAppId)).ToList();
            foreach (var remove in toRemove)
            {
                entity.Descriptions.Remove(remove);
            }
        }

        private IQueryable<DAL.Models.SampleModels.SampleMain> BuildSortQuery(IQueryable<DAL.Models.SampleModels.SampleMain> query, SampleMainPageRequestDto request)
        {
            bool isAsc = request.SortDirection == SortDirection.Asc;

            return request.SortBy switch
            {
                SampleMainSortField.Name => isAsc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name),
                SampleMainSortField.SystemCode => isAsc ? query.OrderBy(x => x.SystemCode) : query.OrderByDescending(x => x.SystemCode),
                SampleMainSortField.CreatedAt => isAsc ? query.OrderBy(x => x.CreatedAt) : query.OrderByDescending(x => x.CreatedAt),
                SampleMainSortField.UpdatedAt => isAsc ? query.OrderBy(x => x.UpdatedAt) : query.OrderByDescending(x => x.UpdatedAt),
                _ => isAsc ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id)
            };
        }
    }
}
