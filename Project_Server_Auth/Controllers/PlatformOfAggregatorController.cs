using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using pr_srv_names.Models.Errors;
using pr_srv_names.Pages.AGGREGATOR.PlatformOfAggregator.Dtos;
using pr_srv_names.Pages.AGGREGATOR.PlatformOfAggregator.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;

namespace pr_srv_names.Controllers
{
    /// <summary>
    /// Контроллер для управления платформами агрегатора (с поддержкой SEO и локализаций)
    /// </summary>
    [ApiController]
    [Route("api/v1/aggregator/platforms")]
    [Produces("application/json")]
    [EnableRateLimiting("DefaultPolicy")]
    public class PlatformOfAggregatorController : ControllerBase
    {
        private readonly IPlatformOfAggregatorService _service;
        private readonly ILogger<PlatformOfAggregatorController> _logger;

        public PlatformOfAggregatorController(
            IPlatformOfAggregatorService service,
            ILogger<PlatformOfAggregatorController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Получить список платформ с пагинацией и SEO данными
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PlatformOfAggregatorPagedResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged([FromQuery] PlatformOfAggregatorPageRequestDto request)
        {
            request ??= new PlatformOfAggregatorPageRequestDto();
            var result = await _service.GetPagedAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Получить детальную информацию о платформе
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PlatformOfAggregatorDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Создать новую платформу с поддержкой SEO
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(PlatformOfAggregatorDetailDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] PlatformOfAggregatorCreateDto request)
        {
            var result = await _service.CreateAsync(request);
            _logger.LogInformation("Агрегатор: Создана новая платформа {PlatformName} (ID: {Id})", result.Name, result.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Обновить платформу и её локализации/SEO
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PlatformOfAggregatorDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int id, [FromBody] PlatformOfAggregatorUpdateDto request)
        {
            request.Id = id;
            var result = await _service.UpdateAsync(request);
            _logger.LogInformation("Агрегатор: Обновлена платформа ID {Id}", id);
            return Ok(result);
        }

        /// <summary>
        /// Удалить платформу
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, [FromQuery] bool isHard = false)
        {
            if (isHard)
            {
                await _service.HardDeleteAsync(id);
                _logger.LogCritical("Агрегатор: ПОЛНОЕ УДАЛЕНИЕ платформы ID {Id}", id);
            }
            else
            {
                await _service.DeleteAsync(id);
                _logger.LogWarning("Агрегатор: Мягкое удаление платформы ID {Id}", id);
            }
            return NoContent();
        }

        /// <summary>
        /// Восстановить платформу из корзины
        /// </summary>
        [HttpPost("{id}/restore")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Restore(int id)
        {
            await _service.RestoreAsync(id);
            _logger.LogInformation("Агрегатор: Восстановлена платформа ID {Id}", id);
            return NoContent();
        }

        /// <summary>
        /// Полная очистка таблицы платформ (Обслуживание)
        /// </summary>
        [HttpDelete("maintenance/clear")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ClearAll()
        {
            await _service.ClearAllAsync();
            return NoContent();
        }

        /// <summary>
        /// Сидинг данных из JSON файла (Обслуживание)
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
