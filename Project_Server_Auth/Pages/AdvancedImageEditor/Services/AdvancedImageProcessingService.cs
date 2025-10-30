// Services/AdvancedImageProcessingService.cs

using pr_srv_names.Pages.AdvancedImageEditor.Models;

namespace pr_srv_names.Pages.AdvancedImageEditor.Services
{
    public class AdvancedImageProcessingService : IAdvancedImageProcessingService
    {
        private readonly ILogger<AdvancedImageProcessingService> _logger;
        private readonly string _imageStoragePath;
        private readonly string _baseUrl;

        public AdvancedImageProcessingService(
            ILogger<AdvancedImageProcessingService> logger,
            IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // ✅ ИСПРАВЛЕНО: Правильный путь к папке
            var uploadPath = configuration.GetValue<string>("ImageStoragePath") ?? "uploads/images";
            _imageStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", uploadPath);

            // ✅ ДОБАВЛЕНО: Базовый URL для возврата ссылок
            _baseUrl = configuration["JwtSettings:Issuer"] ?? "https://localhost:7233";

            // Создаем папку, если не существует
            if (!Directory.Exists(_imageStoragePath))
            {
                Directory.CreateDirectory(_imageStoragePath);
                _logger.LogInformation("Создана папка для хранения изображений: {Path}", _imageStoragePath);
            }

            _logger.LogInformation("Инициализирован AdvancedImageProcessingService. Путь: {Path}", _imageStoragePath);
        }

        #region Заглушки обработки (вся логика на клиенте)

        public async Task<AdvancedImageProcessResult> ProcessImageAsync(AdvancedImageProcessRequest request)
        {
            await Task.CompletedTask;
            _logger.LogInformation("Заглушка: обработка изображения выполняется на клиенте");

            return new AdvancedImageProcessResult
            {
                Success = true,
                Message = "Обработка изображений выполняется на клиенте",
                ProcessedImageBase64 = request.ImageBase64,
                OutputFormat = request.OutputFormat,
                Width = 0,
                Height = 0,
                ProcessingTimeMs = 0,
                AppliedOperations = new List<string> { "client-side-processing" }
            };
        }

        public async Task<ImageInfoResult> GetImageInfoAsync(ImageInfoRequest request)
        {
            await Task.CompletedTask;
            _logger.LogInformation("Заглушка: анализ изображения выполняется на клиенте");

            return new ImageInfoResult
            {
                Width = 0,
                Height = 0,
                Format = "Unknown",
                FileSize = 0,
                ColorDepth = 0,
                HasTransparency = false
            };
        }

        public async Task<ProcessingCapabilitiesResponse> GetProcessingCapabilitiesAsync()
        {
            await Task.CompletedTask;

            return new ProcessingCapabilitiesResponse
            {
                SupportedFormats = new[] { "jpeg", "png", "webp", "bmp", "gif" },
                MaxImageSize = 50 * 1024 * 1024, // 50MB
                MaxWidth = 8192,
                MaxHeight = 8192,
                SupportedFilters = new[]
                {
                    "brightness", "contrast", "saturation", "hue", "gamma",
                    "sepia", "grayscale", "invert", "blur", "sharpen"
                },
                SupportedWatermarkPositions = new[]
                {
                    "top-left", "top-center", "top-right",
                    "center-left", "center", "center-right",
                    "bottom-left", "bottom-center", "bottom-right"
                }
            };
        }

        #endregion

        #region Реальное хранение изображений

