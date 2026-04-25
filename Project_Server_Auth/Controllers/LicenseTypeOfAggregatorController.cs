using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using pr_srv_names.Models.Errors;
using pr_srv_names.Pages.AGGREGATOR.LicenseTypeOfAggregator.Dtos;
using pr_srv_names.Pages.AGGREGATOR.LicenseTypeOfAggregator.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;

namespace pr_srv_names.Controllers
{
    /// <summary>
    /// Контроллер для управления типами лицензий агрегатора (с поддержкой SEO и локализаций)
    /// </summary>
    [ApiController]
    [Route("api/v1/aggregator/license-types")]
    [Produces("application/json")]
    [EnableRateLimiting("DefaultPolicy")]
    public class LicenseTypeOfAggregatorController : ControllerBase
    {
        private readonly ILicenseTypeOfAggregatorService _service;
        private readonly ILogger<LicenseTypeOfAggregatorController> _logger;

        public LicenseTypeOfAggregatorController(
            ILicenseTypeOfAggregatorService service,
            ILogger<LicenseTypeOfAggregatorController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Получить список типов лицензий с пагинацией и SEO данными
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(LicenseTypeOfAggregatorPagedResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged([FromQuery] LicenseTypeOfAggregatorPageRequestDto request)
        {
            request ??= new LicenseTypeOfAggregatorPageRequestDto();
            var result = await _service.GetPagedAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Получить детальную информацию о типе лицензии
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LicenseTypeOfAggregatorDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Создать новый тип лицензии с поддержкой SEO
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(LicenseTypeOfAggregatorDetailDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] LicenseTypeOfAggregatorCreateDto request)
        {
            var result = await _service.CreateAsync(request);
            _logger.LogInformation("Агрегатор: Создан новый тип лицензии {Name} (ID: {Id})", result.CanonicalName, result.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Обновить тип лицензии и его локализации/SEO
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(LicenseTypeOfAggregatorDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int id, [FromBody] LicenseTypeOfAggregatorUpdateDto request)
        {
            request.Id = id;
            var result = await _service.UpdateAsync(request);
            _logger.LogInformation("Агрегатор: Обновлен тип лицензии ID {Id}", id);
            return Ok(result);
        }

        /// <summary>
        /// Удалить тип лицензии
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, [FromQuery] bool isHard = false)
        {
            if (isHard)
            {
                await _service.HardDeleteAsync(id);
                _logger.LogCritical("Агрегатор: ПОЛНОЕ УДАЛЕНИЕ типа лицензии ID {Id}", id);
            }
            else
            {
                await _service.DeleteAsync(id);
                _logger.LogWarning("Агрегатор: Мягкое удаление типа лицензии ID {Id}", id);
            }
            return NoContent();
        }

        /// <summary>
        /// Восстановить тип лицензии из корзины
        /// </summary>
        [HttpPost("{id}/restore")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Restore(int id)
        {
            await _service.RestoreAsync(id);
            _logger.LogInformation("Агрегатор: Восстановлен тип лицензии ID {Id}", id);
            return NoContent();
        }

        /// <summary>
        /// Полная очистка таблицы (Обслуживание)
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
