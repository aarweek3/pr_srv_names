using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pr_srv_names.Pages.Icons.Interfaces;
using pr_srv_names.Pages.Icons.Models;

namespace pr_srv_names.Controllers
{
    [ApiController]
    [Route("api/Icons")]
    //[Authorize(Roles = "Admin")] // Require Admin for laboratory operations
    public class IconsLaboratoryController : ControllerBase
    {
        private readonly IIconLaboratoryService _iconLabService;
        private readonly ILogger<IconsLaboratoryController> _logger;

        public IconsLaboratoryController(IIconLaboratoryService iconLabService, ILogger<IconsLaboratoryController> logger)
        {
            _iconLabService = iconLabService;
            _logger = logger;
        }

        [HttpPost("batch-update")]
        public async Task<IActionResult> UpdateIconsBatch([FromBody] List<UpdateIconRequest> requests)
        {
            try
            {
                if (requests == null || !requests.Any())
                {
                    return BadRequest("Requests list is empty");
                }

                await _iconLabService.UpdateIconsBatchAsync(requests);
                return Ok(new { message = $"Successfully updated {requests.Count} icons" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при массовом обновлении иконок");
                return StatusCode(500, "Ошибка сервера при массовом сохранении иконок");
            }
        }

        [HttpPost("sync-to-local")]
        public async Task<IActionResult> SyncIcons()
        {
            try
            {
                await _iconLabService.SyncToFrontendAsync("");
                return Ok(new { message = "Icons synced successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при синхронизации иконок");
                return StatusCode(500, "Ошибка сервера при синхронизации иконок");
            }
        }

        [HttpPost("move")]
        public async Task<IActionResult> MoveIcon([FromBody] MoveIconRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.IconType) || request.TargetCategoryId <= 0)
                {
                    return BadRequest("Invalid move request. IconType and TargetCategoryId are required.");
                }

                await _iconLabService.MoveIconAsync(request.IconType, request.TargetCategoryId);
                return Ok(new { success = true, message = "Icon moved successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при перемещении иконки {IconType} в категорию ID {TargetId}", request.IconType, request.TargetCategoryId);
                return StatusCode(500, new { success = false, message = "Внутренняя ошибка сервера при перемещении иконки" });
            }
        }

        [HttpPost("rename")]
        public async Task<IActionResult> RenameIcon([FromBody] RenameIconRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.OldName) || string.IsNullOrEmpty(request.NewName))
                {
                    return BadRequest("Invalid rename request. OldName and NewName are required.");
                }

