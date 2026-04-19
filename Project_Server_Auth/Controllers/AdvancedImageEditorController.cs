using Microsoft.AspNetCore.Mvc;
using pr_srv_names.Pages.AdvancedImageEditor.Models;
using pr_srv_names.Pages.AdvancedImageEditor.Services;

namespace pr_srv_names.Pages.AdvancedImageEditor.Controllers
{
    [ApiController]
    [Route("api/advanced-image")]
    public class AdvancedImageEditorController : ControllerBase
    {
        private readonly IAdvancedImageProcessingService _imageProcessingService;
        private readonly ILogger<AdvancedImageEditorController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public AdvancedImageEditorController(
            IAdvancedImageProcessingService imageProcessingService,
            ILogger<AdvancedImageEditorController> logger,
            IHttpClientFactory httpClientFactory)
        {
            _imageProcessingService =
                imageProcessingService ?? throw new ArgumentNullException(nameof(imageProcessingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        /// <summary>
        /// Обработка изображения (заглушка - вся логика на клиенте)
        /// </summary>
        [HttpPost("process-image")]
        [ProducesResponseType(typeof(AdvancedImageProcessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ProcessImage([FromBody] AdvancedImageProcessRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrEmpty(request.ImageBase64))
                {
                    return BadRequest(new { message = "Изображение обязательно для загрузки" });
                }

                var result = await _imageProcessingService.ProcessImageAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке изображения");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { error = "Внутренняя ошибка сервера", details = ex.Message });
            }
        }

        /// <summary>
        /// Получение информации об изображении (заглушка)
        /// </summary>
        [HttpPost("get-image-info")]
        [ProducesResponseType(typeof(ImageInfoResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetImageInfo([FromBody] ImageInfoRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrEmpty(request.ImageBase64))
                {
                    return BadRequest(new { message = "Изображение обязательно для анализа" });
                }

                var result = await _imageProcessingService.GetImageInfoAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении информации об изображении");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { error = "Внутренняя ошибка сервера", details = ex.Message });
            }
        }

        /// <summary>
        /// Получение возможностей API для обработки изображений
        /// </summary>
        [HttpGet("processing-capabilities")]
        [ProducesResponseType(typeof(ProcessingCapabilitiesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProcessingCapabilities()
        {
            try
            {
                var result = await _imageProcessingService.GetProcessingCapabilitiesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении возможностей обработки");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { error = "Внутренняя ошибка сервера", details = ex.Message });
            }
        }

        /// <summary>
        /// Сохранение изображения на сервере
        /// POST /api/advanced-image/save
        /// </summary>
        [HttpPost("save")]
        [ProducesResponseType(typeof(SaveImageResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status413PayloadTooLarge)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveImage([FromBody] SaveImageRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Невалидная модель при сохранении изображения");
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrEmpty(request.ImageBase64))
                {
                    _logger.LogWarning("Пустое изображение при сохранении");
                    return BadRequest(new SaveImageResult
                    {
                        Success = false,
                        Message = "Изображение обязательно для сохранения"
                    });
                }

                if (string.IsNullOrEmpty(request.FileName))
                {
                    _logger.LogWarning("Отсутствует имя файла при сохранении");
                    return BadRequest(new SaveImageResult
                    {
                        Success = false,
                        Message = "Имя файла обязательно"
                    });
                }

                var result = await _imageProcessingService.SaveImageAsync(request);

                if (!result.Success)
                {
                    // ✅ Специфичные HTTP коды для разных типов ошибок

                    // 413 Payload Too Large - файл слишком большой
                    if (result.Message.Contains("слишком большое") ||
                        result.Message.Contains("превышает") ||
                        result.Message.Contains("макс 5MB"))
                    {
                        _logger.LogWarning("Файл слишком большой: {Message}", result.Message);
                        return StatusCode(StatusCodes.Status413PayloadTooLarge, result);
                    }

                    // 415 Unsupported Media Type - неподдерживаемый формат
                    if (result.Message.Contains("Неподдерживаемый формат") ||
                        result.Message.Contains("формат изображения") ||
                        result.Message.Contains("Разрешены:"))
                    {
                        _logger.LogWarning("Неподдерживаемый формат: {Message}", result.Message);
                        return StatusCode(StatusCodes.Status415UnsupportedMediaType, result);
                    }

                    // 400 Bad Request - другие ошибки валидации
                    _logger.LogWarning("Ошибка валидации при сохранении: {Message}", result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("Изображение успешно сохранено: {ImageId}", result.ImageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Критическая ошибка при сохранении изображения");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new SaveImageResult
                    {
                        Success = false,
                        Message = "Внутренняя ошибка сервера при сохранении изображения"
                    });
            }
        }

        /// <summary>
        /// Загрузка изображения с сервера
        /// GET /api/advanced-image/load/{imageId}
        /// </summary>
        [HttpGet("load/{imageId}")]
        [ProducesResponseType(typeof(LoadImageResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadImage(string imageId)
        {
            try
            {
                if (string.IsNullOrEmpty(imageId))
                {
                    _logger.LogWarning("Попытка загрузки с пустым imageId");
                    return BadRequest(new LoadImageResult
                    {
                        Success = false,
                        Message = "ID изображения обязательно"
                    });
                }

                var result = await _imageProcessingService.LoadImageAsync(imageId);

                if (!result.Success)
                {
                    _logger.LogWarning("Изображение не найдено: {ImageId}", imageId);
                    return NotFound(result);
                }

                _logger.LogInformation("Изображение загружено: {ImageId}", imageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке изображения: {ImageId}", imageId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new LoadImageResult
                    {
                        Success = false,
                        Message = "Внутренняя ошибка сервера при загрузке изображения"
                    });
            }
        }

        /// <summary>
        /// Удаление изображения
        /// DELETE /api/advanced-image/delete/{imageId}
        /// </summary>
        [HttpDelete("delete/{imageId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteImage(string imageId)
        {
            try
            {
                if (string.IsNullOrEmpty(imageId))
                {
                    _logger.LogWarning("Попытка удаления с пустым imageId");
                    return BadRequest(new { message = "ID изображения обязательно" });
                }

                var success = await _imageProcessingService.DeleteImageAsync(imageId);

                if (!success)
                {
                    _logger.LogWarning("Изображение не найдено для удаления: {ImageId}", imageId);
                    return NotFound(new { message = "Изображение не найдено" });
                }

                _logger.LogInformation("Изображение удалено: {ImageId}", imageId);
                return Ok(new { message = "Изображение успешно удалено", imageId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении изображения: {ImageId}", imageId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { error = "Внутренняя ошибка сервера", details = ex.Message });
            }
        }

        /// <summary>
        /// Получение списка всех изображений
        /// GET /api/advanced-image/list
        /// </summary>
        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetImageList()
        {
            try
            {
                var imageIds = await _imageProcessingService.GetImageListAsync();

                _logger.LogInformation("Получен список изображений: {Count} шт.", imageIds.Count);

                return Ok(new
                {
                    success = true,
                    count = imageIds.Count,
                    images = imageIds
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка изображений");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { error = "Внутренняя ошибка сервера", details = ex.Message });
            }
        }

        /// <summary>
        /// Прокси для загрузки внешних изображений (обход CORS)
        /// GET /api/advanced-image/proxy-image?url=...
        /// </summary>
        [HttpGet("proxy-image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ProxyImage([FromQuery] string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return BadRequest(new { message = "URL обязателен" });
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                client.Timeout = TimeSpan.FromSeconds(15);
                var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Не удалось загрузить внешнее изображение: {Url}, Status: {Status}", url, response.StatusCode);
                    return StatusCode((int)response.StatusCode, new { message = "Не удалось загрузить изображение из внешнего источника" });
                }

                var contentType = response.Content.Headers.ContentType?.MediaType ?? "image/jpeg";
                var stream = await response.Content.ReadAsStreamAsync();

                _logger.LogInformation("Проксирование изображения: {Url} (Type: {Type})", url, contentType);

                return File(stream, contentType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при проксировании изображения: {Url}", url);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { error = "Ошибка при проксировании изображения", details = ex.Message });
            }
        }
    }
}