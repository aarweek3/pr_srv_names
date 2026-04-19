using System.Text.Json;
using Project_Server_Auth.Services.Maintenance.Interfaces;

namespace Project_Server_Auth.Services.Maintenance
{
    /// <summary>
    /// Реализация загрузчика данных из JSON файлов
    /// </summary>
    public class JsonMaintenanceSeeder : IMaintenanceSeeder
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<JsonMaintenanceSeeder> _logger;

        public JsonMaintenanceSeeder(IWebHostEnvironment env, ILogger<JsonMaintenanceSeeder> logger)
        {
            _env = env;
            _logger = logger;
        }

        public async Task<int> SeedAsync<TDto>(string relativeJsonPath, Func<TDto, Task> seedAction)
        {
            // Пытаемся найти файл относительно корня контента (обычно корень проекта Project_Server_Auth)
            var filePath = Path.Combine(_env.ContentRootPath, relativeJsonPath);

            if (!File.Exists(filePath))
            {
                _logger.LogError("Seeder: Файл не найден по пути: {Path}", filePath);
                // Попробуем поискать в AppContext (на случай если файлы копируются в bin)
                var fallbackPath = Path.Combine(AppContext.BaseDirectory, relativeJsonPath);
                if (File.Exists(fallbackPath))
                {
                    filePath = fallbackPath;
                }
                else
                {
                    throw new FileNotFoundException($"Файл данных не найден: {relativeJsonPath}. Проверено: {filePath} и {fallbackPath}");
                }
            }

            try
            {
                var json = await File.ReadAllTextAsync(filePath);
                var items = JsonSerializer.Deserialize<List<TDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (items == null || items.Count == 0)
                {
                    _logger.LogWarning("Seeder: Файл {Path} пуст или содержит некорректные данные", relativeJsonPath);
                    return 0;
                }

                int count = 0;
                foreach (var item in items)
                {
                    await seedAction(item);
                    count++;
                }

                _logger.LogInformation("Seeder: Успешно импортировано {Count} записей из {Path}", count, relativeJsonPath);
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Seeder: Ошибка при импорте из файла {Path}", relativeJsonPath);
                throw;
            }
        }
    }
}
