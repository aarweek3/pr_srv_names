// Controllers/EditorImageUploadController.cs

using Microsoft.AspNetCore.Mvc;
using pr_srv_names.Models.Editor;
using pr_srv_names.Services.Editor;

namespace pr_srv_names.Controllers
{
    [ApiController]
    [Route("api/editor/images")]
    [Produces("application/json")]
    public class EditorImageUploadController : ControllerBase
    {
        private readonly IEditorImageService _imageService;
        private readonly ILogger<EditorImageUploadController> _logger;

        public EditorImageUploadController(
            IEditorImageService imageService,
            ILogger<EditorImageUploadController> logger
        )
        {
            _imageService = imageService;
            _logger = logger;
        }

        [HttpPost("upload")]
        [ProducesResponseType(typeof(EditorImageUploadResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EditorImageUploadResponse>> Upload(
            [FromBody] EditorImageUploadRequest request
        )
        {
            try
            {
                _logger.LogInformation("Загрузка изображения: {FileName}", request.FileName);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _imageService.UploadImageAsync(request);

                if (!result.Success)
                {
                    return BadRequest(new { message = result.Message });
                }

                var response = new EditorImageUploadResponse
                {
                    Success = true,
                    ImageUrl = result.Url,
                    RelativePath = result.RelativePath,
                    FileSize = result.FileSize
                };

                _logger.LogInformation("Изображение успешно загружено: {Url}", response.ImageUrl);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка загрузки изображения");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Ошибка загрузки" });
            }
        }

        [HttpPost("validate")]
        [ProducesResponseType(typeof(EditorValidationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EditorValidationResponse>> Validate(
            [FromBody] EditorImageValidationRequest request
        )
        {
            try
            {
                _logger.LogInformation("Валидация изображения");

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var metadata = await _imageService.ValidateImageAsync(request);

                var response = new EditorValidationResponse
                {
                    IsValid = true,
                    Message = "Изображение валидно",
                    Width = metadata.Width,
                    Height = metadata.Height,
                    FileSizeBytes = metadata.FileSizeBytes
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Ошибка валидации");

                return BadRequest(new EditorValidationResponse
                {
                    IsValid = false,
                    Message = ex.Message,
                    Width = 0,
                    Height = 0,
                    FileSizeBytes = 0
                });
            }
        }

        [HttpPost("metadata")]
        [ProducesResponseType(typeof(EditorImageMetadataResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EditorImageMetadataResponse>> GetMetadata(
            [FromBody] EditorImageMetadataRequest request
        )
        {
            try
            {
                _logger.LogInformation("Получение метаданных");

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var metadata = await _imageService.GetImageMetadataAsync(request);

                var response = new EditorImageMetadataResponse
                {
                    Format = metadata.Format,
                    Width = metadata.Width,
                    Height = metadata.Height,
                    FileSizeBytes = metadata.FileSizeBytes,
                    MimeType = metadata.MimeType,
                    IsAnimated = metadata.IsAnimated,
                    DurationMs = metadata.DurationMs
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Ошибка получения метаданных");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("config")]
        [ProducesResponseType(typeof(EditorConfigResponse), StatusCodes.Status200OK)]
        public ActionResult<EditorConfigResponse> GetConfig()
        {
            try
            {
                var config = _imageService.GetConfiguration();
                return Ok(config);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения конфигурации");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Ошибка получения конфигурации" });
            }
        }

        [HttpGet("supported-formats")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<string>>> GetSupportedFormats()
        {
            try
            {
                var formats = await _imageService.GetSupportedFormatsAsync();
                return Ok(formats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения форматов");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Ошибка получения форматов" });
            }
        }

        [HttpGet("health")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<object> Health()
        {
            return Ok(new
            {
                status = "Healthy",
                timestamp = DateTime.UtcNow,
                apiVersion = "1.0"
            });
        }

        [HttpDelete("{filename}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteImage(string filename)
        {
            try
            {
                _logger.LogInformation("Удаление изображения: {Filename}", filename);

                var deleted = await _imageService.DeleteImageAsync(filename);

                if (!deleted)
                {
                    return NotFound(new { message = "Файл не найден" });
                }

                return Ok(new { message = "Файл успешно удалён" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка удаления");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Ошибка удаления" });
            }
        }

        [HttpPost("cleanup")]
        [ProducesResponseType(typeof(EditorImageCleanupResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EditorImageCleanupResponse>> Cleanup(
            [FromBody] EditorImageCleanupRequest request
        )
        {
            try
            {
                _logger.LogInformation("Очистка старых изображений: {DaysOld} дней", request.DaysOld);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var deletedCount = await _imageService.CleanupOldImagesAsync(request.DaysOld);

                var response = new EditorImageCleanupResponse
                {
                    DeletedCount = deletedCount,
                    FreedSpaceBytes = 0,
                    Message = $"Удалено {deletedCount} файлов"
                };

                _logger.LogInformation("Очистка завершена: {DeletedCount} файлов", deletedCount);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка очистки");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Ошибка очистки" });
            }
        }
    }
}