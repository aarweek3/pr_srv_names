using pr_srv_names.Pages.Sample.Interfaces;
using pr_srv_names.Pages.Sample.Dtos;
using pr_srv_names.Models.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace pr_srv_names.Controllers
{
    /// <summary>
    /// Контроллер для управления Sample
    /// </summary>
    [ApiController]
    [Route("api/v1/samples")]
    [Produces("application/json")]
    [EnableRateLimiting("DefaultPolicy")]
    public class SampleController : ControllerBase
    {
        private readonly ISampleService _service;
        private readonly ILogger<SampleController> _logger;

        public SampleController(
            ISampleService service,
            ILogger<SampleController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region Основные CRUD операции

        /// <summary>
        /// Получить все Sample для селектора (в алфавитном порядке)
        /// </summary>
        [HttpGet("all")]
        [ProducesResponseType(typeof(IEnumerable<SampleDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllForSelector()
        {
            var correlationId = HttpContext.TraceIdentifier;
            var result = await _service.GetAllSamplesAsync();
            _logger.LogInformation("Все Sample успешно получены для селектора. Количество: {Count}. CorrelationId: {CorrelationId}",
                result.Count(), correlationId);

            return Ok(result);
        }

        /// <summary>
        /// Получить список Sample с пагинацией и фильтрацией
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(SamplePagedResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll([FromQuery] SamplePageRequestDto request)
        {
            var correlationId = HttpContext.TraceIdentifier;
            request ??= new SamplePageRequestDto();

            var result = await _service.GetAllSamplesAsync(request);
            _logger.LogInformation("Список Sample успешно получен. Страница: {PageNumber}, Размер страницы: {PageSize}, Всего: {Total}. CorrelationId: {CorrelationId}",
                request.PageNumber, request.PageSize, result.Total, correlationId);

            return Ok(result);
        }

        /// <summary>
        /// Получить Sample по идентификатору
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SampleDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            var correlationId = HttpContext.TraceIdentifier;
            var result = await _service.GetSampleByIdAsync(id);
            _logger.LogInformation("Sample с ID {Id} успешно получен. CorrelationId: {CorrelationId}", id, correlationId);
            return Ok(result);
        }

        /// <summary>
        /// Создать новый Sample
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(SampleDetailDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] SampleCreateRequestDto request)
        {
            var correlationId = HttpContext.TraceIdentifier;
            var result = await _service.CreateSampleAsync(request);
            _logger.LogInformation("Sample с ID {Id} успешно создан. CorrelationId: {CorrelationId}", result.Id, correlationId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Обновить существующий Sample
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SampleDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] SampleUpdateRequestDto request)
        {
            var correlationId = HttpContext.TraceIdentifier;
            var result = await _service.UpdateSampleAsync(id, request);
            _logger.LogInformation("Sample с ID {Id} успешно обновлен. CorrelationId: {CorrelationId}", id, correlationId);
            return Ok(result);
        }

        /// <summary>
        /// Удалить Sample
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var correlationId = HttpContext.TraceIdentifier;
            await _service.DeleteSampleAsync(id);
            _logger.LogInformation("Sample с ID {Id} успешно удален. CorrelationId: {CorrelationId}", id, correlationId);
            return NoContent();
        }

        #endregion

        #region Дополнительные методы

        /// <summary>
        /// Получить Sample с описанием
        /// </summary>
        [HttpGet("with-description")]
        [ProducesResponseType(typeof(IEnumerable<SampleDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSamplesWithDescription()
        {
            var correlationId = HttpContext.TraceIdentifier;
            var result = await _service.GetSamplesWithDescriptionAsync();
            _logger.LogInformation("Получено {Count} Sample с описанием. CorrelationId: {CorrelationId}",
                result.Count(), correlationId);
            return Ok(result);
        }

        /// <summary>
        /// Проверить существование Sample
        /// </summary>
        [HttpHead("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Exists(int id)
        {
            var correlationId = HttpContext.TraceIdentifier;
            var exists = await _service.SampleExistsAsync(id);

            if (!exists)
            {
                _logger.LogInformation("Sample с ID {Id} не существует. CorrelationId: {CorrelationId}", id, correlationId);
                return NotFound();
            }

            _logger.LogInformation("Sample с ID {Id} существует. CorrelationId: {CorrelationId}", id, correlationId);
            return Ok();
        }

        #endregion

        #region Control методы (упрощенные CRUD)

        /// <summary>
        /// Получить все Sample для контрола (упрощенная версия)
        /// </summary>
        [HttpGet("control/all")]
        [ProducesResponseType(typeof(IEnumerable<SampleControlDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllControl()
        {
            var result = await _service.GetAllControlAsync();
            return Ok(result);
        }

        /// <summary>
        /// Создать Sample (упрощенная версия для контрола)
        /// </summary>
        [HttpPost("control")]
        [ProducesResponseType(typeof(SampleControlDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateControl([FromBody] SampleControlCreateDto dto)
        {
            var result = await _service.CreateControlAsync(dto);
            return CreatedAtAction(nameof(GetAllControl), new { id = result.Id }, result);
        }

        #endregion
    }
}
