using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pr_srv_names.Pages.Icons.Interfaces;
using pr_srv_names.Pages.Icons.Models;
using System.Diagnostics;

namespace pr_srv_names.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize] // Временно закомментировано для тестов, если нужно
    public class IconsController : ControllerBase
    {
        private readonly IIconGetService _iconService;
        private readonly ILogger<IconsController> _logger;

        public IconsController(IIconGetService iconService, ILogger<IconsController> logger)
        {
            _iconService = iconService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetIcons([FromQuery] bool includeSvgContent = false)
        {
            try
            {
                var icons = await _iconService.GetIconsAsync(includeSvgContent);
                return Ok(icons);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка иконок");
                return StatusCode(500, "Ошибка сервера при получении списка иконок");
            }
        }

        [HttpGet("content/{name}")]
        public async Task<IActionResult> GetIconContent(string name)
        {
            try 
            {
                var content = await _iconService.GetContentByNameAsync(name);
                if (content == null) return NotFound($"Icon '{name}' not found");
                
                // Return SVG content directly with correct content type
                return Content(content, "image/svg+xml");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving icon content for {Name}", name);
                return StatusCode(500, "Server error");
            }
        }

        [HttpGet("category/{categoryId}/content")]
        public async Task<IActionResult> GetCategoryIcons(int categoryId)
        {
            try
            {
                var icons = await _iconService.GetIconsByCategoryAsync(categoryId);
                return Ok(icons);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении иконок категории {CategoryId}", categoryId);
                return StatusCode(500, "Ошибка сервера");
            }
        }

        [HttpPost("batch-content")]
        public async Task<IActionResult> GetIconsContentBatch([FromBody] List<string> names)
        {
            try
            {
                var sw = Stopwatch.StartNew();
                var contents = await _iconService.GetIconsContentBatchAsync(names);
                sw.Stop();

                Response.Headers.Append("Server-Timing", $"db;dur={sw.ElapsedMilliseconds}");
                return Ok(contents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при массовом получении контента иконок");
                return StatusCode(500, "Ошибка сервера");
            }
        }
    }
}
