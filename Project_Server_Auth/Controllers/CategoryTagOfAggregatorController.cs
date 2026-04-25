using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using pr_srv_names.Models.Errors;
using pr_srv_names.Pages.AGGREGATOR.CategoryTagOfAggregator.Dtos;
using pr_srv_names.Pages.AGGREGATOR.CategoryTagOfAggregator.Interfaces;
using System.Threading.Tasks;
using System;

namespace pr_srv_names.Controllers
{
    /// <summary>
    /// Контроллер для управления категориями (группами) тегов агрегатора.
    /// </summary>
    [ApiController]
    [Route("api/v1/aggregator/category-tags")]
    [Produces("application/json")]
    [EnableRateLimiting("DefaultPolicy")]
    public class CategoryTagOfAggregatorController : ControllerBase
    {
        private readonly ICategoryTagOfAggregatorService _service;
        private readonly ILogger<CategoryTagOfAggregatorController> _logger;

        public CategoryTagOfAggregatorController(
            ICategoryTagOfAggregatorService service,
            ILogger<CategoryTagOfAggregatorController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(CategoryTagOfAggregatorPagedResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged([FromQuery] CategoryTagOfAggregatorPageRequestDto request)
        {
            request ??= new CategoryTagOfAggregatorPageRequestDto();
            var result = await _service.GetPagedAsync(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryTagOfAggregatorDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CategoryTagOfAggregatorDetailDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CategoryTagOfAggregatorCreateDto request)
        {
            var result = await _service.CreateAsync(request);
            _logger.LogInformation("Агрегатор: Создана новая категория тегов {Slug} (ID: {Id})", result.Slug, result.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CategoryTagOfAggregatorDetailDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryTagOfAggregatorUpdateDto request)
        {
            request.Id = id;
            var result = await _service.UpdateAsync(request);
            _logger.LogInformation("Агрегатор: Обновлена категория тегов ID {Id}", id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            _logger.LogWarning("Агрегатор: Мягкое удаление категории тегов ID {Id}", id);
            return NoContent();
        }

        [HttpPost("{id}/restore")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Restore(int id)
        {
            await _service.RestoreAsync(id);
            _logger.LogInformation("Агрегатор: Восстановлена категория тегов ID {Id}", id);
            return NoContent();
        }

        [HttpPost("maintenance/seed")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> Seed()
        {
            var result = await _service.SeedFromJsonAsync();
            return Ok(result);
        }

        [HttpDelete("maintenance/clear")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> Clear()
        {
            var result = await _service.ClearAllAsync();
            return Ok(result);
        }
    }
}
