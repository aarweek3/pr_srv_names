// Services/Editor/EditorImageService.cs

using Microsoft.Extensions.Configuration;
using pr_srv_names.Models.Editor;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace pr_srv_names.Services.Editor
{
    public class EditorImageService : IEditorImageService
    {
        private readonly string _uploadDirectory;
        private readonly string _baseUrl;
        private readonly long _maxFileSize;
        private readonly int _maxImageWidth;
        private readonly int _maxImageHeight;

        private readonly List<string> _supportedMimeTypes = new()
        {
            "image/jpeg",
            "image/png",
            "image/gif",
            "image/webp",
            "image/bmp"
        };

        public EditorImageService(IConfiguration configuration)
        {
            var imageSettings = configuration.GetSection("ImageUploadSettings");
            var uploadPath = imageSettings["UploadPath"] ?? "uploads";

            _uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", uploadPath);
            _baseUrl = configuration["JwtSettings:Issuer"] ?? "https://localhost:7233";
            _maxFileSize = long.Parse(imageSettings["MaxFileSizeBytes"] ?? (5 * 1024 * 1024).ToString());
            _maxImageWidth = int.Parse(imageSettings["MaxImageWidth"] ?? "2560");
            _maxImageHeight = int.Parse(imageSettings["MaxImageHeight"] ?? "1440");

            if (!Directory.Exists(_uploadDirectory))
            {
                Directory.CreateDirectory(_uploadDirectory);
            }
        }

        public async Task<ImageUploadResult> UploadImageAsync(EditorImageUploadRequest request)
        {
            try
            {
                var validation = await ValidateImageAsync(new EditorImageValidationRequest
                {
                    Base64Data = request.Base64Data,
                    FileFormat = request.FileFormat
                });

                if (validation.Width == 0 || validation.Height == 0)
                {
                    return new ImageUploadResult
                    {
                        Success = false,
                        Message = "Изображение не валидно"
                    };
                }

                var base64 = request.Base64Data.Contains(",")
                    ? request.Base64Data.Split(",")[1]
                    : request.Base64Data;

                byte[] imageBytes = Convert.FromBase64String(base64);

                if (imageBytes.Length > _maxFileSize)
                {
                    return new ImageUploadResult
                    {
                        Success = false,
                        Message = $"Размер файла превышает максимум: {_maxFileSize / (1024 * 1024)} MB"
                    };
                }

                var fileName = GenerateFileName(request.FileName);
                var filePath = Path.Combine(_uploadDirectory, fileName);

                await File.WriteAllBytesAsync(filePath, imageBytes);

                return new ImageUploadResult
                {
                    Success = true,
                    Message = "Изображение успешно загружено",
                    FilePath = filePath,
                    RelativePath = $"uploads/{fileName}",
                    Url = $"{_baseUrl}/uploads/{fileName}",
                    FileSize = imageBytes.Length,
                    Metadata = validation
                };
            }
            catch (Exception ex)
            {
                return new ImageUploadResult
                {
                    Success = false,
                    Message = $"Ошибка загрузки: {ex.Message}"
                };
            }
        }

        public async Task<ImageMetadata> ValidateImageAsync(EditorImageValidationRequest request)
        {
            try
            {
                if (!_supportedMimeTypes.Contains(request.FileFormat))
                {
                    throw new Exception($"MIME тип не поддерживается: {request.FileFormat}");
                }

                var base64 = request.Base64Data.Contains(",")
                    ? request.Base64Data.Split(",")[1]
                    : request.Base64Data;

                byte[] imageBytes = Convert.FromBase64String(base64);

                if (imageBytes.Length > _maxFileSize)
                {
                    throw new Exception("Размер файла слишком большой");
                }

                using (var ms = new MemoryStream(imageBytes))
                {
                    var imageInfo = await SixLabors.ImageSharp.Image.IdentifyAsync(ms);

                    if (imageInfo == null)
                    {
                        throw new Exception("Не удалось определить формат изображения");
                    }

                    var metadata = new ImageMetadata
                    {
                        Width = imageInfo.Width,
                        Height = imageInfo.Height,
                        Format = GetFormatFromMimeType(request.FileFormat),
                        MimeType = request.FileFormat,
                        IsAnimated = imageInfo.Metadata.DecodedImageFormat?.Name == "GIF" &&
                                     imageInfo.FrameMetadataCollection?.Count > 1,
                        FileSizeBytes = imageBytes.Length,
                        DurationMs = 0
                    };

                    if (metadata.Width > _maxImageWidth || metadata.Height > _maxImageHeight)
                    {
                        throw new Exception(
                            $"Размеры изображения превышают максимум: {_maxImageWidth}x{_maxImageHeight}"
                        );
                    }

                    return metadata;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка валидации: {ex.Message}");
            }
        }

        public async Task<ImageMetadata> GetImageMetadataAsync(EditorImageMetadataRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request?.Base64Data))
                {
                    throw new ArgumentException("Base64 данные не предоставлены");
                }

                var base64 = request.Base64Data.Contains(",")
                    ? request.Base64Data.Split(",")[1]
                    : request.Base64Data;

                byte[] imageBytes = Convert.FromBase64String(base64);

                using (var ms = new MemoryStream(imageBytes))
                {
                    var imageInfo = await SixLabors.ImageSharp.Image.IdentifyAsync(ms);

                    if (imageInfo == null)
                    {
                        throw new Exception("Не удалось определить формат изображения");
                    }

                    var format = imageInfo.Metadata.DecodedImageFormat;
                    var formatName = format?.Name?.ToLower() ?? "unknown";
                    var mimeType = GetMimeTypeFromFormat(formatName);

                    var metadata = new ImageMetadata
                    {
                        Width = imageInfo.Width,
                        Height = imageInfo.Height,
                        Format = formatName,
                        MimeType = mimeType,
                        IsAnimated = formatName == "gif" && imageInfo.FrameMetadataCollection?.Count > 1,
                        FileSizeBytes = imageBytes.Length,
                        DurationMs = 0
                    };

                    return metadata;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения метаданных: {ex.Message}");
            }
        }

        public async Task<List<string>> GetSupportedFormatsAsync()
        {
            return await Task.FromResult(_supportedMimeTypes.ToList());
        }

        public async Task<bool> DeleteImageAsync(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    return false;
                }

                var fullPath = filePath;
                if (!Path.IsPathRooted(filePath))
                {
                    fullPath = Path.Combine(_uploadDirectory, Path.GetFileName(filePath));
                }

                if (File.Exists(fullPath))
                {
                    await Task.Run(() => File.Delete(fullPath));
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> CleanupOldImagesAsync(int daysOld = 30)
        {
            try
            {
                if (!Directory.Exists(_uploadDirectory))
                {
                    return 0;
                }

                var files = Directory.GetFiles(_uploadDirectory);
                var deletedCount = 0;
                var cutoffDate = DateTime.Now.AddDays(-daysOld);

                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.CreationTime < cutoffDate)
                    {
                        try
                        {
                            await Task.Run(() => File.Delete(file));
                            deletedCount++;
                        }
                        catch
                        {
                            // Игнорируем ошибки при удалении отдельных файлов
                        }
                    }
                }

                return deletedCount;
            }
            catch
            {
                return 0;
            }
        }

        public EditorConfigResponse GetConfiguration()
        {
            return new EditorConfigResponse
            {
                MaxFileSizeBytes = _maxFileSize,
                SupportedFormats = _supportedMimeTypes,
                UploadBaseUrl = $"{_baseUrl}/uploads",
                CropConfig = new EditorCropConfigResponse
                {
                    PredefinedSizes = new List<CropSize>
                    {
                        new() { Name = "Квадрат 300x300", Width = 300, Height = 300 },
                        new() { Name = "Баннер 800x200", Width = 800, Height = 200 },
                        new() { Name = "Пост 600x400", Width = 600, Height = 400 },
                        new() { Name = "Аватар 150x150", Width = 150, Height = 150 }
                    }
                },
                CompressionConfig = new EditorCompressionConfigResponse
                {
                    Enabled = true,
                    Quality = 80
                }
            };
        }

        // Приватные вспомогательные методы
        private string GenerateFileName(string originalFileName)
        {
            var ext = Path.GetExtension(originalFileName);
            var name = Guid.NewGuid().ToString();
            return $"{name}{ext}";
        }

        private string GetFormatFromMimeType(string mimeType)
        {
            return mimeType switch
            {
                "image/jpeg" => "jpeg",
                "image/png" => "png",
                "image/gif" => "gif",
                "image/webp" => "webp",
                "image/bmp" => "bmp",
                _ => "unknown"
            };
        }

        private string GetMimeTypeFromFormat(string format)
        {
            return format?.ToLower() switch
            {
                "jpeg" or "jpg" => "image/jpeg",
                "png" => "image/png",
                "gif" => "image/gif",
                "webp" => "image/webp",
                "bmp" => "image/bmp",
                _ => "image/unknown"
            };
        }
    }
}