                await _iconLabService.RenameIconAsync(request.OldName, request.NewName);
                return Ok(new { success = true, message = "Icon renamed successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { success = false, message = ex.Message });
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при переименовании иконки {OldName} → {NewName}", request.OldName, request.NewName);
                return StatusCode(500, new { success = false, message = "Внутренняя ошибка сервера при переименовании иконки" });
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateIcon([FromBody] UpdateIconRequest request)
        {
            try
            {
                await _iconLabService.UpdateIconContentAsync(request.IconType, request.SvgContent, request.ToBackend, request.ToFrontend);
                return Ok(new { message = "Icon updated successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении иконки {IconType}", request.IconType);
                return StatusCode(500, new { message = "Ошибка сервера при обновлении иконки", detail = ex.Message });
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteIcon([FromQuery] string iconType, [FromQuery] bool fromBackend = true, [FromQuery] bool fromFrontend = true)
        {
            try
            {
                if (string.IsNullOrEmpty(iconType)) return BadRequest("IconType is required");
                // Декодируем тип иконки (т.к. он может содержать слеш)
                string decodedType = Uri.UnescapeDataString(iconType);
                await _iconLabService.DeleteIconAsync(decodedType, fromBackend, fromFrontend);
                return Ok(new { message = "Icon deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении иконки {IconType}", iconType);
                return StatusCode(500, "Ошибка сервера при удалении иконки");
            }
        }

        [HttpGet("sync-status")]
        public async Task<IActionResult> GetSyncStatus()
        {
            try
            {
                var status = await _iconLabService.GetSyncStatusAsync();
                return Ok(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении статуса синхронизации");
                return StatusCode(500, "Ошибка сервера при получении статуса");
            }
        }

        [HttpPost("refactor-names")]
        public async Task<IActionResult> RefactorNames([FromQuery] int? categoryId = null)
        {
            try
            {
                var results = await _iconLabService.RefactorIconNamesAsync(categoryId);
                return Ok(new
                {
                    message = "Icon names refactored and synced successfully",
                    details = results
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при рефакторинге имен иконок");
                return StatusCode(500, "Ошибка сервера при рефакторинге имен");
            }
        }

        [HttpPost("bulk-rename")]
        public async Task<IActionResult> BulkRename([FromBody] BulkRenameRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new { success = false, message = "Запрос не может быть пустым" });
                }

                // Если не указаны конкретные файлы и не указана исходная папка (для режима "копировать всё")
                if ((request.FileNames == null || !request.FileNames.Any()) && string.IsNullOrEmpty(request.SourceFolder))
                {
                    return BadRequest(new { success = false, message = "Список файлов или исходная папка должны быть указаны" });
                }

                if (string.IsNullOrEmpty(request.Prefix))
                {
                    return BadRequest(new { success = false, message = "Префикс обязателен" });
                }

                if (string.IsNullOrEmpty(request.TargetFolder))
                {
                    return BadRequest(new { success = false, message = "Целевая папка обязательна" });
                }

                _logger.LogInformation("Bulk rename request: {Count} files, prefix: {Prefix}, target: {Target}", 
                    request.FileNames.Count, request.Prefix, request.TargetFolder);

                var result = await _iconLabService.BulkRenameAsync(request);
                
                return Ok(new 
                { 
                    success = true, 
                    message = $"Обработано {result.TotalFiles} файлов. Успешно: {result.SuccessCount}, Ошибок: {result.FailedCount}",
                    data = result
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error in bulk rename");
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при массовом переименовании файлов");
                return StatusCode(500, new { success = false, message = "Внутренняя ошибка сервера при массовом переименовании" });
            }
        }

        [HttpGet("browse-filesystem")]
        public async Task<IActionResult> BrowseFileSystem([FromQuery] string? path = "")
        {
            try
            {
                var items = await _iconLabService.BrowseFileSystemAsync(path ?? "");
                return Ok(items);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при просмотре файлов");
                return StatusCode(500, "Ошибка сервера при просмотре файлов");
            }
        }

        [HttpPost("save-to-disk")]
        public async Task<IActionResult> SaveToDisk([FromBody] SaveIconToDiskRequest request)
        {
            try
            {
                if (request == null) return BadRequest("Request is null");
                await _iconLabService.SaveIconToDiskAsync(request);
                return Ok(new { success = true, message = $"Иконка {request.FileName} успешно сохранена на диск!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении иконки на диск");
                return StatusCode(500, new { success = false, message = "Ошибка сервера при сохранении иконки", detail = ex.Message });
            }
        }

        [HttpPost("create-directory")]
        public async Task<IActionResult> CreateDirectory([FromQuery] string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path)) return BadRequest("Path is required");
                await _iconLabService.CreateDirectoryAsync(path);
                return Ok(new { success = true, message = $"Папка успешно создана!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании папки");
                return StatusCode(500, new { success = false, message = "Ошибка сервера при создании папки", detail = ex.Message });
            }
        }

        [HttpPost("open-file")]
        public async Task<IActionResult> OpenFile([FromQuery] string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path)) return BadRequest("Path is required");
                await _iconLabService.OpenFileInEditorAsync(path);
                return Ok(new { success = true, message = "Файл открыт успешно!" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при открытии файла {Path}", path);
                return StatusCode(500, new { success = false, message = "Ошибка сервера при открытии файла", detail = ex.Message });
            }
        }
    }
}
