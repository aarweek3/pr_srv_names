using Microsoft.AspNetCore.Mvc;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Text;
using DAL.Models.GeneralModels;

namespace pr_srv_names.Controllers
{
    [ApiController]
    [Route("api/av-image-studio")]
    [Authorize]
    public class AvImageStudioController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;
        private readonly ILogger<AvImageStudioController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public AvImageStudioController(
            IWebHostEnvironment env, 
            AppDbContext context, 
            ILogger<AvImageStudioController> logger,
            IHttpClientFactory httpClientFactory)
        {
            _env = env;
            _context = context;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Прокси для загрузки внешних изображений (обход CORS)
        /// GET /api/av-image-studio/proxy-image?url=...
        /// </summary>
        [HttpGet("proxy-image")]
        [AllowAnonymous] // Позволяем загружать превью без авторизации, если нужно (или уберите, если сессия обязательна)
        public async Task<IActionResult> ProxyImage([FromQuery] string url)
        {
            if (string.IsNullOrEmpty(url))
                return BadRequest(new { message = "URL обязателен" });

            try
            {
                var client = _httpClientFactory.CreateClient("ImageStudioProxy");
                client.Timeout = TimeSpan.FromSeconds(15);
                
                // Добавляем типичные заголовки, чтобы сервера не блокировали как бота
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

                var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Studio Proxy: Не удалось загрузить изображение: {Url}, Status: {Status}", url, response.StatusCode);
                    return StatusCode((int)response.StatusCode, new { message = "Не удалось загрузить изображение из внешнего источника" });
                }

                var contentType = response.Content.Headers.ContentType?.MediaType ?? "image/jpeg";
                var stream = await response.Content.ReadAsStreamAsync();

                _logger.LogInformation("Studio Proxy: Проксирование {Url} -> {Type}", url, contentType);

                return File(stream, contentType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Studio Proxy: Ошибка при загрузке {Url}", url);
                return StatusCode(500, new { error = "Ошибка проксирования", details = ex.Message });
            }
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] StudioSaveRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.ImageBase64))
                return BadRequest(new { success = false, message = "Файл не передан" });

            try
            {
                // 1. Подготовка папок по дате
                var now = DateTime.Now;
                var year = now.Year.ToString();
                var month = now.Month.ToString("00");
                var day = now.Day.ToString("00");

                var uploadPath = Path.Combine(_env.WebRootPath, "uploads", "studio", year, month, day);
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                // 2. Обработка Base64
                var base64Data = request.ImageBase64;
                var mimeType = "image/jpeg";
                if (base64Data.Contains(","))
                {
                    var parts = base64Data.Split(',');
                    mimeType = parts[0].Split(':')[1].Split(';')[0];
                    base64Data = parts[1];
                }

                // 3. Генерация имени файла (Slugify)
                string rawName = request.FileName ?? "studio-image";
                string slug = Slugify(rawName);
                string extension = GetExtensionFromMime(mimeType);
                
                var imageId = Guid.NewGuid();
                string shortId = imageId.ToString().Substring(0, 8);
                var fileName = $"{slug}-{shortId}{extension}";
                
                var filePath = Path.Combine(uploadPath, fileName);
                var imageBytes = Convert.FromBase64String(base64Data);

                await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);

                // 4. Относительный URL для базы данных
                var relativeUrl = $"/uploads/studio/{year}/{month}/{day}/{fileName}";

                // 5. Сохранение в БД MediaFiles
                var mediaFile = new MediaFile
                {
                    ImageId = imageId,
                    OriginalName = request.FileName ?? fileName,
                    RelativePath = relativeUrl,
                    ContentType = mimeType,
                    FileSize = imageBytes.Length,
                    Width = request.Width > 0 ? request.Width : 0,
                    Height = request.Height > 0 ? request.Height : 0,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "system",
                    Purpose = "studio"
                };

                _context.MediaFiles.Add(mediaFile);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Image Studio: Сохранено изображение {FileName} -> {Url}", fileName, relativeUrl);

                return Ok(new 
                { 
                    success = true, 
                    url = relativeUrl, 
                    imageId = imageId.ToString(),
                    name = fileName
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении изображения в Image Studio");
                return StatusCode(500, new { success = false, message = $"Ошибка сервера: {ex.Message}" });
            }
        }

        private string GetExtensionFromMime(string mimeType)
        {
            return mimeType.ToLower() switch
            {
                "image/jpeg" => ".jpg",
                "image/pjpeg" => ".jpg",
                "image/png" => ".png",
                "image/webp" => ".webp",
                "image/gif" => ".gif",
                "image/bmp" => ".bmp",
                "image/x-windows-bmp" => ".bmp",
                "image/tiff" => ".tif",
                _ => ".jpg"
            };
        }

        private string Slugify(string text)
        {
            if (string.IsNullOrEmpty(text)) return "image";
            
            // Если пришел полный путь или имя с расширением - убираем лишнее
            string fileName = Path.GetFileNameWithoutExtension(text);
            if (string.IsNullOrEmpty(fileName)) fileName = "image";

            string result = fileName.ToLower().Trim();
            result = Transliterate(result);
            
            // Замена всех не-алфавитно-цифровых символов на дефис
            result = Regex.Replace(result, @"[^a-z0-9\s-_]", "-");
            // Схлопывание пробелов и дефисов
            result = Regex.Replace(result, @"[\s-_]+", "-");
            // Убираем дефисы по краям
            result = result.Trim('-');

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
            foreach (var item in words) text = text.Replace(item.Key, item.Value);
            return text;
        }
    }

    public class StudioSaveRequest
    {
        public string ImageBase64 { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