        public async Task<SaveImageResult> SaveImageAsync(SaveImageRequest request)
        {
            try
            {
                _logger.LogInformation("Сохраняем изображение: {FileName}", request.FileName);

                // ✅ ИСПРАВЛЕНО: Удаляем префикс data:image, если он есть
                var base64Data = request.ImageBase64;
                if (base64Data.Contains(","))
                {
                    base64Data = base64Data.Split(',')[1];
                }

                var imageId = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(request.FileName);
                var fileName = $"{imageId}{extension}";
                var filePath = Path.Combine(_imageStoragePath, fileName);

                var imageBytes = Convert.FromBase64String(base64Data);
                await File.WriteAllBytesAsync(filePath, imageBytes);

                // Сохраняем метаданные
                var metadataPath = Path.Combine(_imageStoragePath, $"{imageId}.json");
                var metadata = new
                {
                    ImageId = imageId,
                    OriginalFileName = request.FileName,
                    SavedFileName = fileName,
                    Description = request.Description,
                    CreatedAt = DateTime.UtcNow,
                    FileSize = imageBytes.Length
                };

                var metadataJson = System.Text.Json.JsonSerializer.Serialize(metadata,
                    new System.Text.Json.JsonSerializerOptions
                    {
                        WriteIndented = true
                    });
                await File.WriteAllTextAsync(metadataPath, metadataJson);

                // ✅ ИСПРАВЛЕНО: Правильный URL
                var imageUrl = $"{_baseUrl}/uploads/images/{fileName}";

                _logger.LogInformation("Изображение сохранено: {ImageId}, URL: {Url}", imageId, imageUrl);

                return new SaveImageResult
                {
                    Success = true,
                    Message = "Изображение успешно сохранено",
                    ImageId = imageId,
                    ImageUrl = imageUrl
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении изображения");
                return new SaveImageResult
                {
                    Success = false,
                    Message = $"Ошибка сохранения: {ex.Message}"
                };
            }
        }

        public async Task<LoadImageResult> LoadImageAsync(string imageId)
        {
            try
            {
                _logger.LogInformation("Загружаем изображение: {ImageId}", imageId);

                var metadataPath = Path.Combine(_imageStoragePath, $"{imageId}.json");
                if (!File.Exists(metadataPath))
                {
                    return new LoadImageResult
                    {
                        Success = false,
                        Message = "Изображение не найдено"
                    };
                }

                var metadataJson = await File.ReadAllTextAsync(metadataPath);
                using var doc = System.Text.Json.JsonDocument.Parse(metadataJson);
                var root = doc.RootElement;

                // ✅ ИСПРАВЛЕНО: Используем сохраненное имя файла
                var savedFileName = root.GetProperty("SavedFileName").GetString() ?? "";
                var filePath = Path.Combine(_imageStoragePath, savedFileName);

                if (!File.Exists(filePath))
                {
                    return new LoadImageResult
                    {
                        Success = false,
                        Message = "Файл изображения не найден"
                    };
                }

                var imageBytes = await File.ReadAllBytesAsync(filePath);
                var imageBase64 = Convert.ToBase64String(imageBytes);

                // ✅ ДОБАВЛЕНО: Определяем MIME тип
                var extension = Path.GetExtension(savedFileName).ToLower();
                var mimeType = extension switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".gif" => "image/gif",
                    ".webp" => "image/webp",
                    ".bmp" => "image/bmp",
                    _ => "image/jpeg"
                };

                var originalFileName = root.GetProperty("OriginalFileName").GetString() ?? "";

                return new LoadImageResult
                {
                    Success = true,
                    Message = "Изображение загружено",
                    ImageBase64 = $"data:{mimeType};base64,{imageBase64}",
                    FileName = originalFileName,
                    Description = root.TryGetProperty("Description", out var desc) ? desc.GetString() : null,
                    CreatedAt = root.GetProperty("CreatedAt").GetDateTime()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке изображения");
                return new LoadImageResult
                {
                    Success = false,
                    Message = $"Ошибка загрузки: {ex.Message}"
                };
            }
        }

        public async Task<bool> DeleteImageAsync(string imageId)
        {
            try
            {
                _logger.LogInformation("Удаляем изображение: {ImageId}", imageId);

                var metadataPath = Path.Combine(_imageStoragePath, $"{imageId}.json");
                if (!File.Exists(metadataPath))
                {
                    return false;
                }

                var metadataJson = await File.ReadAllTextAsync(metadataPath);
                using var doc = System.Text.Json.JsonDocument.Parse(metadataJson);
                var root = doc.RootElement;

                // ✅ ИСПРАВЛЕНО: Используем сохраненное имя файла
                var savedFileName = root.GetProperty("SavedFileName").GetString() ?? "";
                var filePath = Path.Combine(_imageStoragePath, savedFileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    _logger.LogInformation("Удален файл изображения: {FilePath}", filePath);
                }

                File.Delete(metadataPath);
                _logger.LogInformation("Удалены метаданные: {MetadataPath}", metadataPath);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении изображения");
                return false;
            }
        }

        public async Task<List<string>> GetImageListAsync()
        {
            try
            {
                await Task.CompletedTask;

                var metadataFiles = Directory.GetFiles(_imageStoragePath, "*.json");
                var imageIds = metadataFiles
                    .Select(f => Path.GetFileNameWithoutExtension(f))
                    .ToList();

                _logger.LogInformation("Найдено изображений: {Count}", imageIds.Count);

                return imageIds;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка изображений");
                return new List<string>();
            }
        }

        #endregion
    }
}