using Microsoft.AspNetCore.Mvc;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http; // ВАЖНО для IFormFile
using Microsoft.EntityFrameworkCore; // ДЛЯ ToListAsync
using System.IO;  // ВАЖНО для Path, Directory, FileStream
using System;     // ВАЖНО для Guid, Exception
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;
using DAL.Models.GeneralModels;

namespace pr_srv_names.Controllers
{
    [ApiController]
    [Route("api/simple-image")]
    [Authorize] // Оставляем, так как токен мы передаем
    public class ImageUploadTinyController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;
        // Убрали лишние сервисы
        public ImageUploadTinyController(IWebHostEnvironment env, AppDbContext context)
        {
            _env = env;
            _context = context;
        }
        [HttpPost("upload")]
        [ApiExplorerSettings(IgnoreApi = true)] // Скрыть от Swagger
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Upload([FromForm] IFormFile image, [FromForm] string? originalName)
        {
            if (image == null || image.Length == 0)
                return BadRequest("Файл не передан");
            try
            {
                // Date-oriented folder structure: uploads/images/YYYY/MM/DD
                var now = DateTime.Now;
                var year = now.Year.ToString();
                var month = now.Month.ToString("00");
                var day = now.Day.ToString("00");

                var uploadPath = Path.Combine(_env.WebRootPath, "uploads", "images", year, month, day);
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                // SEO-friendly name generation
                string rawName = originalName ?? image?.FileName ?? "image";
                string slug = Slugify(rawName);
                string extension = Path.GetExtension(image?.FileName ?? rawName);
                if (string.IsNullOrEmpty(extension)) extension = ".jpg";
                
                // Final filename: slug-shortid.ext
                var imageId = Guid.NewGuid();
                string shortId = imageId.ToString().Substring(0, 8);
                var fileName = $"{slug}-{shortId}{extension}";
                
                var filePath = Path.Combine(uploadPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                var url = $"/uploads/images/{year}/{month}/{day}/{fileName}";
                var idStr = imageId.ToString();

                // Получаем размеры изображения
                int width = 0;
                int height = 0;
                try
                {
                    var imageInfo = SixLabors.ImageSharp.Image.Identify(filePath);
                    if (imageInfo != null)
                    {
                        width = imageInfo.Width;
                        height = imageInfo.Height;
                    }
                }
                catch { /* Игнорируем ошибки чтения размеров */ }

                // Сохраняем метаданные в БД
                var mediaFile = new MediaFile
                {
                    ImageId = imageId,
                    OriginalName = originalName ?? image?.FileName ?? fileName,
                    RelativePath = url,
                    ContentType = image?.ContentType ?? "image/jpeg",
                    FileSize = image?.Length ?? 0,
                    Width = width,
                    Height = height,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    Purpose = "editor"
                };

                _context.MediaFiles.Add(mediaFile);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, imageUrl = url, imageId = idStr });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Ошибка сервера: {ex.Message}" });
            }
        }

        [HttpGet("list")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> GetImages()
        {
            try
            {
                var files = await _context.MediaFiles
                    .OrderByDescending(f => f.CreatedAt)
                    .Take(50)
                    .Select(f => new
                    {
                        f.ImageId,
                        f.OriginalName,
                        f.RelativePath,
                        f.ContentType,
                        f.FileSize,
                        f.Width,
                        f.Height,
                        f.CreatedAt
                    })
                    .ToListAsync();

                return Ok(files);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка получения списка: {ex.Message}" });
            }
        }

        private string Slugify(string text)
        {
            if (string.IsNullOrEmpty(text)) return "image";

            // 1. Убираем расширение
            string fileName = Path.GetFileNameWithoutExtension(text);
            
            // 2. Имя в нижний регистр + тримминг пробелов
            string result = fileName.Replace(".", "-").ToLower().Trim();

            // 3. Транслитерация
            result = Transliterate(result);

            // 4. Очистка (разрешаем _, -, пробелы)
            result = Regex.Replace(result, @"[^a-z0-9\s-_]", "");

            // 5. Заменяем ВНУТРЕННИЕ нижние подчеркивания на дефисы
            // (?<!^) - не в начале строки
            // (?!$)  - не в конце строки
            result = Regex.Replace(result, @"(?<!^)_+(?!$)", "-");

            // 6. Пробелы в дефисы
            result = Regex.Replace(result, @"\s+", "-");

            // 7. Схлопываем множественные дефисы
            result = Regex.Replace(result, @"-+", "-");

            return string.IsNullOrEmpty(result) ? "image" : result;
        }

        private string Transliterate(string text)
        {
            var words = new Dictionary<string, string>
            {
                {"а", "a"}, {"б", "b"}, {"в", "v"}, {"г", "g"}, {"д", "d"}, {"е", "e"}, {"ё", "yo"},
                {"ж", "zh"}, {"з", "z"}, {"и", "i"}, {"й", "j"}, {"к", "k"}, {"л", "l"}, {"м", "m"},
                {"н", "n"}, {"о", "o"}, {"п", "p"}, {"р", "r"}, {"с", "s"}, {"т", "t"}, {"у", "u"},
                {"ф", "f"}, {"х", "h"}, {"ц", "ts"}, {"ч", "ch"}, {"ш", "sh"}, {"щ", "shch"},
                {"ъ", ""}, {"ы", "y"}, {"ь", ""}, {"э", "e"}, {"ю", "yu"}, {"я", "ya"}
            };
            foreach (var item in words)
            {
                text = text.Replace(item.Key, item.Value);
            }
            return text;
        }
    }
}
