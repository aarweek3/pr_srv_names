using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DAL;
using DAL.Models.GeneralModels;
using pr_srv_names.Models.Errors;

namespace pr_srv_names.Controllers
{
    [ApiController]
    [Route("api/v1/media")]
    [Authorize]
    [Produces("application/json")]
    [Tags("Universal Media 🖼️")]
    public class UniversalMediaController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;
        private readonly ILogger<UniversalMediaController> _logger;

        public UniversalMediaController(IWebHostEnvironment env, AppDbContext context, ILogger<UniversalMediaController> logger)
        {
            _env = env;
            _context = context;
            _logger = logger;
        }

        [HttpGet("check-exists")]
        public async Task<IActionResult> CheckExists([FromQuery] string fileName, [FromQuery] string folder)
        {
            if (string.IsNullOrEmpty(fileName)) return BadRequest("Имя файла не указано");
            
            string subFolder = SanitizePath(folder) ?? "general";
            string slug = Slugify(fileName);
            string extension = Path.GetExtension(fileName).ToLower();
            if (string.IsNullOrEmpty(extension)) extension = ".jpg"; 

            string finalFileName = $"{slug}{extension}";
            string shard = GetShard(finalFileName);
            var relativePath = $"uploads/{subFolder}/{shard}/{finalFileName}";

            // Ищем в БД
            var existingRecord = await _context.MediaFiles
                .Where(f => f.RelativePath == relativePath)
                .FirstOrDefaultAsync();

            var fullPath = Path.Combine(_env.WebRootPath, relativePath.Replace('/', Path.DirectorySeparatorChar));
            bool physicalExists = System.IO.File.Exists(fullPath);

            if (existingRecord == null && !physicalExists)
            {
                return Ok(new { exists = false });
            }

            // Если файл есть, пытаемся вытащить метаданные
            long size = 0;
            int w = 0;
            int h = 0;

            if (physicalExists)
            {
                var fi = new FileInfo(fullPath);
                size = fi.Length;
            }

            return Ok(new
            {
                exists = true,
                id = existingRecord?.Id,
                originalName = existingRecord?.OriginalName ?? finalFileName,
                relativePath = relativePath,
                fileSize = existingRecord?.FileSize ?? size,
                width = existingRecord?.Width > 0 ? existingRecord.Width : w,
                height = existingRecord?.Height > 0 ? existingRecord.Height : h,
                fullUrl = GetFullUrl(relativePath)
            });
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromBody] UniversalUploadRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.ImageBase64))
            {
                return BadRequest(new ErrorResponse { Detail = "Нет данных изображения" });
            }

            try
            {
                string subFolder = SanitizePath(request.Folder) ?? "general";
                string slug = Slugify(request.FileName);
                
                var base64Data = request.ImageBase64;
                var mimeType = "image/jpeg";
                if (base64Data.Contains(","))
                {
                    var parts = base64Data.Split(',');
                    mimeType = parts[0].Split(':')[1].Split(';')[0];
                    base64Data = parts[1];
                }
                string extension = GetExtensionFromMime(mimeType);
                
                string finalFileName = $"{slug}{extension}";
                string shard = GetShard(finalFileName);
                
                var relativePath = $"uploads/{subFolder}/{shard}/{finalFileName}";
                var uploadPath = Path.Combine(_env.WebRootPath, "uploads", subFolder, shard);

                if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, finalFileName);
                var imageBytes = Convert.FromBase64String(base64Data);

                // Сохранение
                await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);

                var existingRecord = await _context.MediaFiles
                    .Where(f => f.RelativePath == relativePath)
                    .FirstOrDefaultAsync();

                if (existingRecord != null)
                {
                    existingRecord.FileSize = imageBytes.Length;
                    existingRecord.Width = request.Width > 0 ? request.Width : existingRecord.Width;
                    existingRecord.Height = request.Height > 0 ? request.Height : existingRecord.Height;
                    existingRecord.UpdatedAt = DateTime.UtcNow;
                    _context.MediaFiles.Update(existingRecord);
                }
                else
                {
                    var newFile = new MediaFile
                    {
                        ImageId = Guid.NewGuid(),
                        OriginalName = request.FileName ?? finalFileName,
                        RelativePath = relativePath,
                        ContentType = mimeType,
                        FileSize = imageBytes.Length,
                        Width = request.Width,
                        Height = request.Height,
                        UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "system",
                        Purpose = subFolder
                    };
                    _context.MediaFiles.Add(newFile);
                }

                await _context.SaveChangesAsync();

                return Ok(new MediaUploadResponse
                {
                    FileName = finalFileName,
                    RelativePath = relativePath,
                    FullUrl = GetFullUrl(relativePath),
                    Folder = subFolder,
                    Size = imageBytes.Length
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Detail = ex.Message });
            }
        }

        #region Helpers

        private string GetShard(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return "o";
            return char.ToLower(fileName[0]).ToString();
        }

        private string GetFullUrl(string relativePath)
        {
            return $"{Request.Scheme}://{Request.Host}{Request.PathBase}/{relativePath.TrimStart('/')}";
        }

        private string SanitizePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return null;
            var cleanPath = path.Replace("..", "").Replace(":", "").Replace("*", "").Trim('/', '\\');
            cleanPath = Regex.Replace(cleanPath, @"[^a-zA-Z0-9\s-_\/\\\.]", "-");
            return string.IsNullOrEmpty(cleanPath) ? null : cleanPath;
        }

        private string Slugify(string text)
        {
            if (string.IsNullOrEmpty(text)) return $"img-{DateTime.Now.Ticks.ToString().Substring(10)}";
            string fileName = Path.GetFileNameWithoutExtension(text);
            // Усиленная нормализация
            string result = Transliterate(fileName.ToLower().Trim());
            result = Regex.Replace(result, @"[^a-z0-9\s-_]", "-");
            result = Regex.Replace(result, @"[\s-_]+", "-");
            return result.Trim('-', ' ').ToLower();
        }

        private string Transliterate(string text)
        {
            var words = new System.Collections.Generic.Dictionary<string, string>
            {
                {"а", "a"}, {"б", "b"}, {"в", "v"}, {"г", "g"}, {"д", "d"}, {"е", "e"}, {"ё", "yo"},
                {"ж", "zh"}, {"з", "z"}, {"и", "i"}, {"й", "j"}, {"к", "k"}, {"л", "l"}, {"м", "m"},
                {"н", "n"}, {"о", "o"}, {"п", "p"}, {"р", "r"}, {"с", "s"}, {"т", "t"}, {"у", "u"},
                {"ф", "f"}, {"х", "h"}, {"ц", "ts"}, {"ч", "ch"}, {"ш", "sh"}, {"щ", "shch"},
                {"ъ", ""}, {"ы", "y"}, {"ь", ""}, {"э", "e"}, {"ю", "yu"}, {"я", "ya"}
            };
            foreach (var item in words) text = text.Replace(item.Key, item.Value);
            return text;
        }

        private string GetExtensionFromMime(string mimeType)
        {
            return mimeType.ToLower() switch
            {
                "image/jpeg" => ".jpg",
                "image/png" => ".png",
                "image/webp" => ".webp",
                "image/gif" => ".gif",
                _ => ".jpg"
            };
        }
        #endregion

        #region DTOs
        public class UniversalUploadRequest
        {
            public string ImageBase64 { get; set; } = string.Empty;
            public string FileName { get; set; } = string.Empty;
            public string Folder { get; set; } = string.Empty;
            public int Width { get; set; }
            public int Height { get; set; }
        }

        public class MediaUploadResponse
        {
            public string FileName { get; set; } = string.Empty;
            public string RelativePath { get; set; } = string.Empty;
            public string FullUrl { get; set; } = string.Empty;
            public string Folder { get; set; } = string.Empty;
            public long Size { get; set; }
        }
        #endregion
    }
}
