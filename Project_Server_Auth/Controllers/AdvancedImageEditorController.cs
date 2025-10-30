using Microsoft.AspNetCore.Mvc;
using pr_srv_names.Pages.AdvancedImageEditor.Models;
using pr_srv_names.Pages.AdvancedImageEditor.Services;

namespace pr_srv_names.Pages.AdvancedImageEditor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdvancedImageEditorController : ControllerBase
    {
        private readonly IAdvancedImageProcessingService _imageProcessingService;
        private readonly ILogger<AdvancedImageEditorController> _logger;

        public AdvancedImageEditorController(
            IAdvancedImageProcessingService imageProcessingService,
            ILogger<AdvancedImageEditorController> logger)
        {
            _imageProcessingService =
                imageProcessingService ?? throw new ArgumentNullException(nameof(imageProcessingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Обработка изображения (заглушка - вся логика на клиенте)
        /// </summary>
        [HttpPost("process-image")]
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
                    return BadRequest("Изображение обязательно для загрузки");
                }

                var result = await _imageProcessingService.ProcessImageAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке изображения");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера", details = ex.Message });
            }
        }

        /// <summary>
        /// Получение информации об изображении (заглушка)
        /// </summary>
        [HttpPost("get-image-info")]
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
                    return BadRequest("Изображение обязательно для анализа");
                }

                var result = await _imageProcessingService.GetImageInfoAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении информации об изображении");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера", details = ex.Message });
            }
        }

        /// <summary>
        /// Получение возможностей API для обработки изображений
        /// </summary>
        [HttpGet("processing-capabilities")]
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
                return StatusCode(500, new { error = "Внутренняя ошибка сервера", details = ex.Message });
            }
        }

        /// <summary>
        /// Сохранение изображения на сервере
        /// </summary>
        [HttpPost("save")]
        public async Task<IActionResult> SaveImage([FromBody] SaveImageRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrEmpty(request.ImageBase64))
                {
                    return BadRequest("Изображение обязательно для сохранения");
                }

                var result = await _imageProcessingService.SaveImageAsync(request);

                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении изображения");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера", details = ex.Message });
            }
        }

        /// <summary>
        /// Загрузка изображения с сервера
        /// </summary>
        [HttpGet("load/{imageId}")]
        public async Task<IActionResult> LoadImage(string imageId)
        {
            try
            {
                if (string.IsNullOrEmpty(imageId))
                {
                    return BadRequest("ID изображения обязательно");
                }

                var result = await _imageProcessingService.LoadImageAsync(imageId);

                if (!result.Success)
                {
                    return NotFound(result.Message);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке изображения");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера", details = ex.Message });
            }
        }

        /// <summary>
        /// Удаление изображения
        /// </summary>
        [HttpDelete("delete/{imageId}")]
        public async Task<IActionResult> DeleteImage(string imageId)
        {
            try
            {
                if (string.IsNullOrEmpty(imageId))
                {
                    return BadRequest("ID изображения обязательно");
                }

                var success = await _imageProcessingService.DeleteImageAsync(imageId);

                if (!success)
                {
                    return NotFound("Изображение не найдено");
                }

                return Ok(new { message = "Изображение удалено" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении изображения");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера", details = ex.Message });
            }
        }

        /// <summary>
        /// Получение списка всех изображений
        /// </summary>
        [HttpGet("list")]
        public async Task<IActionResult> GetImageList()
        {
            try
            {
                var imageIds = await _imageProcessingService.GetImageListAsync();
                return Ok(new { images = imageIds });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка изображений");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера", details = ex.Message });
            }
        }
    }
}