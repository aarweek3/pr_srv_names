using pr_srv_names.Pages.Icons.Interfaces;
using pr_srv_names.Pages.Icons.Models;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System;
using DAL.Interfaces;
using DbModels = DAL.Models.GeneralModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;


namespace pr_srv_names.Pages.Icons.Services
{
    public class IconLaboratoryService : IIconLaboratoryService
    {
        private readonly ILogger<IconLaboratoryService> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _config;
        
        private string FrontendProjectPath => _config["FrontendProjectPath"] ?? @"d:\_PROGECT\pr_aurora_admin";

        public IconLaboratoryService(
            IUnitOfWork uow,
            IConfiguration config,
            ILogger<IconLaboratoryService> logger)
        {
            _uow = uow;
            _config = config;
            _logger = logger;
        }

        private void InvalidateCache()
        {
            IconGetService.InvalidateCache();
        }

        // ==========================================
        // MANAGEMENT METHODS
        // ==========================================

        public async Task UpdateIconContentAsync(string iconType, string svgContent, bool toBackend = true, bool toFrontend = true)
        {
            _logger.LogInformation("[IconLaboratoryService] 📥 UpdateIconContentAsync called for: {IconType}", iconType);

            // iconType can be "category/name" or just "name"
            var parts = iconType.Split('/');
            string name = parts.Last(); 
            string catNameFromType = parts.Length > 1 ? parts[0] : "general";

            // Search by Name
            var icon = await _uow.Icons.GetFirstOrDefaultTrackingAsync(i => i.Name == name);
            if (icon != null)
            {
                _logger.LogInformation("[IconLaboratoryService] 🔄 Icon '{Name}' found. Updating content.", name);
                icon.SvgContent = svgContent;
                // Category changes are handled via MoveIconAsync
                await _uow.SaveChangesAsync();
                InvalidateCache(); 
            }
            else
            {
                // Create new
                _logger.LogInformation("[IconLaboratoryService] ✨ Creating NEW icon '{Name}' in category '{Category}'", name, catNameFromType);
                
                var category = await _uow.IconCategories.GetFirstOrDefaultAsync(c => c.Name == catNameFromType || c.FolderName == catNameFromType);
                
                if (category == null) 
                {
                     _logger.LogError("[IconLaboratoryService] ❌ Category '{Category}' not found!", catNameFromType);
                     throw new InvalidOperationException($"Category '{catNameFromType}' not found.");
                }

                _logger.LogInformation("[IconLaboratoryService] ✅ Category resolved: ID={Id}, Name={Name}, Folder={Folder}", category.Id, category.Name, category.FolderName);

                var newIcon = new DbModels.Icon 
                { 
                    Name = name, 
                    SvgContent = svgContent, 
                    CategoryId = category.Id 
                };
                await _uow.Icons.AddAsync(newIcon);
                await _uow.SaveChangesAsync();
                InvalidateCache(); 
                _logger.LogInformation("[IconLaboratoryService] 💾 Icon saved to DB with ID: {Id}", newIcon.Id);
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
            _logger.LogInformation("[IconLaboratoryService] 🔄 MoveIconAsync started. Type: {IconType}, TargetCategoryId: {TargetId}", iconType, targetCategoryId);

            var parts = iconType.Split('/');
            string name = parts.Last();
            
            var targetCat = await _uow.IconCategories.GetByIdAsync(targetCategoryId);
            if (targetCat == null)
            {
                _logger.LogError("[IconLaboratoryService] ❌ Target category with ID {Id} not found.", targetCategoryId);
                throw new ArgumentException($"Target category with ID '{targetCategoryId}' not found.");
            }

            var icon = await _uow.Icons.GetFirstOrDefaultTrackingAsync(i => i.Name == name);
            if (icon == null)
            {
                _logger.LogError("[IconLaboratoryService] ❌ Icon '{Name}' not found in DB.", name);
                throw new ArgumentException($"Icon '{name}' not found.");
            }

            _logger.LogInformation("[IconLaboratoryService] 📦 Found icon '{Name}'. Moving from CategoryId {OldId} to {NewId}", 
                icon.Name, icon.CategoryId, targetCat.Id);

            icon.CategoryId = targetCat.Id;
            await _uow.SaveChangesAsync();
            InvalidateCache(); 

            _logger.LogInformation("[IconLaboratoryService] ✅ Icon '{Name}' successfully moved to '{CatName}'", icon.Name, targetCat.Name);
        }

        public async Task RenameIconAsync(string oldName, string newName)
        {
            _logger.LogInformation("[IconLaboratoryService] 🔄 RenameIconAsync started. OldName: {OldName}, NewName: {NewName}", oldName, newName);

            // 1. Validation
            if (!System.Text.RegularExpressions.Regex.IsMatch(newName, @"^[a-z0-9_]+$"))
            {
                _logger.LogWarning("[IconLaboratoryService] ⚠️ Invalid name format: {NewName}", newName);
                throw new ArgumentException("Имя может содержать только латинские буквы (a-z), цифры (0-9) и подчеркивание (_)");
            }

            if (newName.Length > 500)
            {
                _logger.LogWarning("[IconLaboratoryService] ⚠️ Name too long: {Length} characters", newName.Length);
                throw new ArgumentException("Имя не может быть длиннее 500 символов");
            }

            if (string.IsNullOrWhiteSpace(newName))
            {
                _logger.LogWarning("[IconLaboratoryService] ⚠️ Empty name provided");
                throw new ArgumentException("Имя не может быть пустым");
            }

            // 3. Uniqueness check
            var existingIcon = await _uow.Icons.GetFirstOrDefaultAsync(i => i.Name == newName);
            if (existingIcon != null)
            {
                _logger.LogWarning("[IconLaboratoryService] ⚠️ Icon with name '{NewName}' already exists", newName);
                throw new InvalidOperationException($"Иконка с именем '{newName}' уже существует");
            }

            // 4. Find icon
            var icon = await _uow.Icons.GetFirstOrDefaultTrackingAsync(i => i.Name == oldName);
            if (icon == null)
            {
                _logger.LogError("[IconLaboratoryService] ❌ Icon '{OldName}' not found in DB", oldName);
                throw new FileNotFoundException($"Иконка '{oldName}' не найдена");
            }

            // 5. Get category for debug
            var category = await _uow.IconCategories.GetByIdAsync(icon.CategoryId);
            
            _logger.LogInformation("[IconLaboratoryService] 📦 Found icon '{OldName}' in category '{CategoryName}'", oldName, category?.Name);

            // 6. Rename in DB
            icon.Name = newName;
            await _uow.SaveChangesAsync();
            InvalidateCache(); 

            _logger.LogInformation("[IconLaboratoryService] ✅ Icon successfully renamed in DB: {OldName} → {NewName}", oldName, newName);
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
                 InvalidateCache(); 
             }
        }

