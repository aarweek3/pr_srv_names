using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using pr_srv_names.Models.Errors;
using pr_srv_names.Pages.Aggregator.LanguageOfAggregator.Dtos;
using pr_srv_names.Pages.Aggregator.LanguageOfAggregator.Interfaces;

namespace pr_srv_names.Controllers
{
    /// <summary>
    /// Контроллер для управления языками агрегатора.
    /// </summary>
    [ApiController]
    [Route("api/v1/aggregator/languages")]
    [Produces("application/json")]
    [EnableRateLimiting("DefaultPolicy")]
    public class LanguageOfAggregatorController : ControllerBase
    {
        private readonly ILanguageOfAggregatorService _service;
        private readonly ILogger<LanguageOfAggregatorController> _logger;

        public LanguageOfAggregatorController(ILanguageOfAggregatorService service, ILogger<LanguageOfAggregatorController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Получить все языки агрегатора.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LanguageOfAggregatorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll([FromQuery] bool includeDisabled = true)
        {
            var result = await _service.GetAllLanguagesAsync(includeDisabled);
            return Ok(result);
        }

        /// <summary>
        /// Получить только доступные (активные) языки агрегатора.
        /// </summary>
        [HttpGet("available")]
        [ProducesResponseType(typeof(IEnumerable<LanguageOfAggregatorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAvailable()
        {
            var result = await _service.GetAvailableLanguagesAsync();
            return Ok(result);
        }

        /// <summary>
        /// Получить язык по идентификатору.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LanguageOfAggregatorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetLanguageByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Создать новый язык агрегатора.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(LanguageOfAggregatorDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateLanguageOfAggregatorDto request)
        {
            var result = await _service.CreateLanguageAsync(request);
            _logger.LogInformation("Aggregator language created with ID {Id}", result.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Обновить существующий язык агрегатора.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(LanguageOfAggregatorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLanguageOfAggregatorDto request)
        {
            var result = await _service.UpdateLanguageAsync(id, request);
            _logger.LogInformation("Aggregator language with ID {Id} updated", id);
            return Ok(result);
        }

        /// <summary>
        /// Удалить язык агрегатора.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteLanguageAsync(id);
            _logger.LogInformation("Aggregator language with ID {Id} deleted", id);
            return NoContent();
        }

        /// <summary>
        /// Установить язык основным по умолчанию.
        /// </summary>
        [HttpPost("{id}/default")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetDefault(int id)
        {
            await _service.SetDefaultLanguageAsync(id);
            _logger.LogInformation("Aggregator language with ID {Id} set as default", id);
            return NoContent();
        }

        /// <summary>
        /// Переключить статус активности языка.
        /// </summary>
        [HttpPatch("{id}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ToggleStatus(int id, [FromQuery] bool enabled)
        {
            await _service.ToggleLanguageStatusAsync(id, enabled);
            _logger.LogInformation("Aggregator language with ID {Id} status toggled to {Enabled}", id, enabled);
            return NoContent();
        }

        /// <summary>
        /// Полная очистка таблицы языков агрегатора (Hard Reset).
        /// </summary>
        [HttpDelete("hard-reset")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> HardReset()
        {
            await _service.HardResetAsync();
            _logger.LogWarning("Система: Выполнен Hard Reset таблицы языков агрегатора.");
            return NoContent();
        }

        /// <summary>
        /// Инициализация языков агрегатора из JSON файла.
        /// </summary>
        [HttpPost("initialize")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Initialize()
        {
            await _service.InitializeAsync();
            _logger.LogInformation("Система: Выполнена инициализация языков агрегатора.");
            return NoContent();
        }
    }
}
