using pr_srv_names.Pages.Icons.Interfaces;
using pr_srv_names.Pages.Icons.Models;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System;
using Project_Server_Auth.Pages.CategoryRepository.Interfaces;
using DAL.Interfaces;
using DbModels = DAL.Models.GeneralModels;
using Microsoft.EntityFrameworkCore;

namespace pr_srv_names.Pages.Icons.Services
{
    public class IconService : IIconService
    {
        private readonly ILogger<IconService> _logger;
        private readonly IIconCategoryService _categoryService;
        private readonly IUnitOfWork _uow;

        private static readonly string _registryFilePath = @"d:\_PROGECT\pr_aurora_admin\src\app\pages\ui-demo\old-control\icon-ui\icon-registry.ts";

        // Static cache for the metadata registry
        private static List<IconCategoryPage>? _cachedRegistry = null;
        private static readonly object _cacheLock = new object();

        public IconService(
            IIconCategoryService categoryService,
            IUnitOfWork uow,
            ILogger<IconService> logger)
        {
            _categoryService = categoryService;
            _uow = uow;
            _logger = logger;
        }


        // ==========================================
        // PUBLIC METHODS (DB BASED)
        // ==========================================
        
        /// <summary>
        /// Helper to rebuild the registry DTOs from Database data
        /// </summary>
        private async Task<List<IconCategoryPage>> RefreshRegistryFromDbAsync()
        {
            var registry = new List<IconCategoryPage>();
            var categories = await _uow.IconCategories.GetAllAsync();
            var allIcons = await _uow.Icons.GetQueryable()
                .Select(i => new { i.Name, i.CategoryId })
                .ToListAsync();

            var iconsByCategory = allIcons.GroupBy(i => i.CategoryId).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var cat in categories)
            {
                var catModel = new IconCategoryPage { Category = cat.Name, Icons = new List<IconMetadata>() };
                
                if (iconsByCategory.TryGetValue(cat.Id, out var icons))
                {
                    foreach (var icon in icons)
                    {
                        catModel.Icons.Add(new IconMetadata 
                        { 
                            Name = icon.Name,
                            Category = cat.Name,
                            Type = $"{cat.Name}/{icon.Name}"
                        });
                    }
                }
                
                registry.Add(catModel);
            }
            return registry;
        }

        public async Task<List<IconCategoryPage>> GetIconsAsync(bool includeSvgContent = false)
        {
            if (includeSvgContent)
            {
                // Batch load with SVG content (not cached due to size)
                return await GetIconsWithContentAsync();
            }
            
            // Standard load (metadata only) - USING CACHE
            if (_cachedRegistry == null)
            {
                var fresh = await RefreshRegistryFromDbAsync();
                lock (_cacheLock)
                {
                    _cachedRegistry ??= fresh;
                }
            }
            return _cachedRegistry;
        }

        /// <summary>
        /// Loads all icons with their SVG content for batch loading
        /// </summary>
        private async Task<List<IconCategoryPage>> GetIconsWithContentAsync()
        {
            var result = new List<IconCategoryPage>();
            var categories = await _uow.IconCategories.GetAllAsync();
            var allIcons = await _uow.Icons.GetQueryable()
                .Select(i => new { i.Name, i.CategoryId, i.SvgContent })
                .ToListAsync();

            var iconsByCategory = allIcons.GroupBy(i => i.CategoryId).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var cat in categories)
            {
                var catModel = new IconCategoryPage { Category = cat.Name, Icons = new List<IconMetadata>() };
                
                if (iconsByCategory.TryGetValue(cat.Id, out var icons))
                {
                    foreach (var icon in icons)
                    {
                        catModel.Icons.Add(new IconMetadata 
                        { 
                            Name = icon.Name,
                            Category = cat.Name,
                            Type = $"{cat.Name}/{icon.Name}",
                            SvgContent = icon.SvgContent
                        });
                    }
                }
                
                result.Add(catModel);
            }
            