        public async Task SyncToFrontendAsync(string filePath)
        {
            // Note: Reuse IIconGetService here to avoid duplicating retrieval logic? 
            // Better to re-implement simpler logic or just use UOW directly to avoid circular dependency.
            
            // 1. Fetch all icons from DB
             var result = new List<IconCategoryPage>();
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
                        // Note: SvgContent is NOT needed for registry file
                        catModel.Icons.Add(new IconMetadata 
                        { 
                            Name = icon.Name,
                            Category = cat.Name,
                            Type = $"{cat.Name}/{icon.Name}",
                        });
                    }
                }
                result.Add(catModel);
            }

            var dbCategories = result;
            
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

            var dynamicRegistryPath = Path.Combine(FrontendProjectPath, "src", "app", "pages", "ui-demo", "old-control", "icon-ui", "icon-registry.ts");
            await File.WriteAllTextAsync(dynamicRegistryPath, sb.ToString(), System.Text.Encoding.UTF8);
        }

        public async Task<List<IconSyncStatus>> GetSyncStatusAsync()
        {
            // For now return empty or simple status based on DB presence.
            return new List<IconSyncStatus>(); 
        }

        public async Task<List<FileSystemItem>> BrowseFileSystemAsync(string subPath)
        {
            var result = new List<FileSystemItem>();
            
            if (string.IsNullOrWhiteSpace(subPath))
            {
                foreach (var drive in DriveInfo.GetDrives().Where(d => d.IsReady))
                {
                    result.Add(new FileSystemItem
                    {
                        Name = drive.Name,
                        Path = drive.Name,
                        Type = "drive"
                    });
                }
                return result;
            }

            var fullPath = subPath;

            if (!Directory.Exists(fullPath))
            {
                _logger.LogWarning("[IconLaboratoryService] ⚠️ Directory not found: {Path}", fullPath);
                return result;
            }

            var directoryInfo = new DirectoryInfo(fullPath);

            foreach (var dir in directoryInfo.GetDirectories())
            {
                result.Add(new FileSystemItem
                {
                    Name = dir.Name,
                    Path = dir.FullName,
                    Type = "folder"
                });
            }

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
            _logger.LogInformation("[IconLaboratoryService] 🔄 BulkRename (Copy/Rename) started. Source: {Source}, Target: {Target}, Prefix: {Prefix}", 
                request.SourceFolder, request.TargetFolder, request.Prefix);

            var result = new BulkRenameResult();

            if (string.IsNullOrWhiteSpace(request.SourceFolder) || string.IsNullOrWhiteSpace(request.TargetFolder))
            {
                throw new ArgumentException("Исходная и целевая папки должны быть указаны");
            }

            var sourcePath = request.SourceFolder;
            var targetPath = request.TargetFolder;

            if (string.Equals(sourcePath?.TrimEnd('\\', '/'), targetPath?.TrimEnd('\\', '/'), StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Исходная и целевая папки не могут быть одинаковыми");
            }

            if (!Directory.Exists(sourcePath))
            {
                throw new DirectoryNotFoundException($"Исходная папка '{request.SourceFolder}' не найдена");
            }

            if (!Directory.Exists(targetPath))
            {
                _logger.LogInformation("[IconLaboratoryService] ✨ Creating physical target directory: {Path}", targetPath);
                Directory.CreateDirectory(targetPath);
            }

            var filesToProcess = request.FileNames != null && request.FileNames.Any() 
                ? request.FileNames 
                : Directory.GetFiles(sourcePath, "*.svg").Select(Path.GetFileName).ToList();

            _logger.LogInformation("[IconLaboratoryService] 📂 Found {Count} SVG files to process in {Path}", filesToProcess.Count, sourcePath);
            result.TotalFiles = filesToProcess.Count;

            if (filesToProcess.Count == 0)
            {
                _logger.LogWarning("[IconLaboratoryService] ⚠️ No SVG files found in source folder: {Path}", sourcePath);
            }

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
                        _logger.LogWarning("[IconLaboratoryService] ❌ Source file not found: {Path}", sourceFile);
                    }
                    else
                    {
                        _logger.LogDebug("[IconLaboratoryService] 📑 Copying: {Src} -> {Dst}", sourceFile, targetFile);
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
                    _logger.LogError(ex, "[IconLaboratoryService] ❌ Error copying/renaming {File}", fileName);
                }
                result.Operations.Add(operation);
            }

            return result;
        }

        public async Task<List<IconRenameResult>> RefactorIconNamesAsync(int? categoryId = null)
        {
            _logger.LogInformation("[IconLaboratoryService] 🪄 RefactorIconNamesAsync started. CategoryId: {Id}", categoryId);
            var results = new List<IconRenameResult>();

            var query = _uow.Icons.GetQueryable();
            if (categoryId.HasValue)
            {
                query = query.Where(i => i.CategoryId == categoryId.Value);
            }

            var icons = await query.ToListAsync();
            _logger.LogInformation("[IconLaboratoryService] 🔍 Found {Count} icons to refactor", icons.Count);

            foreach (var icon in icons)
            {
                var oldName = icon.Name;
                
                var processedName = oldName.ToLowerInvariant().Replace("-", "_");
                
                if (processedName.StartsWith("av_")) processedName = processedName.Substring(3);
                else if (processedName.StartsWith("_av_")) processedName = processedName.Substring(4);
                else if (processedName.StartsWith("av-")) processedName = processedName.Substring(3);
                else if (processedName.StartsWith("_")) processedName = processedName.TrimStart('_');

                var newName = "av_" + processedName;

                if (oldName == newName) continue;

                var result = new IconRenameResult { OldName = oldName, NewName = newName, Success = true };

                try
                {
                    var conflict = await _uow.Icons.GetFirstOrDefaultAsync(i => i.Name == newName && i.Id != icon.Id);
                    if (conflict != null)
                    {
                        result.Success = false;
                        result.Message = $"Conflict with existing icon ID {conflict.Id}";
                        results.Add(result);
                        continue;
                    }

                    icon.Name = newName;
                    await _uow.SaveChangesAsync();
                    results.Add(result);
                    _logger.LogInformation("[IconLaboratoryService] 🏷️ Refactored: {Old} -> {New}", oldName, newName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[IconLaboratoryService] ❌ Failed to refactor icon {Name}", oldName);
                    result.Success = false;
                    result.Message = ex.Message;
                    results.Add(result);
                }
            }

            if (results.Any(r => r.Success))
            {
                await SyncToFrontendAsync("");
                InvalidateCache();
            }

            return results;
        }

        public async Task SaveIconToDiskAsync(SaveIconToDiskRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.FileName)) throw new ArgumentException("Filename is required");
            if (string.IsNullOrWhiteSpace(request.Path)) throw new ArgumentException("Path is required");

            var fileName = request.FileName;
            // Removed forced .svg extension to allow HTML/MD/etc.
            
            var targetPath = request.Path;
            // If path is relative and starts with src/, prepend FrontendProjectPath
            if (targetPath.StartsWith("src") || targetPath.StartsWith("/src"))
            {
                targetPath = Path.Combine(FrontendProjectPath, targetPath.TrimStart('/'));
            }

            if (!Directory.Exists(targetPath))
            {
                _logger.LogInformation("[IconLaboratoryService] 📁 Creating directory: {Path}", targetPath);
                Directory.CreateDirectory(targetPath);
            }

            var fullPath = Path.Combine(targetPath, fileName);
            _logger.LogInformation("[IconLaboratoryService] 💾 Saving file to disk: {FullPath}", fullPath);

            await File.WriteAllTextAsync(fullPath, request.SvgContent, System.Text.Encoding.UTF8);
        }

        public async Task CreateDirectoryAsync(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Path is required");
            
            if (!Directory.Exists(path))
            {
                _logger.LogInformation("[IconLaboratoryService] 📁 Explicitly creating directory: {Path}", path);
                Directory.CreateDirectory(path);
            }
        }

        public async Task OpenFileInEditorAsync(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Path is required");

            // Обработка относительных путей (если пришло src/...)
            string fullPath = path;
            if (path.StartsWith("src") || path.StartsWith("/src"))
            {
                fullPath = Path.Combine(FrontendProjectPath, path.TrimStart('/'));
            }

            // Security check: allow d:\_PROGECT\
            if (!fullPath.StartsWith(@"d:\_PROGECT\", StringComparison.OrdinalIgnoreCase))
            {
                 _logger.LogWarning("[IconLaboratoryService] ⚠️ Unauthorized file access attempt: {Path}", fullPath);
                 throw new UnauthorizedAccessException("Доступ к этому файлу запрещен.");
            }

            if (!File.Exists(fullPath))
            {
                _logger.LogWarning("[IconLaboratoryService] ⚠️ File not found for opening: {Path}", fullPath);
                throw new FileNotFoundException($"Файл не найден: {fullPath}");
            }

            _logger.LogInformation("[IconLaboratoryService] 🚀 Opening file in editor: {Path}", fullPath);
            
            Process.Start(new ProcessStartInfo
            {
                FileName = fullPath,
                UseShellExecute = true
            });

            await Task.CompletedTask;
        }
    }
}
