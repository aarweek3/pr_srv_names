using pr_srv_names.Pages.Icons.Interfaces;
using pr_srv_names.Pages.Icons.Models;
using System.Linq;
using System.Collections.Generic;
using DAL.Interfaces;
using DbModels = DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace pr_srv_names.Pages.Icons.Services
{
    public class IconGetService : IIconGetService
    {
        private readonly ILogger<IconGetService> _logger;
        private readonly IUnitOfWork _uow;

        // Static cache for the metadata registry
        private static List<IconCategoryPage>? _cachedRegistry = null;
        private static readonly object _cacheLock = new object();

        public IconGetService(
            IUnitOfWork uow,
            ILogger<IconGetService> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        /// <summary>
        /// Invalidates the cache. Should be called by Laboratory Service.
        /// </summary>
        public static void InvalidateCache()
        {
            lock (_cacheLock)
            {
                _cachedRegistry = null;
            }
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
            return _cachedRegistry!;
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
    }
}