            return result;
        }

        public async Task<List<IconMetadata>> GetAllIconsFlatAsync()
        {
             var registry = await GetIconsAsync();
             return registry.SelectMany(c => c.Icons).ToList();
        }

        public async Task<int> GetTotalIconsCountAsync()
        {
             return await _uow.Icons.CountAsync();
        }
        
        public async Task<string?> GetContentByNameAsync(string name)
        {
            return await _uow.Icons.GetContentByNameAsync(name);
        }

        public async Task<List<IconMetadata>> GetIconsByCategoryAsync(int categoryId)
        {
            var category = await _uow.IconCategories.GetByIdAsync(categoryId);
            if (category == null) return new List<IconMetadata>();

            var icons = await _uow.Icons
                 .GetQueryable()
                 .Where(i => i.CategoryId == categoryId)
                 .Select(i => new { i.Name, i.SvgContent })
                 .ToListAsync();

            return icons.Select(icon => new IconMetadata
            {
                Name = icon.Name,
                Category = category.Name,
                Type = $"{category.Name}/{icon.Name}",
                SvgContent = icon.SvgContent
            }).ToList();
        }

        public async Task<Dictionary<string, string>> GetIconsContentBatchAsync(List<string> itemNames)
        {
            if (itemNames == null || !itemNames.Any()) return new Dictionary<string, string>();

            var icons = await _uow.Icons
                .GetQueryable()
                .Where(i => itemNames.Contains(i.Name))
                .Select(i => new { i.Name, i.SvgContent })
                .ToListAsync();

            return icons
                .Where(i => !string.IsNullOrEmpty(i.SvgContent))
                .ToDictionary(i => i.Name, i => i.SvgContent!);
        }

        public async Task UpdateIconContentAsync(string iconType, string svgContent, bool toBackend = true, bool toFrontend = true)
        {
            _logger.LogInformation("[IconService] 📥 UpdateIconContentAsync called for: {IconType}", iconType);

            // iconType can be "category/name" or just "name"
            var parts = iconType.Split('/');
            string name = parts.Last(); 
            string catNameFromType = parts.Length > 1 ? parts[0] : "general";

            // Search by Name
            var icon = await _uow.Icons.GetFirstOrDefaultTrackingAsync(i => i.Name == name);
            if (icon != null)
            {
                _logger.LogInformation("[IconService] 🔄 Icon '{Name}' found. Updating content.", name);
                icon.SvgContent = svgContent;
                // Category changes are handled via MoveIconAsync
                await _uow.SaveChangesAsync();
                _cachedRegistry = null; // Invalidate cache
            }
            else
            {
                // Create new
                _logger.LogInformation("[IconService] ✨ Creating NEW icon '{Name}' in category '{Category}'", name, catNameFromType);
                
                var category = await _uow.IconCategories.GetFirstOrDefaultAsync(c => c.Name == catNameFromType || c.FolderName == catNameFromType);
                
                if (category == null) 
                {
                     _logger.LogError("[IconService] ❌ Category '{Category}' not found!", catNameFromType);
                     throw new InvalidOperationException($"Category '{catNameFromType}' not found.");
                }

                _logger.LogInformation("[IconService] ✅ Category resolved: ID={Id}, Name={Name}, Folder={Folder}", category.Id, category.Name, category.FolderName);

                var newIcon = new DbModels.Icon 
                { 
                    Name = name, 
                    SvgContent = svgContent, 
                    CategoryId = category.Id 
                };
                await _uow.Icons.AddAsync(newIcon);
                await _uow.SaveChangesAsync();
                _cachedRegistry = null; // Invalidate cache
                _logger.LogInformation("[IconService] 💾 Icon saved to DB with ID: {Id}", newIcon.Id);
            }
        }
        
        public async Task UpdateIconsBatchAsync(List<UpdateIconRequest> requests)
        {
            foreach (var req in requests)
            {
                await UpdateIconContentAsync(req.IconType, req.SvgContent);
            }
        }

        public async Task MoveIconAsync(string iconType, int targetCategoryId)
        {
            _logger.LogInformation("[IconService] 🔄 MoveIconAsync started. Type: {IconType}, TargetCategoryId: {TargetId}", iconType, targetCategoryId);

            var parts = iconType.Split('/');
            string name = parts.Last();
            
            var targetCat = await _uow.IconCategories.GetByIdAsync(targetCategoryId);
            if (targetCat == null)
            {
                _logger.LogError("[IconService] ❌ Target category with ID {Id} not found.", targetCategoryId);
                throw new ArgumentException($"Target category with ID '{targetCategoryId}' not found.");
            }

            var icon = await _uow.Icons.GetFirstOrDefaultTrackingAsync(i => i.Name == name);
            if (icon == null)
            {
                _logger.LogError("[IconService] ❌ Icon '{Name}' not found in DB.", name);
                throw new ArgumentException($"Icon '{name}' not found.");
            }

            _logger.LogInformation("[IconService] 📦 Found icon '{Name}'. Moving from CategoryId {OldId} to {NewId}", 
                icon.Name, icon.CategoryId, targetCat.Id);

            icon.CategoryId = targetCat.Id;
            await _uow.SaveChangesAsync();
            _cachedRegistry = null; // Invalidate cache

            _logger.LogInformation("[IconService] ✅ Icon '{Name}' successfully moved to '{CatName}'", icon.Name, targetCat.Name);
        }

        public async Task RenameIconAsync(string oldName, string newName)
        {
            _logger.LogInformation("[IconService] 🔄 RenameIconAsync started. OldName: {OldName}, NewName: {NewName}", oldName, newName);

            // 1. Валидация формата
            if (!System.Text.RegularExpressions.Regex.IsMatch(newName, @"^[a-z0-9_]+$"))
            {
                _logger.LogWarning("[IconService] ⚠️ Invalid name format: {NewName}", newName);
                throw new ArgumentException("Имя может содержать только латинские буквы (a-z), цифры (0-9) и подчеркивание (_)");
            }

            // 2. Валидация длины
            if (newName.Length > 500)
            {
                _logger.LogWarning("[IconService] ⚠️ Name too long: {Length} characters", newName.Length);
                throw new ArgumentException("Имя не может быть длиннее 500 символов");
            }

            if (string.IsNullOrWhiteSpace(newName))
            {
                _logger.LogWarning("[IconService] ⚠️ Empty name provided");
                throw new ArgumentException("Имя не может быть пустым");
            }

            // 3. Проверка уникальности
            var existingIcon = await _uow.Icons.GetFirstOrDefaultAsync(i => i.Name == newName);
            if (existingIcon != null)
            {
                _logger.LogWarning("[IconService] ⚠️ Icon with name '{NewName}' already exists", newName);
                throw new InvalidOperationException($"Иконка с именем '{newName}' уже существует");
            }

            // 4. Найти иконку для переименования
            var icon = await _uow.Icons.GetFirstOrDefaultTrackingAsync(i => i.Name == oldName);
            if (icon == null)
            {
                _logger.LogError("[IconService] ❌ Icon '{OldName}' not found in DB", oldName);
                throw new FileNotFoundException($"Иконка '{oldName}' не найдена");
            }

            // 5. Получить категорию для построения пути
            var category = await _uow.IconCategories.GetByIdAsync(icon.CategoryId);
            if (category == null)
            {
                _logger.LogError("[IconService] ❌ Category with ID {CategoryId} not found", icon.CategoryId);
                throw new InvalidOperationException($"Категория с ID {icon.CategoryId} не найдена");
            }

            _logger.LogInformation("[IconService] 📦 Found icon '{OldName}' in category '{CategoryName}'", oldName, category.Name);

            // 6. Переименовать в БД
            icon.Name = newName;
            await _uow.SaveChangesAsync();
            _cachedRegistry = null; // Invalidate cache

            _logger.LogInformation("[IconService] ✅ Icon successfully renamed in DB: {OldName} → {NewName}", oldName, newName);
        }

        public async Task DeleteIconAsync(string iconType, bool fromBackend, bool fromFrontend)
        {
             var parts = iconType.Split('/');
             string name = parts.Last();

             var icon = await _uow.Icons.GetFirstOrDefaultAsync(i => i.Name == name);
             if (icon != null)
             {
                 _uow.Icons.Delete(icon);
                 await _uow.SaveChangesAsync();
                 _cachedRegistry = null; // Invalidate cache
             }
        }


        public async Task SyncToFrontendAsync(string filePath)
        {
            // 1. Fetch all icons from DB
            var dbCategories = await GetIconsWithContentAsync();
            
            // 2. Generate icon-registry.ts based on DB data
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("// src/app/pages/ui-demo/icon-ui/icon-registry.ts");
            sb.AppendLine("// AUTO-GENERATED by BACKEND (DB-DRIVEN)");
            sb.AppendLine();
            sb.AppendLine("import { IconMetadata } from './icon-metadata.model';");
            sb.AppendLine();
            sb.AppendLine("export interface IconCategory {");
            sb.AppendLine("  category: string;");
            sb.AppendLine("  icons: IconMetadata[];");
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine("export const ICON_REGISTRY: IconCategory[] = [");

            for (int i = 0; i < dbCategories.Count; i++)
            {
                var cat = dbCategories[i];
                sb.AppendLine("  {");
                sb.AppendLine($"    \"category\": \"{cat.Category}\",");
                sb.AppendLine("    \"icons\": [");

                for (int j = 0; j < cat.Icons.Count; j++)
                {
                    var icon = cat.Icons[j];
                    sb.AppendLine("      {");
                    sb.AppendLine($"        \"name\": \"{icon.Name}\",");
                    sb.AppendLine($"        \"type\": \"{icon.Type}\","); 
                    sb.AppendLine($"        \"category\": \"{icon.Category}\"");
                    sb.Append("      }");
                    if (j < cat.Icons.Count - 1) sb.Append(",");
                    sb.AppendLine();
                }

                sb.AppendLine("    ]");
                sb.Append("  }");
                if (i < dbCategories.Count - 1) sb.Append(",");
                sb.AppendLine();
            }

            sb.AppendLine("];");

            await File.WriteAllTextAsync(_registryFilePath, sb.ToString(), System.Text.Encoding.UTF8);
        }

        // ==========================================
        // LEGACY / NOT IMPLEMENTED (Clean up later)
        // ==========================================
        public async Task<List<IconSyncStatus>> GetSyncStatusAsync()
        {
            // Since we are DB-based, backend always exists. Frontend sync is checked via internal logic if needed.
            // For now return empty or simple status based on DB presence.
            return new List<IconSyncStatus>(); 
        }

        private static readonly string _baseIconsPath = @"d:\_PROGECT\pr_srv_names";

        public async Task<List<FileSystemItem>> BrowseFileSystemAsync(string subPath)
        {
            var result = new List<FileSystemItem>();
            
            // Если путь пустой, показываем список дисков
            if (string.IsNullOrWhiteSpace(subPath))
            {
                foreach (var drive in DriveInfo.GetDrives().Where(d => d.IsReady))
                {
                    result.Add(new FileSystemItem
                    {
                        Name = drive.Name,
                        Path = drive.Name,
                        Type = "folder"
                    });
                }
                return result;
            }

            var fullPath = subPath;

            if (!Directory.Exists(fullPath))
            {
                _logger.LogWarning("[IconService] ⚠️ Directory not found: {Path}", fullPath);
                return result;
            }

            var directoryInfo = new DirectoryInfo(fullPath);

            // Добавляем папки
            foreach (var dir in directoryInfo.GetDirectories())
            {
                result.Add(new FileSystemItem
                {
                    Name = dir.Name,
                    Path = dir.FullName,
                    Type = "folder"
                });
            }

            // Добавляем SVG файлы
            foreach (var file in directoryInfo.GetFiles("*.svg"))
            {
                result.Add(new FileSystemItem
                {
                    Name = file.Name,
                    Path = file.FullName,
                    Type = "file",
                    Extension = file.Extension,
                    Size = file.Length
                });
            }

            return result;
        }

        public async Task<BulkRenameResult> BulkRenameAsync(BulkRenameRequest request)
        {
            _logger.LogInformation("[IconService] 🔄 BulkRename (Copy/Rename) started. Source: {Source}, Target: {Target}, Prefix: {Prefix}", 
                request.SourceFolder, request.TargetFolder, request.Prefix);

            var result = new BulkRenameResult();

            // 1. Проверка путей (теперь поддерживаем любые абсолютные пути)
            if (string.IsNullOrWhiteSpace(request.SourceFolder) || string.IsNullOrWhiteSpace(request.TargetFolder))
            {
                throw new ArgumentException("Исходная и целевая папки должны быть указаны");
            }

            var sourcePath = request.SourceFolder;
            var targetPath = request.TargetFolder;

            // 1.1. Проверка на идентичность путей (папка не может быть сама себе целью)
            if (string.Equals(sourcePath?.TrimEnd('\\', '/'), targetPath?.TrimEnd('\\', '/'), StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Исходная и целевая папки не могут быть одинаковыми");
            }

            if (!Directory.Exists(sourcePath))
            {
                throw new DirectoryNotFoundException($"Исходная папка '{request.SourceFolder}' не найдена");
            }

            // 2. Создание целевой папки
            if (!Directory.Exists(targetPath))
            {
                _logger.LogInformation("[IconService] ✨ Creating physical target directory: {Path}", targetPath);
                Directory.CreateDirectory(targetPath);
            }

            // 3. Список файлов (если не переданы конкретные, берем все SVG из папки)
            var filesToProcess = request.FileNames != null && request.FileNames.Any() 
                ? request.FileNames 
                : Directory.GetFiles(sourcePath, "*.svg").Select(Path.GetFileName).ToList();

            _logger.LogInformation("[IconService] 📂 Found {Count} SVG files to process in {Path}", filesToProcess.Count, sourcePath);
            result.TotalFiles = filesToProcess.Count;

            if (filesToProcess.Count == 0)
            {
                _logger.LogWarning("[IconService] ⚠️ No SVG files found in source folder: {Path}", sourcePath);
            }

            // 4. Копирование с префиксом
            foreach (var fileName in filesToProcess)
            {
                if (string.IsNullOrEmpty(fileName)) continue;

                var operation = new RenameOperation { OriginalName = fileName };
                try
                {
                    var sourceFile = Path.Combine(sourcePath, fileName);
                    var newFileName = $"{request.Prefix}{fileName}";
                    var targetFile = Path.Combine(targetPath, newFileName);
                    
                    operation.NewName = newFileName;

                    if (!File.Exists(sourceFile))
                    {
                        operation.Success = false;
                        operation.ErrorMessage = "Файл внезапно исчез";
                        result.FailedCount++;
                        _logger.LogWarning("[IconService] ❌ Source file not found: {Path}", sourceFile);
                    }
                    else
                    {
                        _logger.LogDebug("[IconService] 📑 Copying: {Src} -> {Dst}", sourceFile, targetFile);
                        File.Copy(sourceFile, targetFile, request.OverwriteExisting);
                        operation.Success = true;
                        result.SuccessCount++;
                    }
                }
                catch (Exception ex)
                {
                    operation.Success = false;
                    operation.ErrorMessage = ex.Message;
                    result.FailedCount++;
                    _logger.LogError(ex, "[IconService] ❌ Error copying/renaming {File}", fileName);
                }
                result.Operations.Add(operation);
            }

            return result;
        }

        public async Task<List<IconRenameResult>> RefactorIconNamesAsync(int? categoryId = null)
        {
            _logger.LogInformation("[IconService] 🪄 RefactorIconNamesAsync started. CategoryId: {Id}", categoryId);
            var results = new List<IconRenameResult>();

            // 1. Fetch icons to process (using tracking as we will update names)
            var query = _uow.Icons.GetQueryable();
            if (categoryId.HasValue)
            {
                query = query.Where(i => i.CategoryId == categoryId.Value);
            }

            var icons = await query.ToListAsync();
            _logger.LogInformation("[IconService] 🔍 Found {Count} icons to refactor", icons.Count);

            foreach (var icon in icons)
            {
                var oldName = icon.Name;
                
                // 2. Logic: lowercase, replace hyphens, collapse multiple underscores
                var processedName = oldName.ToLowerInvariant().Replace("-", "_");
                
                // Standardize prefix: remove existing variations to prevent "av_av_..."
                if (processedName.StartsWith("av_")) processedName = processedName.Substring(3);
                else if (processedName.StartsWith("_av_")) processedName = processedName.Substring(4);
                else if (processedName.StartsWith("av-")) processedName = processedName.Substring(3);
                else if (processedName.StartsWith("_")) processedName = processedName.TrimStart('_');

                var newName = "av_" + processedName;

                if (oldName == newName) continue;

                var result = new IconRenameResult { OldName = oldName, NewName = newName, Success = true };

                try
                {
                    // 3. Check for conflict
                    var conflict = await _uow.Icons.GetFirstOrDefaultAsync(i => i.Name == newName && i.Id != icon.Id);
                    if (conflict != null)
                    {
                        result.Success = false;
                        result.Message = $"Conflict with existing icon ID {conflict.Id}";
                        results.Add(result);
                        continue;
                    }

                    // 4. Update via repository (assuming tracking is on or using a manual update)
                    icon.Name = newName;
                    await _uow.SaveChangesAsync();
                    results.Add(result);
                    _logger.LogInformation("[IconService] 🏷️ Refactored: {Old} -> {New}", oldName, newName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[IconService] ❌ Failed to refactor icon {Name}", oldName);
                    result.Success = false;
                    result.Message = ex.Message;
                    results.Add(result);
                }
            }

            // Sync to local registry after mass rename
            if (results.Any(r => r.Success))
            {
                await SyncToFrontendAsync("");
            }

            return results;
        }
    }
}
