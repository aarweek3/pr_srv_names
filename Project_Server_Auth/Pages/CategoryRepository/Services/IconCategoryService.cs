using DAL.Interfaces;
using DAL.Models;
using Project_Server_Auth.Pages.CategoryRepository.Dtos;
using Project_Server_Auth.Pages.CategoryRepository.Interfaces;
using Project_Server_Auth.Pages.CategoryRepository.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using DAL.Models.GeneralModels;

namespace Project_Server_Auth.Pages.CategoryRepository.Services
{
    public class IconCategoryService : IIconCategoryService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<IconCategoryService> _logger;
        private readonly IConfiguration _config;

        private string FrontendProjectPath => _config["FrontendProjectPath"] ?? @"d:\_PROGECT\pr_aurora_admin";

        public IconCategoryService(
            IUnitOfWork uow, 
            IConfiguration config,
            ILogger<IconCategoryService> logger)
        {
            _uow = uow;
            _config = config;
            _logger = logger;
        }

        public async Task<IEnumerable<IconCategoryDto>> GetAllAsync()
        {
            var query = _uow.IconCategories.GetQueryable();
            
            return await query
                .OrderBy(c => c.Name)
                .Select(c => new IconCategoryDto
                {
                    Id = c.Id,
                    FolderName = c.FolderName,
                    DisplayName = c.Name,
                    IsSystem = c.IsSystem,
                    MenuIcon = c.MenuIcon,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    IconCount = c.Icons.Count()
                })
                .ToListAsync();
        }

        public async Task<IconCategoryDto?> GetByIdAsync(int id)
        {
            var category = await _uow.IconCategories.GetByIdAsync(id);
            return category?.ToDto();
        }

        public async Task<IconCategoryDto> CreateAsync(IconCategoryCreateDto createDto)
        {
            var entity = createDto.ToEntity();
            await _uow.IconCategories.AddAsync(entity);
            await _uow.SaveChangesAsync();
            return entity.ToDto();
        }

        public async Task<IconCategoryDto?> UpdateAsync(int id, IconCategoryUpdateDto updateDto)
        {
            var entity = await _uow.IconCategories.GetByIdAsync(id);
            if (entity == null) return null;

            updateDto.UpdateEntity(entity);
            _uow.IconCategories.Update(entity);
            await _uow.SaveChangesAsync();

            return entity.ToDto();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _uow.IconCategories.GetByIdAsync(id);
            if (entity == null) return false;
            
            // Проверка на наличие иконок в категории перед удалением
            // (Если хотим запретить удаление непустых категорий)
            // var hasIcons = await _uow.Icons.AnyAsync(i => i.CategoryId == id);
            // if (hasIcons) throw new InvalidOperationException("Category is not empty.");

            _uow.IconCategories.Delete(entity);
            return await _uow.SaveChangesAsync() > 0;
        }

        // Метод Sync для синхронизации файловой системы с БД
        public async Task<IconSyncResultDto> SyncWithFileSystemAsync()
        {
            var result = new IconSyncResultDto();
            try
            {
                var iconsRootPath = Path.Combine(FrontendProjectPath, "src", "assets", "icons");
                result.Logs.Add($"🔄 Запуск синхронизации. Корень: {iconsRootPath}");
                _logger.LogInformation("[IconCategoryService] 🔄 Sync started. Root: {Path}", iconsRootPath);

                if (!Directory.Exists(iconsRootPath))
                {
                    var msg = $"❌ Директория иконок не найдена: {iconsRootPath}";
                    result.Logs.Add(msg);
                    result.Message = msg;
                    result.Success = false;
                    _logger.LogError("[IconCategoryService] {Message}", msg);
                    return result;
                }

                var subDirs = Directory.GetDirectories(iconsRootPath);
                
                foreach (var dirPath in subDirs)
                {
                    var folderName = Path.GetFileName(dirPath);
                    result.Logs.Add($"📂 Обработка папки категории: {folderName}");
                    _logger.LogInformation("[IconCategoryService] 📂 Processing category folder: {Folder}", folderName);

                    // 1. Find or create Category
                    var category = await _uow.IconCategories.GetFirstOrDefaultTrackingAsync(c => c.FolderName == folderName || c.Name == folderName);
                    if (category == null)
                    {
                        result.Logs.Add($"✨ Создание новой категории в БД: {folderName}");
                        category = new IconCategory
                        {
                            Name = folderName,
                            FolderName = folderName,
                            IsSystem = true
                        };
                        await _uow.IconCategories.AddAsync(category);
                        await _uow.SaveChangesAsync(); // Save to get the ID for icons
                    }

                    // 2. Scan SVG files
                    var svgFiles = Directory.GetFiles(dirPath, "*.svg");
                    result.Logs.Add($"🔍 Найдено {svgFiles.Length} SVG файлов в {folderName}");

                    foreach (var filePath in svgFiles)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(filePath);
                        var svgContent = await File.ReadAllTextAsync(filePath);

                        // 3. Find or create Icon
                        var icon = await _uow.Icons.GetFirstOrDefaultTrackingAsync(i => i.Name == fileName);
                        if (icon == null)
                        {
                            result.Logs.Add($"  ➕ Добавление иконки: {fileName}");
                            icon = new DAL.Models.GeneralModels.Icon
                            {
                                Name = fileName,
                                SvgContent = svgContent,
                                CategoryId = category.Id
                            };
                            await _uow.Icons.AddAsync(icon);
                        }
                        else
                        {
                            result.Logs.Add($"  🔄 Обновление контента иконки: {fileName}");
                            icon.SvgContent = svgContent;
                            icon.CategoryId = category.Id;
                            _uow.Icons.Update(icon);
                        }
                        result.IconsProcessed++;
                    }
                    result.CategoriesProcessed++;
                }

                await _uow.SaveChangesAsync();
                result.Success = true;
                result.Message = "Синхронизация успешно завершена";
                result.Logs.Add($"✅ Синхронизация завершена. Категорий: {result.CategoriesProcessed}, Иконок: {result.IconsProcessed}");
                _logger.LogInformation("[IconCategoryService] ✅ Sync completed. Categories: {Cats}, Icons: {Icons}", result.CategoriesProcessed, result.IconsProcessed);
                return result;
            }
            catch (Exception ex)
            {
                var msg = $"❌ Ошибка при синхронизации: {ex.Message}";
                result.Logs.Add(msg);
                result.Message = msg;
                result.Success = false;
                _logger.LogError(ex, "[IconCategoryService] ❌ Sync failed");
                return result;
            }
        }

        public Task<IconCategoryDto?> GetByFolderNameAsync(string folderName)
        {
            throw new NotImplementedException();
        }
    }
}
