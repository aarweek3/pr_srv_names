using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using pr_srv_names.Models.Errors;
using pr_srv_names.Pages.AGGREGATOR.DeveloperOfAggregator.Dtos;
using pr_srv_names.Pages.AGGREGATOR.DeveloperOfAggregator.Interfaces;
using System.Threading.Tasks;
using System;

namespace pr_srv_names.Controllers
{
    /// <summary>
    /// Контроллер для управления разработчиками агрегатора (с поддержкой локализаций)
    /// </summary>
    [ApiController]
    [Route("api/v1/aggregator/developers")]
    [Produces("application/json")]
    [EnableRateLimiting("DefaultPolicy")]
    public class DeveloperOfAggregatorController : ControllerBase
    {
        private readonly IDeveloperOfAggregatorService _service;
        private readonly ILogger<DeveloperOfAggregatorController> _logger;

        public DeveloperOfAggregatorController(
            IDeveloperOfAggregatorService service,
            ILogger<DeveloperOfAggregatorController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Получить список разработчиков с пагинацией
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(DeveloperOfAggregatorPagedResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged([FromQuery] DeveloperOfAggregatorPageRequestDto request)
        {
            request ??= new DeveloperOfAggregatorPageRequestDto();
            var result = await _service.GetPagedAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Получить детальную информацию о разработчике
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DeveloperOfAggregatorDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Создать нового разработчика
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(DeveloperOfAggregatorDetailDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] DeveloperOfAggregatorCreateDto request)
        {
            var result = await _service.CreateAsync(request);
            _logger.LogInformation("Агрегатор: Создан новый разработчик {DeveloperName} (ID: {Id})", result.Name, result.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Обновить данные разработчика
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(DeveloperOfAggregatorDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int id, [FromBody] DeveloperOfAggregatorUpdateDto request)
        {
            request.Id = id;
            var result = await _service.UpdateAsync(request);
            _logger.LogInformation("Агрегатор: Обновлен разработчик ID {Id}", id);
            return Ok(result);
        }

        /// <summary>
        /// Удалить разработчика
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, [FromQuery] bool isHard = false)
        {
            if (isHard)
            {
                await _service.HardDeleteAsync(id);
                _logger.LogCritical("Агрегатор: ПОЛНОЕ УДАЛЕНИЕ разработчика ID {Id}", id);
            }
            else
            {
                await _service.DeleteAsync(id);
                _logger.LogWarning("Агрегатор: Мягкое удаление разработчика ID {Id}", id);
            }
            return NoContent();
        }

        /// <summary>
        /// Восстановить разработчика из корзины
        /// </summary>
        [HttpPost("{id}/restore")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Restore(int id)
        {
            await _service.RestoreAsync(id);
            _logger.LogInformation("Агрегатор: Восстановлен разработчик ID {Id}", id);
            return NoContent();
        }

        /// <summary>
        /// Полная очистка таблицы разработчиков (Обслуживание)
        /// </summary>
        [HttpDelete("maintenance/clear")]
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
