using pr_srv_names.Pages.NameMain.Intarfaces;
using pr_srv_names.Pages.NameMain.Dtos;
using pr_srv_names.Models.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using DAL.Interfaces;
using DAL.Repositories.Interfaces;
using DAL.Models;
using DAL.Models.NameModels;

namespace pr_srv_names.Controllers
{
    /// <summary>
    /// Контроллер для управления языками
    /// </summary>
    [ApiController]
    [Route("api/v1/namemains")]
    [Produces("application/json")]
    [EnableRateLimiting("DefaultPolicy")]
    public class NameMainController : ControllerBase
    {
        private readonly INameMainService _service;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INameMainRepository _namemainrepository;
        private readonly ILogger<NameMainController> _logger;

        public NameMainController(
            INameMainService service,
            IUnitOfWork unitOfWork,
            INameMainRepository namemainrepository,
            ILogger<NameMainController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _namemainrepository = namemainrepository ?? throw new ArgumentNullException(nameof(namemainrepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region Основные CRUD операции

        /// <summary>
        /// Получить все языки для селектора (в алфавитном порядке)
        /// </summary>
        [HttpGet("all")]
        [ProducesResponseType(typeof(IEnumerable<NameMainDetailDto>), StatusCodes.Status200OK)]
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
        [ProducesResponseType(typeof(NameMainPagedResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll([FromQuery] NameMainPageRequestDto request)
        {
            var correlationId = HttpContext.TraceIdentifier;
            request ??= new NameMainPageRequestDto();

            var result = await _service.GetAllCategoriesAsync(request);
            _logger.LogInformation("Список языков успешно получен. Страница: {PageNumber}, Размер страницы: {PageSize}, Всего: {Total}. CorrelationId: {CorrelationId}",
                request.PageNumber, request.PageSize, result.Total, correlationId);

            return Ok(result);
        }

        /// <summary>
        /// Получить язык по идентификатору
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NameMainDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            var correlationId = HttpContext.TraceIdentifier;
            var result = await _service.GetNameMainByIdAsync(id);
            _logger.LogInformation("Язык с ID {Id} успешно получен. CorrelationId: {CorrelationId}", id, correlationId);
            return Ok(result);
        }

        /// <summary>
        /// Создать новый язык
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(NameMainDetailDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] NameMainCreateRequestDto request)
        {
            var correlationId = HttpContext.TraceIdentifier;
            var result = await _service.CreateNameMainAsync(request);
            _logger.LogInformation("Язык с ID {Id} успешно создан. CorrelationId: {CorrelationId}", result.Id, correlationId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Обновить существующий язык
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(NameMainDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] NameMainUpdateRequestDto request)
        {
            var correlationId = HttpContext.TraceIdentifier;
            var result = await _service.UpdateNameMainAsync(id, request);
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
            await _service.DeleteNameMainAsync(id);
            _logger.LogInformation("Язык с ID {Id} успешно удален. CorrelationId: {CorrelationId}", id, correlationId);
            return NoContent();
        }

        #endregion

        #region Дополнительные методы

        /// <summary>
        /// Получить языки с описанием
        /// </summary>
        [HttpGet("with-description")]
        [ProducesResponseType(typeof(IEnumerable<NameMainDetailDto>), StatusCodes.Status200OK)]
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
            var exists = await _service.NameMainExistsAsync(id);

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
        [ProducesResponseType(typeof(IEnumerable<NameMainControlDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllControl()
        {
            var namemains = await _namemainrepository.GetAllAsync();
            var dtos = namemains.Select(r => new NameMainControlDto { Id = r.Id, Name = r.Name }).ToList();
            return Ok(dtos);
        }

        /// <summary>
        /// Создать язык (упрощенная версия для контрола)
        /// </summary>
        [HttpPost("control")]
        [ProducesResponseType(typeof(NameMainControlDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateControl([FromBody] NameMainControlCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest("Имя языка не может быть пустым.");
            }

            var isUnique = await _namemainrepository.IsNameMainNameUniqueAsync(dto.Name);
            if (!isUnique)
            {
                return Conflict("Язык с таким именем уже существует.");
            }

            var newNameMain = new NameMain { Name = dto.Name };
            await _namemainrepository.AddAsync(newNameMain);
            await _unitOfWork.SaveChangesAsync();

            var createdDto = new NameMainControlDto { Id = newNameMain.Id, Name = newNameMain.Name };
            return CreatedAtAction(nameof(GetAllControl), new { id = createdDto.Id }, createdDto);
        }

        #endregion
    }
}