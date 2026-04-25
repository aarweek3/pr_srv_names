using AutoMapper;
using DAL.Interfaces;
using DAL.Models.Aggregator;
using DAL.Models.Aggregator.Localizations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using pr_srv_names.Exceptions;
using pr_srv_names.Pages.AGGREGATOR.ProgramOfAggregator.Dtos;
using pr_srv_names.Pages.AGGREGATOR.ProgramOfAggregator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project_Server_Auth.Services.Maintenance.Interfaces;

using ProgramEntity = DAL.Models.Aggregator.ProgramOfAggregator;

namespace pr_srv_names.Pages.AGGREGATOR.ProgramOfAggregator.Services
{
    public class ProgramOfAggregatorService : IProgramOfAggregatorService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<ProgramOfAggregatorService> _logger;
        private readonly IValidator<ProgramOfAggregatorCreateDto> _createValidator;
        private readonly IValidator<ProgramOfAggregatorUpdateDto> _updateValidator;
        private readonly IValidator<VersionOfAggregatorCreateDto> _vCreateValidator;
        private readonly IValidator<VersionOfAggregatorUpdateDto> _vUpdateValidator;
        private readonly IMaintenanceSeeder _maintenanceSeeder;

        public ProgramOfAggregatorService(
            IUnitOfWork uow,
            IMapper mapper,
            ILogger<ProgramOfAggregatorService> logger,
            IValidator<ProgramOfAggregatorCreateDto> createValidator,
            IValidator<ProgramOfAggregatorUpdateDto> updateValidator,
            IValidator<VersionOfAggregatorCreateDto> vCreateValidator,
            IValidator<VersionOfAggregatorUpdateDto> vUpdateValidator,
            IMaintenanceSeeder maintenanceSeeder)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _vCreateValidator = vCreateValidator;
            _vUpdateValidator = vUpdateValidator;
            _maintenanceSeeder = maintenanceSeeder;
        }

        #region Program CRUD

        public async Task<ProgramOfAggregatorPagedResponseDto> GetPagedAsync(ProgramOfAggregatorPageRequestDto request)
        {
            IQueryable<ProgramEntity> query = _uow.ProgramsOfAggregator.GetQueryable();

            if (request.ShowDeleted)
                query = query.IgnoreQueryFilters().Where(x => x.IsDeleted);

            if (request.LanguageId.HasValue)
            {
                query = query.Where(x => x.Localizations.Any(l => l.LanguageOfAggregatorId == request.LanguageId.Value));
            }

            if (request.CategoryId.HasValue)
                query = query.Where(x => x.CategoryOfAggregatorId == request.CategoryId || x.SubCategoryOfAggregatorId == request.CategoryId);

            if (request.PlatformId.HasValue)
                query = query.Where(x => x.ProgramPlatforms.Any(p => p.PlatformOfAggregatorId == request.PlatformId.Value));

            if (request.DeveloperId.HasValue)
                query = query.Where(x => x.DeveloperOfAggregatorId == request.DeveloperId.Value);

            if (request.Status.HasValue)
                query = query.Where(x => x.Status == request.Status.Value);

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var search = request.SearchTerm.ToLower();
                query = query.Where(x => 
                    x.CanonicalName.ToLower().Contains(search) || 
                    x.Slug.ToLower().Contains(search) ||
                    x.Localizations.Any(l => l.LocalizedName.ToLower().Contains(search))
                );
            }

            query = ApplySorting(query, request);

            var total = await query.CountAsync();
            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Include(x => x.CategoryOfAggregator)
                .Include(x => x.DeveloperOfAggregator)
                .Include(x => x.Versions)
                .Include(x => x.Localizations.Where(l => request.LanguageId == null || l.LanguageOfAggregatorId == request.LanguageId))
                .ToListAsync();

            var dtos = items.Select(item => {
                var dto = _mapper.Map<ProgramOfAggregatorItemDto>(item);
                var loc = item.Localizations.FirstOrDefault(l => l.LanguageOfAggregatorId == request.LanguageId) 
                          ?? item.Localizations.FirstOrDefault();
                dto.LocalizedName = loc?.LocalizedName;
                return dto;
            });

            return new ProgramOfAggregatorPagedResponseDto
            {
                Items = dtos,
                Total = total,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<ProgramOfAggregatorDetailDto?> GetByIdAsync(int id)
        {
            var entity = await _uow.ProgramsOfAggregator.GetWithDetailsAsync(id);
            if (entity == null) throw new NotFoundException("ProgramOfAggregator", $"Программа с ID {id} не найдена.");

            return _mapper.Map<ProgramOfAggregatorDetailDto>(entity);
        }

        public async Task<int> CreateAsync(ProgramOfAggregatorCreateDto dto)
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid) throw new ValidationException(validation.Errors);

            await CheckSlugUniqueness(dto.Slug);

            var entity = _mapper.Map<ProgramEntity>(dto);
            
            // Sync platforms and tags
            SyncPlatforms(entity, dto.PlatformIds);
            SyncTags(entity, dto.TagIds);

            await _uow.ProgramsOfAggregator.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return entity.Id;
        }

        public async Task UpdateAsync(ProgramOfAggregatorUpdateDto dto)
        {
            var validation = await _updateValidator.ValidateAsync(dto);
            if (!validation.IsValid) throw new ValidationException(validation.Errors);

            var entity = await _uow.ProgramsOfAggregator.GetWithDetailsAsync(dto.Id);
            if (entity == null) throw new NotFoundException("ProgramOfAggregator", $"Программа с ID {dto.Id} не найдена.");

            // Handle Slug change and Redirect
            if (entity.Slug != dto.Slug)
            {
                await CheckSlugUniqueness(dto.Slug, entity.Id);
                
                // Create redirect
                var redirect = new ProgramSlugRedirectOfAggregator
                {
                    ProgramOfAggregatorId = entity.Id,
                    OldSlug = entity.Slug,
                    NewSlug = dto.Slug
                };
                await _uow.GetRepository<ProgramSlugRedirectOfAggregator>().AddAsync(redirect);
            }

            _mapper.Map(dto, entity);
            SyncLocalizations(entity, dto.Localizations);
            SyncPlatforms(entity, dto.PlatformIds);
            SyncTags(entity, dto.TagIds);

            _uow.ProgramsOfAggregator.Update(entity);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _uow.ProgramsOfAggregator.SoftDeleteAsync(id);
            await _uow.SaveChangesAsync();
        }

        public async Task HardDeleteAsync(int id)
        {
            var entity = await _uow.ProgramsOfAggregator.GetWithDetailsAsync(id);
            if (entity == null) throw new NotFoundException("ProgramOfAggregator", $"Программа с ID {id} не найдена.");

            // Cascade delete sub-entities explicitly if not configured in DB
            // Localizations
            foreach (var loc in entity.Localizations.ToList())
                _uow.GetRepository<ProgramOfAggregatorLocalization>().Delete(loc);
            
            // Platforms
            foreach (var p in entity.ProgramPlatforms.ToList())
                _uow.GetRepository<ProgramPlatformOfAggregator>().Delete(p);
            
            // Tags
            foreach (var t in entity.ProgramTags.ToList())
                _uow.GetRepository<ProgramTagOfAggregator>().Delete(t);

            // Versions (and their links/locs)
            foreach (var v in entity.Versions.ToList())
            {
                var vFull = await _uow.VersionsOfAggregator.GetWithDetailsAsync(v.Id);
                if (vFull != null)
                {
                    foreach (var vLoc in vFull.Localizations.ToList())
                        _uow.GetRepository<VersionOfAggregatorLocalization>().Delete(vLoc);
                    foreach (var link in vFull.DownloadLinks.ToList())
                        _uow.GetRepository<DownloadLinkOfAggregator>().Delete(link);
                    _uow.VersionsOfAggregator.Delete(vFull);
                }
            }

            // MarketData
            foreach (var m in entity.MarketData.ToList())
                _uow.GetRepository<ProgramMarketDataOfAggregator>().Delete(m);

            // Redirects
            foreach (var r in entity.SlugRedirects.ToList())
                _uow.GetRepository<ProgramSlugRedirectOfAggregator>().Delete(r);

            _uow.ProgramsOfAggregator.Delete(entity);
            await _uow.SaveChangesAsync();
        }

        public async Task RestoreAsync(int id)
        {
            await _uow.ProgramsOfAggregator.RestoreAsync(id);
            await _uow.SaveChangesAsync();
        }

        public async Task<int> ClearAllAsync()
        {
            _logger.LogCritical("Агрегатор: ПОЛНАЯ ОЧИСТКА таблицы программ!");
            var result = await _uow.TruncateTableAsync("programs_of_aggregator");
            _uow.DetachAllEntities();
            return result;
        }

        public async Task<int> SeedFromJsonAsync()
        {
            _logger.LogInformation("Запущен сидинг программ из JSON.");
            try
            {
                return await _maintenanceSeeder.SeedAsync<ProgramOfAggregatorCreateDto>(
                    "Pages/AGGREGATOR/ProgramOfAggregator/Jsons/ProgramOfAggregator.json",
                    async (dto) =>
                    {
                        // В реальной системе здесь можно добавить проверку на уникальность по CanonicalName
                        // Но так как у программ основным уникальным полем является Slug (который проверяется в CreateAsync)
                        // Просто вызываем CreateAsync
                        await CreateAsync(dto);
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сидинге программ из JSON.");
                throw;
            }
        }

        #endregion

        #region Version CRUD

        public async Task<List<VersionOfAggregatorItemDto>> GetVersionsAsync(int programId)
        {
            var versions = await _uow.VersionsOfAggregator.GetByProgramIdAsync(programId);
            return _mapper.Map<List<VersionOfAggregatorItemDto>>(versions);
        }

        public async Task<VersionOfAggregatorDetailDto?> GetVersionByIdAsync(int id)
        {
            var version = await _uow.VersionsOfAggregator.GetWithDetailsAsync(id);
            return _mapper.Map<VersionOfAggregatorDetailDto>(version);
        }

        public async Task<int> CreateVersionAsync(VersionOfAggregatorCreateDto dto)
        {
            var validation = await _vCreateValidator.ValidateAsync(dto);
            if (!validation.IsValid) throw new ValidationException(validation.Errors);

            if (dto.IsLatest)
                await SwitchLatestVersion(dto.ProgramOfAggregatorId);

            var entity = _mapper.Map<VersionOfAggregator>(dto);
            await _uow.VersionsOfAggregator.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return entity.Id;
        }

        public async Task UpdateVersionAsync(VersionOfAggregatorUpdateDto dto)
        {
            var validation = await _vUpdateValidator.ValidateAsync(dto);
            if (!validation.IsValid) throw new ValidationException(validation.Errors);

            var entity = await _uow.VersionsOfAggregator.GetWithDetailsAsync(dto.Id);
            if (entity == null) throw new NotFoundException("VersionOfAggregator", $"Версия с ID {dto.Id} не найдена.");

            if (dto.IsLatest && !entity.IsLatest)
                await SwitchLatestVersion(entity.ProgramOfAggregatorId);

            _mapper.Map(dto, entity);
            // Sync localizations and links...
            
            _uow.VersionsOfAggregator.Update(entity);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteVersionAsync(int id)
        {
            var entity = await _uow.VersionsOfAggregator.GetByIdAsync(id);
            if (entity != null)
            {
                _uow.VersionsOfAggregator.Delete(entity);
                await _uow.SaveChangesAsync();
            }
        }

        #endregion

        #region Helpers

        private async Task CheckSlugUniqueness(string slug, int? excludeId = null)
        {
            // 1. Check in Programs
            if (!await _uow.ProgramsOfAggregator.IsSlugUniqueAsync(slug, excludeId))
                throw new ConflictException("Slug уже занят другой программой.", "ProgramOfAggregator", "Slug");

            // 2. Check in Redirects
            var redirectConflict = await _uow.GetRepository<ProgramSlugRedirectOfAggregator>().GetQueryable()
                .AnyAsync(r => r.OldSlug == slug && (!excludeId.HasValue || r.ProgramOfAggregatorId != excludeId.Value));
            
            if (redirectConflict)
                throw new ConflictException("Этот Slug зарезервирован как редирект. Удалите редирект, чтобы использовать его.", "ProgramOfAggregator", "Slug");
        }

        private async Task SwitchLatestVersion(int programId)
        {
            var latest = await _uow.VersionsOfAggregator.GetLatestVersionAsync(programId);
            if (latest != null)
            {
                latest.IsLatest = false;
                _uow.VersionsOfAggregator.Update(latest);
            }
        }

        private void SyncLocalizations(ProgramEntity entity, List<ProgramOfAggregatorLocalizationDto> dtos)
        {
            foreach (var dto in dtos)
            {
                var existing = entity.Localizations.FirstOrDefault(l => l.LanguageOfAggregatorId == dto.LanguageOfAggregatorId);
                if (existing != null)
                    _mapper.Map(dto, existing);
                else
                    entity.Localizations.Add(_mapper.Map<ProgramOfAggregatorLocalization>(dto));
            }
            // Remove missing...
        }

        private void SyncPlatforms(ProgramEntity entity, List<int> platformIds)
        {
            entity.ProgramPlatforms ??= new List<ProgramPlatformOfAggregator>();
            var currentIds = entity.ProgramPlatforms.Select(p => p.PlatformOfAggregatorId).ToList();
            
            // Add new
            foreach (var id in platformIds.Where(id => !currentIds.Contains(id)))
                entity.ProgramPlatforms.Add(new ProgramPlatformOfAggregator { PlatformOfAggregatorId = id });
            
            // Remove old
            var toRemove = entity.ProgramPlatforms.Where(p => !platformIds.Contains(p.PlatformOfAggregatorId)).ToList();
            foreach (var p in toRemove)
                entity.ProgramPlatforms.Remove(p);
        }

        private void SyncTags(ProgramEntity entity, List<int> tagIds)
        {
            entity.ProgramTags ??= new List<ProgramTagOfAggregator>();
            var currentIds = entity.ProgramTags.Select(t => t.TagOfAggregatorId).ToList();
            
            foreach (var id in tagIds.Where(id => !currentIds.Contains(id)))
                entity.ProgramTags.Add(new ProgramTagOfAggregator { TagOfAggregatorId = id });
            
            var toRemove = entity.ProgramTags.Where(t => !tagIds.Contains(t.TagOfAggregatorId)).ToList();
            foreach (var t in toRemove)
                entity.ProgramTags.Remove(t);
        }

        private IQueryable<ProgramEntity> ApplySorting(IQueryable<ProgramEntity> query, ProgramOfAggregatorPageRequestDto request)
        {
            var isAsc = request.SortDirection == pr_srv_names.Models.SortDirection.Asc;
            return request.SortBy switch
            {
                ProgramOfAggregatorSortField.Id => isAsc ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id),
                ProgramOfAggregatorSortField.CanonicalName => isAsc ? query.OrderBy(x => x.CanonicalName) : query.OrderByDescending(x => x.CanonicalName),
                ProgramOfAggregatorSortField.Slug => isAsc ? query.OrderBy(x => x.Slug) : query.OrderByDescending(x => x.Slug),
                ProgramOfAggregatorSortField.TotalDownloads => isAsc ? query.OrderBy(x => x.TotalDownloads) : query.OrderByDescending(x => x.TotalDownloads),
                ProgramOfAggregatorSortField.AverageRating => isAsc ? query.OrderBy(x => x.AverageRating) : query.OrderByDescending(x => x.AverageRating),
                _ => query.OrderByDescending(x => x.CreatedAt)
            };
        }

        #endregion
    }
}
