using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using pr_srv_names.Models.Errors;
using pr_srv_names.Pages.AGGREGATOR.TagOfAggregator.Dtos;
using pr_srv_names.Pages.AGGREGATOR.TagOfAggregator.Interfaces;
using System.Threading.Tasks;
using System;

namespace pr_srv_names.Controllers
{
    /// <summary>
    /// Контроллер для управления тегами агрегатора.
    /// </summary>
    [ApiController]
    [Route("api/v1/aggregator/tags")]
    [Produces("application/json")]
    [EnableRateLimiting("DefaultPolicy")]
    public class TagOfAggregatorController : ControllerBase
    {
        private readonly ITagOfAggregatorService _service;
        private readonly ILogger<TagOfAggregatorController> _logger;

        public TagOfAggregatorController(
            ITagOfAggregatorService service,
            ILogger<TagOfAggregatorController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(TagOfAggregatorPagedResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged([FromQuery] TagOfAggregatorPageRequestDto request)
        {
            request ??= new TagOfAggregatorPageRequestDto();
            var result = await _service.GetPagedAsync(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TagOfAggregatorDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(TagOfAggregatorDetailDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] TagOfAggregatorCreateDto request)
        {
            var result = await _service.CreateAsync(request);
            _logger.LogInformation("Агрегатор: Создан новый тег {Slug} (ID: {Id})", result.Slug, result.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TagOfAggregatorDetailDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(int id, [FromBody] TagOfAggregatorUpdateDto request)
        {
            request.Id = id;
            var result = await _service.UpdateAsync(request);
            _logger.LogInformation("Агрегатор: Обновлен тег ID {Id}", id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            _logger.LogWarning("Агрегатор: Мягкое удаление тега ID {Id}", id);
            return NoContent();
        }

        [HttpPost("{id}/restore")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Restore(int id)
        {
            await _service.RestoreAsync(id);
            _logger.LogInformation("Агрегатор: Восстановлен тег ID {Id}", id);
            return NoContent();
        }

        /// <summary>
        /// Обновить порядок сортировки (Drag-and-Drop)
        /// </summary>
        [HttpPatch("{id}/sort-order")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateSortOrder(int id, [FromBody] int newSortOrder)
        {
            await _service.UpdateSortOrderAsync(id, newSortOrder);
            return Ok(new { id, sortOrder = newSortOrder });
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
