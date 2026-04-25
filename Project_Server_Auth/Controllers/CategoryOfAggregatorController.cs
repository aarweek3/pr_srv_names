using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using pr_srv_names.Models.Errors;
using pr_srv_names.Pages.AGGREGATOR.CategoryOfAggregator.Dtos;
using pr_srv_names.Pages.AGGREGATOR.CategoryOfAggregator.Interfaces;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace pr_srv_names.Controllers
{
    /// <summary>
    /// Контроллер для управления категориями агрегатора (с поддержкой иерархии и локализаций)
    /// </summary>
    [ApiController]
    [Route("api/v1/aggregator/categories")]
    [Produces("application/json")]
    [EnableRateLimiting("DefaultPolicy")]
    public class CategoryOfAggregatorController : ControllerBase
    {
        private readonly ICategoryOfAggregatorService _service;
        private readonly ILogger<CategoryOfAggregatorController> _logger;

        public CategoryOfAggregatorController(
            ICategoryOfAggregatorService service,
            ILogger<CategoryOfAggregatorController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Получить список категорий с пагинацией
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(CategoryOfAggregatorPagedResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged([FromQuery] CategoryOfAggregatorPageRequestDto request)
        {
            request ??= new CategoryOfAggregatorPageRequestDto();
            var result = await _service.GetPagedAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Получить иерархическое дерево категорий
        /// </summary>
        [HttpGet("tree")]
        [ProducesResponseType(typeof(List<CategoryOfAggregatorItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTree([FromQuery] int? languageId)
        {
            var result = await _service.GetTreeAsync(languageId);
            return Ok(result);
        }

        /// <summary>
        /// Получить детальную информацию о категории
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryOfAggregatorDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Создать новую категорию
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CategoryOfAggregatorDetailDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CategoryOfAggregatorCreateDto request)
        {
            var result = await _service.CreateAsync(request);
            _logger.LogInformation("Агрегатор: Создана новая категория {CategoryName} (ID: {Id})", result.CanonicalName, result.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Обновить данные категории
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CategoryOfAggregatorDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryOfAggregatorUpdateDto request)
        {
            request.Id = id;
            var result = await _service.UpdateAsync(request);
            _logger.LogInformation("Агрегатор: Обновлена категория ID {Id}", id);
            return Ok(result);
        }

        /// <summary>
        /// Удалить категорию
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, [FromQuery] bool hardDelete = false)
        {
            if (hardDelete)
            {
                await _service.HardDeleteAsync(id);
                _logger.LogCritical("Агрегатор: ПОЛНОЕ УДАЛЕНИЕ категории ID {Id}", id);
            }
            else
            {
                await _service.DeleteAsync(id);
                _logger.LogWarning("Агрегатор: Мягкое удаление категории ID {Id}", id);
            }
            return NoContent();
        }

        /// <summary>
        /// Восстановить категорию из корзины
        /// </summary>
        [HttpPost("{id}/restore")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Restore(int id)
        {
            await _service.RestoreAsync(id);
            _logger.LogInformation("Агрегатор: Восстановлена категория ID {Id}", id);
            return NoContent();
        }

        /// <summary>
        /// Полная очистка таблицы категорий (Обслуживание)
        /// </summary>
        [HttpPost("maintenance/clear")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ClearAll()
        {
            await _service.ClearAllAsync();
            return NoContent();
        }

        /// <summary>
        /// Сидинг данных из JSON (Обслуживание)
        /// </summary>
        [HttpPost("maintenance/seed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Seed()
        {
            var count = await _service.SeedFromJsonAsync();
            return Ok(new { message = $"Сидинг завершен. Добавлено записей: {count}" });
        }
    }
}
