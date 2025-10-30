using pr_srv_names.Pages.Sample.Intarfaces;
using pr_srv_names.Pages.Sample.Dtos;
using pr_srv_names.Models.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using DAL.Interfaces;
using DAL.Repositories.Interfaces;
using DAL.Models;

namespace pr_srv_names.Controllers
{
    /// <summary>
    /// Контроллер для управления языками
    /// </summary>
    [ApiController]
    [Route("api/v1/samples")]
    [Produces("application/json")]
    [EnableRateLimiting("DefaultPolicy")]
    public class SampleController : ControllerBase
    {
        private readonly ISampleService _service;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISampleRepository _sampleRepository;
        private readonly ILogger<SampleController> _logger;

        public SampleController(
            ISampleService service,
            IUnitOfWork unitOfWork,
            ISampleRepository sampleRepository,
            ILogger<SampleController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _sampleRepository = sampleRepository ?? throw new ArgumentNullException(nameof(sampleRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region Основные CRUD операции

        /// <summary>
        /// Получить все языки для селектора (в алфавитном порядке)
        /// </summary>
        [HttpGet("all")]
        [ProducesResponseType(typeof(IEnumerable<SampleDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllForSelector()
        {
            var correlationId = HttpContext.TraceIdentifier;
            var result = await _service.GetAllCategoriesAsync();
            _logger.LogInformation("Все языки успешно получены для селектора. Количество: {Count}. CorrelationId: {CorrelationId}",
                result.Count(), correlationId);

            return Ok(result);
        }

        /// <summary>
        /// Получить список языков с пагинацией и фильтрацией
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

            var result = await _service.GetAllCategoriesAsync(request);
            _logger.LogInformation("Список языков успешно получен. Страница: {PageNumber}, Размер страницы: {PageSize}, Всего: {Total}. CorrelationId: {CorrelationId}",
                request.PageNumber, request.PageSize, result.Total, correlationId);

            return Ok(result);
        }

        /// <summary>
        /// Получить язык по идентификатору
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
            _logger.LogInformation("Язык с ID {Id} успешно получен. CorrelationId: {CorrelationId}", id, correlationId);
            return Ok(result);
        }

        /// <summary>
        /// Создать новый язык
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
            _logger.LogInformation("Язык с ID {Id} успешно создан. CorrelationId: {CorrelationId}", result.Id, correlationId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Обновить существующий язык
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
            _logger.LogInformation("Язык с ID {Id} успешно обновлен. CorrelationId: {CorrelationId}", id, correlationId);
            return Ok(result);
        }

        /// <summary>
        /// Удалить язык
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
            _logger.LogInformation("Язык с ID {Id} успешно удален. CorrelationId: {CorrelationId}", id, correlationId);
            return NoContent();
        }

        #endregion

        #region Дополнительные методы

        /// <summary>
        /// Получить языки с описанием
        /// </summary>
        [HttpGet("with-description")]
        [ProducesResponseType(typeof(IEnumerable<SampleDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategoriesWithDescription()
        {
            var correlationId = HttpContext.TraceIdentifier;
            var result = await _service.GetCategoriesWithDescriptionAsync();
            _logger.LogInformation("Получено {Count} языков с описанием. CorrelationId: {CorrelationId}",
                result.Count(), correlationId);
            return Ok(result);
        }

        /// <summary>
        /// Проверить существование языка
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
                _logger.LogInformation("Язык с ID {Id} не существует. CorrelationId: {CorrelationId}", id, correlationId);
                return NotFound();
            }

            _logger.LogInformation("Язык с ID {Id} существует. CorrelationId: {CorrelationId}", id, correlationId);
            return Ok();
        }

        #endregion

        #region Control методы (упрощенные CRUD)

        /// <summary>
        /// Получить все языки для контрола (упрощенная версия)
        /// </summary>
        [HttpGet("control/all")]
        [ProducesResponseType(typeof(IEnumerable<SampleControlDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllControl()
        {
            var samples = await _sampleRepository.GetAllAsync();
            var dtos = samples.Select(r => new SampleControlDto { Id = r.Id, Name = r.Name }).ToList();
            return Ok(dtos);
        }

        /// <summary>
        /// Создать язык (упрощенная версия для контрола)
        /// </summary>
        [HttpPost("control")]
        [ProducesResponseType(typeof(SampleControlDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateControl([FromBody] SampleControlCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest("Имя языка не может быть пустым.");
            }

            var isUnique = await _sampleRepository.IsSampleNameUniqueAsync(dto.Name);
            if (!isUnique)
            {
                return Conflict("Язык с таким именем уже существует.");
            }

            var newSample = new Sample { Name = dto.Name };
            await _sampleRepository.AddAsync(newSample);
            await _unitOfWork.SaveChangesAsync();

            var createdDto = new SampleControlDto { Id = newSample.Id, Name = newSample.Name };
            return CreatedAtAction(nameof(GetAllControl), new { id = createdDto.Id }, createdDto);
        }

        #endregion
    }
}