using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using pr_srv_names.Models.Errors;
using pr_srv_names.Pages.SampleMain.Dtos;
using pr_srv_names.Pages.SampleMain.Interfaces;

namespace pr_srv_names.Controllers
{
    /// <summary>
    /// Контроллер для управления многоязычными записями SampleMain
    /// </summary>
    [ApiController]
    [Route("api/v1/samples-main")]
    [Produces("application/json")]
    [EnableRateLimiting("DefaultPolicy")]
    public class SampleMainController : ControllerBase
    {
        private readonly ISampleMainService _service;
        private readonly ILogger<SampleMainController> _logger;

        public SampleMainController(
            ISampleMainService service,
            ILogger<SampleMainController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Получить список SampleMain с пагинацией, фильтрацией и локализацией
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(SampleMainPagedResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPaged([FromQuery] SampleMainPageRequestDto request)
        {
            var correlationId = HttpContext.TraceIdentifier;
            request ??= new SampleMainPageRequestDto();

            var result = await _service.GetPagedAsync(request);
            
            _logger.LogInformation("Список SampleMain получен. Найдено: {Total}. CorrelationId: {CorrelationId}", 
                result.Total, correlationId);
            
            return Ok(result);
        }

        /// <summary>
        /// Получить детальную информацию о SampleMain (со всеми переводами)
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SampleMainDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var correlationId = HttpContext.TraceIdentifier;
            var result = await _service.GetByIdAsync(id);
            
            _logger.LogInformation("Детали SampleMain ID {Id} получены. CorrelationId: {CorrelationId}", id, correlationId);
            return Ok(result);
        }

        /// <summary>
        /// Создать новую многоязычную запись
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(SampleMainDetailDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] SampleMainCreateRequestDto request)
        {
            var correlationId = HttpContext.TraceIdentifier;
            var result = await _service.CreateAsync(request);
            
            _logger.LogInformation("SampleMain создана. ID: {Id}. CorrelationId: {CorrelationId}", result.Id, correlationId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Обновить существующую запись и её переводы
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SampleMainDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int id, [FromBody] SampleMainUpdateRequestDto request)
        {
            var correlationId = HttpContext.TraceIdentifier;
            request.Id = id;
            var result = await _service.UpdateAsync(request);
            
            _logger.LogInformation("SampleMain ID {Id} обновлена. CorrelationId: {CorrelationId}", id, correlationId);
            return Ok(result);
        }

        /// <summary>
        /// Удалить запись и все связанные переводы
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var correlationId = HttpContext.TraceIdentifier;
            await _service.DeleteAsync(id);
            
            _logger.LogInformation("SampleMain ID {Id} удалена. CorrelationId: {CorrelationId}", id, correlationId);
            return NoContent();
        }

        /// <summary>
        /// Проверить уникальность технического имени
        /// </summary>
        [HttpGet("check-name")]
        public async Task<IActionResult> CheckName([FromQuery] string name, [FromQuery] int? excludeId)
        {
            var isUnique = await _service.IsNameUniqueAsync(name, excludeId);
            return Ok(new { isUnique });
        }
    }
}
