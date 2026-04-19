using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using pr_srv_names.Models.Errors;
using pr_srv_names.Pages.LanguageApp.Dtos;
using pr_srv_names.Pages.LanguageApp.Interfaces;

namespace pr_srv_names.Controllers
{
    [ApiController]
    [Route("api/v1/languages-app")]
    [Produces("application/json")]
    [EnableRateLimiting("DefaultPolicy")]
    public class LanguagesAppController : ControllerBase
    {
        private readonly ILanguageAppService _service;
        private readonly ILogger<LanguagesAppController> _logger;

        public LanguagesAppController(ILanguageAppService service, ILogger<LanguagesAppController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LanguageAppDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll([FromQuery] bool includeDisabled = true)
        {
            var result = await _service.GetAllLanguagesAsync(includeDisabled);
            return Ok(result);
        }

        [HttpGet("available")]
        [ProducesResponseType(typeof(IEnumerable<LanguageAppDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAvailable()
        {
            var result = await _service.GetAvailableLanguagesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LanguageAppDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetLanguageByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(LanguageAppDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateLanguageAppDto request)
        {
            var result = await _service.CreateLanguageAsync(request);
            _logger.LogInformation("Language created with ID {Id}", result.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(LanguageAppDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLanguageAppDto request)
        {
            var result = await _service.UpdateLanguageAsync(id, request);
            _logger.LogInformation("Language with ID {Id} updated", id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteLanguageAsync(id);
            _logger.LogInformation("Language with ID {Id} deleted", id);
            return NoContent();
        }

        [HttpPost("{id}/default")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetDefault(int id)
        {
            await _service.SetDefaultLanguageAsync(id);
            _logger.LogInformation("Language with ID {Id} set as default", id);
            return NoContent();
        }

        [HttpPatch("{id}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ToggleStatus(int id, [FromQuery] bool enabled)
        {
            await _service.ToggleLanguageStatusAsync(id, enabled);
            _logger.LogInformation("Language with ID {Id} status toggled to {Enabled}", id, enabled);
            return NoContent();
        }

        [HttpDelete("hard-reset")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> HardReset()
        {
            await _service.HardResetAsync();
            _logger.LogWarning("Система: Выполнен Hard Reset таблицы языков.");
            return NoContent();
        }

        [HttpPost("initialize")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Initialize()
        {
            await _service.InitializeAsync();
            _logger.LogInformation("Система: Выполнена инициализация языков.");
            return NoContent();
        }
    }
}
