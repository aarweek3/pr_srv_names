using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Server_Auth.Pages.CategoryRepository.Dtos;
using Project_Server_Auth.Pages.CategoryRepository.Interfaces;

namespace pr_srv_names.Controllers
{
    /// <summary>
    /// Контроллер для управления категориями (разделами) иконок
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class IconCategoryController : ControllerBase
    {
        private readonly IIconCategoryService _categoryService;
        private readonly pr_srv_names.Pages.Icons.Interfaces.IIconService _iconService;
        private readonly ILogger<IconCategoryController> _logger;

        public IconCategoryController(
            IIconCategoryService categoryService,
            pr_srv_names.Pages.Icons.Interfaces.IIconService iconService,
            ILogger<IconCategoryController> logger)
        {
            _categoryService = categoryService;
            _iconService = iconService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await _categoryService.GetAllAsync();
                return Ok(new { success = true, data = categories });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении категорий иконок");
                return StatusCode(500, new { success = false, message = "Внутренняя ошибка сервера" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null) return NotFound(new { success = false, message = "Категория не найдена" });
            return Ok(new { success = true, data = category });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IconCategoryCreateDto dto)
        {
            try
            {
                var result = await _categoryService.CreateAsync(dto);
                await _iconService.SyncToFrontendAsync("");
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании категории иконок");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] IconCategoryUpdateDto dto)
        {
            var result = await _categoryService.UpdateAsync(id, dto);
            if (result == null) return NotFound(new { success = false, message = "Категория не найдена" });
            await _iconService.SyncToFrontendAsync("");
            return Ok(new { success = true, data = result });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _categoryService.DeleteAsync(id);
                if (!success) return NotFound(new { success = false, message = "Категория не найдена" });
                await _iconService.SyncToFrontendAsync("");
                return Ok(new { success = true, message = "Категория успешно удалена из БД" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении категории {Id}", id);
                return StatusCode(500, new { success = false, message = "Внутренняя ошибка сервера" });
            }
        }

        [HttpPost("sync")]
        public async Task<IActionResult> Sync()
        {
            try
            {
                var result = await _categoryService.SyncWithFileSystemAsync();
                await _iconService.SyncToFrontendAsync("");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка во время синхронизации иконок");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
