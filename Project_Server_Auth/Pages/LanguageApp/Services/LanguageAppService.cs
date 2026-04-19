using AutoMapper;
using DAL.Interfaces;
using DAL.Repositories.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;
using pr_srv_names.Exceptions;
using pr_srv_names.Pages.LanguageApp.Dtos;
using pr_srv_names.Pages.LanguageApp.Interfaces;

using System.Text.Json;
using Microsoft.AspNetCore.Hosting;

namespace pr_srv_names.Pages.LanguageApp.Services
{
    public class LanguageAppService : ILanguageAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILanguageAppRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<LanguageAppService> _logger;
        private readonly IValidator<CreateLanguageAppDto> _createValidator;
        private readonly IValidator<UpdateLanguageAppDto> _updateValidator;
        private readonly IWebHostEnvironment _env;

        public LanguageAppService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<LanguageAppService> logger,
            IValidator<CreateLanguageAppDto> createValidator,
            IValidator<UpdateLanguageAppDto> updateValidator,
            IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _repository = unitOfWork.LanguagesApp;
            _mapper = mapper;
            _logger = logger;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _env = env;
        }

        public async Task<IEnumerable<LanguageAppDto>> GetAllLanguagesAsync(bool includeDisabled = true)
        {
            var languages = await _repository.GetAllAsync();
            if (!includeDisabled)
            {
                languages = languages.Where(x => x.Enabled);
            }
            return _mapper.Map<IEnumerable<LanguageAppDto>>(languages.OrderBy(x => x.SortOrder));
        }

        public async Task<IEnumerable<LanguageAppDto>> GetAvailableLanguagesAsync()
        {
            return await GetAllLanguagesAsync(false);
        }

        public async Task<LanguageAppDto> GetLanguageByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new NotFoundException("LanguageApp", $"Language with ID {id} not found.");
            }
            return _mapper.Map<LanguageAppDto>(entity);
        }

        public async Task<LanguageAppDto> CreateLanguageAsync(CreateLanguageAppDto request)
        {
            await _createValidator.ValidateAndThrowAsync(request);

            if (!await _repository.IsCodeUniqueAsync(request.Code))
                throw new ConflictException("Language code must be unique.", "LanguageApp", "Code");

            if (!await _repository.IsShortCodeUniqueAsync(request.ShortCode))
                throw new ConflictException("Language short code must be unique.", "LanguageApp", "ShortCode");

            var entity = _mapper.Map<DAL.Models.LocalizationModels.LanguageApp>(request);

            if (entity.IsDefault)
            {
                await ResetOtherDefaults();
            }

            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<LanguageAppDto>(entity);
        }

        public async Task<LanguageAppDto> UpdateLanguageAsync(int id, UpdateLanguageAppDto request)
        {
            await _updateValidator.ValidateAndThrowAsync(request);

            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException("LanguageApp", $"Language with ID {id} not found.");

            if (request.Code != null && request.Code != entity.Code)
            {
                if (!await _repository.IsCodeUniqueAsync(request.Code, id))
                    throw new ConflictException("Language code must be unique.", "LanguageApp", "Code");
            }

            if (request.ShortCode != null && request.ShortCode != entity.ShortCode)
            {
                if (!await _repository.IsShortCodeUniqueAsync(request.ShortCode, id))
                    throw new ConflictException("Language short code must be unique.", "LanguageApp", "ShortCode");
            }

            _mapper.Map(request, entity);

            if (request.IsDefault == true && entity.IsDefault)
            {
                await ResetOtherDefaults(id);
            }

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<LanguageAppDto>(entity);
        }

        public async Task<bool> DeleteLanguageAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException("LanguageApp", $"Language with ID {id} not found.");

            if (entity.IsSystem)
                throw new ForbiddenAccessException("Cannot delete system language.");

            if (entity.IsDefault)
                throw new InvalidParametersException("Cannot delete default language. Set another language as default first.");

            await _repository.DeleteByIdAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetDefaultLanguageAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException("LanguageApp", $"Language with ID {id} not found.");

            if (!entity.Enabled)
                throw new InvalidParametersException("Cannot set disabled language as default.");

            await ResetOtherDefaults();
            entity.IsDefault = true;
            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleLanguageStatusAsync(int id, bool enabled)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException("LanguageApp", $"Language with ID {id} not found.");

            if (entity.IsSystem && !enabled)
                throw new ForbiddenAccessException("Cannot disable system language.");

            if (entity.IsDefault && !enabled)
                throw new InvalidParametersException("Cannot disable default language.");

            entity.Enabled = enabled;
            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task HardResetAsync()
        {
            _logger.LogWarning("Выполняется полная очистка таблицы языков приложения через TRUNCATE CASCADE.");
            
            // Используем TRUNCATE для быстрой очистки, сброса ID (RESTART IDENTITY) 
            // и каскадного удаления зависимостей (CASCADE)
            var sql = "TRUNCATE TABLE \"LanguagesApp\" RESTART IDENTITY CASCADE;";
            await _unitOfWork.ExecuteSqlRawAsync(sql);
            
            _logger.LogInformation("Таблица языков успешно очищена, ID сброшены.");
        }

        public async Task InitializeAsync()
        {
            var count = await _repository.CountAsync();
            if (count > 0)
                throw new InvalidOperationException("Инициализация невозможна: в базе данных уже есть языки.");

            var jsonPath = Path.Combine(_env.WebRootPath, "data", "default-languages.json");
            if (!File.Exists(jsonPath))
                throw new FileNotFoundException("Файл конфигурации языков не найден.", jsonPath);

            var jsonString = await File.ReadAllTextAsync(jsonPath);
            var languages = JsonSerializer.Deserialize<List<DAL.Models.LocalizationModels.LanguageApp>>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (languages == null || !languages.Any()) return;

            foreach (var lang in languages)
            {
                await _repository.AddAsync(lang);
            }
            
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("База данных языков успешно инициализирована из JSON.");
        }

        private async Task ResetOtherDefaults(int? excludeId = null)
        {
            var languages = await _repository.FindAsync(x => x.IsDefault);
            foreach (var lang in languages)
            {
                if (excludeId.HasValue && lang.Id == excludeId.Value)
                    continue;

                lang.IsDefault = false;
                _repository.Update(lang);
            }
        }
    }
}
