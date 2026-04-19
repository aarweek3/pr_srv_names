using AutoMapper;
using DAL.Interfaces;
using DAL.Repositories.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using pr_srv_names.Exceptions;
using pr_srv_names.Pages.Aggregator.LanguageOfAggregator.Dtos;
using pr_srv_names.Pages.Aggregator.LanguageOfAggregator.Interfaces;
using LanguageOfAggregatorEntity = DAL.Models.Aggregator.LanguageOfAggregator;

namespace pr_srv_names.Pages.Aggregator.LanguageOfAggregator.Services
{
    public class LanguageOfAggregatorService : ILanguageOfAggregatorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILanguageOfAggregatorRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<LanguageOfAggregatorService> _logger;
        private readonly IValidator<CreateLanguageOfAggregatorDto> _createValidator;
        private readonly IValidator<UpdateLanguageOfAggregatorDto> _updateValidator;
        private readonly IWebHostEnvironment _env;

        public LanguageOfAggregatorService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<LanguageOfAggregatorService> logger,
            IValidator<CreateLanguageOfAggregatorDto> createValidator,
            IValidator<UpdateLanguageOfAggregatorDto> updateValidator,
            IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _repository = unitOfWork.LanguagesOfAggregator;
            _mapper = mapper;
            _logger = logger;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _env = env;
        }

        public async Task<IEnumerable<LanguageOfAggregatorDto>> GetAllLanguagesAsync(bool includeDisabled = true)
        {
            var languages = await _repository.GetAllAsync();
            if (!includeDisabled)
            {
                languages = languages.Where(x => x.Enabled);
            }
            return _mapper.Map<IEnumerable<LanguageOfAggregatorDto>>(languages.OrderBy(x => x.SortOrder));
        }

        public async Task<IEnumerable<LanguageOfAggregatorDto>> GetAvailableLanguagesAsync()
        {
            return await GetAllLanguagesAsync(false);
        }

        public async Task<LanguageOfAggregatorDto> GetLanguageByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new NotFoundException("LanguageOfAggregator", $"Language with ID {id} not found.");
            }
            return _mapper.Map<LanguageOfAggregatorDto>(entity);
        }

        public async Task<LanguageOfAggregatorDto> CreateLanguageAsync(CreateLanguageOfAggregatorDto request)
        {
            await _createValidator.ValidateAndThrowAsync(request);

            if (!await _repository.IsCodeUniqueAsync(request.Code))
                throw new ConflictException("Language code must be unique.", "LanguageOfAggregator", "Code");

            if (!await _repository.IsShortCodeUniqueAsync(request.ShortCode))
                throw new ConflictException("Language short code must be unique.", "LanguageOfAggregator", "ShortCode");

            var entity = _mapper.Map<LanguageOfAggregatorEntity>(request);

            if (entity.IsDefault)
            {
                await ResetOtherDefaults();
            }

            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<LanguageOfAggregatorDto>(entity);
        }

        public async Task<LanguageOfAggregatorDto> UpdateLanguageAsync(int id, UpdateLanguageOfAggregatorDto request)
        {
            await _updateValidator.ValidateAndThrowAsync(request);

            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException("LanguageOfAggregator", $"Language with ID {id} not found.");

            if (request.Code != null && request.Code != entity.Code)
            {
                if (!await _repository.IsCodeUniqueAsync(request.Code, id))
                    throw new ConflictException("Language code must be unique.", "LanguageOfAggregator", "Code");
            }

            if (request.ShortCode != null && request.ShortCode != entity.ShortCode)
            {
                if (!await _repository.IsShortCodeUniqueAsync(request.ShortCode, id))
                    throw new ConflictException("Language short code must be unique.", "LanguageOfAggregator", "ShortCode");
            }

            _mapper.Map(request, entity);

            if (request.IsDefault == true && entity.IsDefault)
            {
                await ResetOtherDefaults(id);
            }

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<LanguageOfAggregatorDto>(entity);
        }

        public async Task<bool> DeleteLanguageAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException("LanguageOfAggregator", $"Language with ID {id} not found.");

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
                throw new NotFoundException("LanguageOfAggregator", $"Language with ID {id} not found.");

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
                throw new NotFoundException("LanguageOfAggregator", $"Language with ID {id} not found.");

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
            _logger.LogWarning("Выполняется полная очистка таблицы языков агрегатора через TRUNCATE CASCADE.");
            
            // Используем TRUNCATE для быстрой очистки, сброса ID (RESTART IDENTITY) 
            // и каскадного удаления зависимостей (CASCADE)
            var sql = "TRUNCATE TABLE \"languages_of_aggregator\" RESTART IDENTITY CASCADE;";
            await _unitOfWork.ExecuteSqlRawAsync(sql);
            
            _logger.LogInformation("Таблица языков агрегатора успешно очищена, ID сброшены.");
        }

        public async Task InitializeAsync()
        {
            _logger.LogInformation("[LanguageAggregator] Начинаем инициализацию языков...");
            
            var jsonPath = Path.Combine(_env.WebRootPath, "data", "default-aggregator-languages.json");
            _logger.LogInformation("[LanguageAggregator] Попытка загрузить JSON: {Path}", jsonPath);

            // Если специфичного файла нет, пробуем использовать общий
            if (!File.Exists(jsonPath))
            {
                _logger.LogWarning("[LanguageAggregator] Специфичный файл не найден, пробуем базовый.");
                jsonPath = Path.Combine(_env.WebRootPath, "data", "default-languages.json");
            }

            if (!File.Exists(jsonPath))
            {
                _logger.LogError("[LanguageAggregator] Файл конфигурации НЕ НАЙДЕН по всем путям.");
                throw new FileNotFoundException("Файл конфигурации языков не найден.", jsonPath);
            }

            var jsonString = await File.ReadAllTextAsync(jsonPath);
            var languagesJson = JsonSerializer.Deserialize<List<JsonElement>>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (languagesJson == null || !languagesJson.Any()) 
            {
                _logger.LogWarning("[LanguageAggregator] JSON пуст или некорректен.");
                return;
            }

            _logger.LogInformation("[LanguageAggregator] Загружено объектов из JSON: {Count}", languagesJson.Count);

            var existingLanguages = await _repository.GetAllAsync();
            var existingCodes = existingLanguages.Select(x => x.Code).ToHashSet();

            foreach (var langJson in languagesJson)
            {
                var code = langJson.GetProperty("Code").GetString() ?? "";
                
                // Ключ иконки берем из JSON (поле IconKey или IconPath для совместимости)
                var iconKey = langJson.TryGetProperty("IconKey", out var ik) ? ik.GetString() : 
                              (langJson.TryGetProperty("IconPath", out var ip) ? ip.GetString() : null);

                // Добавляем язык только если его еще нет
                if (!existingCodes.Contains(code))
                {
                    _logger.LogInformation("[LanguageAggregator] Добавляю новый язык: {Code} (Icon: {IconKey})", code, iconKey);
                    var lang = new LanguageOfAggregatorEntity
                    {
                        Code = code,
                        ShortCode = langJson.GetProperty("ShortCode").GetString() ?? "",
                        Title = langJson.GetProperty("Title").GetString() ?? "",
                        NativeTitle = langJson.GetProperty("NativeTitle").GetString() ?? "",
                        Enabled = langJson.TryGetProperty("Enabled", out var e) ? e.GetBoolean() : true,
                        IsDefault = langJson.TryGetProperty("IsDefault", out var d) ? d.GetBoolean() : false,
                        IsSystem = langJson.TryGetProperty("IsSystem", out var s) ? s.GetBoolean() : false,
                        SortOrder = langJson.TryGetProperty("SortOrder", out var so) ? so.GetInt32() : 0,
                        IconKey = iconKey,
                        IsRtl = langJson.TryGetProperty("IsRtl", out var ir) ? ir.GetBoolean() : 
                                (langJson.TryGetProperty("Direction", out var dir) ? dir.GetString() == "rtl" : false)
                    };
                    await _repository.AddAsync(lang);
                }
                else
                {
                    _logger.LogInformation("[LanguageAggregator] Язык {Code} уже существует в базе, пропускаю.", code);
                }
            }
            
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("[LanguageAggregator] Инициализация завершена успешно.");
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
