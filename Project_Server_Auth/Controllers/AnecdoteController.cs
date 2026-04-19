using pr_srv_names.Pages.Anecdote.Interfaces;
using pr_srv_names.Pages.Anecdote.Dtos;
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
    /// Контроллер для управления анекдотами
    /// </summary>
    [ApiController]
    [Route("api/v1/anecdotes")]
    [Produces("application/json")]
    [EnableRateLimiting("DefaultPolicy")]
    public class AnecdoteController : ControllerBase
    {
        private readonly IAnecdoteService _service;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAnecdoteRepository _anecdoteRepository;
        private readonly ILogger<AnecdoteController> _logger;

        public AnecdoteController(
            IAnecdoteService service,
            IUnitOfWork unitOfWork,
            IAnecdoteRepository anecdoteRepository,
            ILogger<AnecdoteController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _anecdoteRepository = anecdoteRepository ?? throw new ArgumentNullException(nameof(anecdoteRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region Основные CRUD операции

        /// <summary>
        /// Получить все анекдоты для селектора
        /// </summary>
        [HttpGet("all")]
        [ProducesResponseType(typeof(IEnumerable<AnecdoteDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllForSelector()
        {
            var correlationId = HttpContext.TraceIdentifier;
            var result = await _service.GetAllAnecdotesAsync();
            _logger.LogInformation("Все анекдоты успешно получены. Количество: {Count}. CorrelationId: {CorrelationId}",
                result.Count(), correlationId);

            return Ok(result);
        }

        /// <summary>
        /// Получить список анекдотов с пагинацией и фильтрацией
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(AnecdotePagedResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll([FromQuery] AnecdotePageRequestDto request)
        {
            var correlationId = HttpContext.TraceIdentifier;
            request ??= new AnecdotePageRequestDto();

            var result = await _service.GetAllAnecdotesAsync(request);
            _logger.LogInformation(
                "Список анекдотов успешно получен. Страница: {PageNumber}, Размер: {PageSize}, Всего: {Total}. CorrelationId: {CorrelationId}",
                request.PageNumber, request.PageSize, result.Total, correlationId);

            return Ok(result);
        }

        /// <summary>
        /// Получить анекдот по идентификатору
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AnecdoteDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            var correlationId = HttpContext.TraceIdentifier;
            var result = await _service.GetAnecdoteByIdAsync(id);
            _logger.LogInformation("Анекдот с ID {Id} успешно получен. CorrelationId: {CorrelationId}", id,
                correlationId);
            return Ok(result);
        }

        /// <summary>
        /// Создать новый анекдот
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(AnecdoteDetailDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] AnecdoteCreateRequestDto request)
        {
            var correlationId = HttpContext.TraceIdentifier;
            var result = await _service.CreateAnecdoteAsync(request);
            _logger.LogInformation("Анекдот с ID {Id} успешно создан. CorrelationId: {CorrelationId}", result.Id,
                correlationId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Обновить существующий анекдот
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(AnecdoteDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] AnecdoteUpdateRequestDto request)
        {
            var correlationId = HttpContext.TraceIdentifier;
            var result = await _service.UpdateAnecdoteAsync(id, request);
            _logger.LogInformation("Анекдот с ID {Id} успешно обновлен. CorrelationId: {CorrelationId}", id,
                correlationId);
            return Ok(result);
        }

        /// <summary>
        /// Удалить анекдот
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
            await _service.DeleteAnecdoteAsync(id);
            _logger.LogInformation("Анекдот с ID {Id} успешно удален. CorrelationId: {CorrelationId}", id,
                correlationId);
            return NoContent();
        }

        #endregion

        #region Дополнительные методы

        /// <summary>
        /// Получить анекдоты для конкретного имени
        /// </summary>
        [HttpGet("by-name/{nameMainId}")]
        [ProducesResponseType(typeof(IEnumerable<AnecdoteDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByNameMainId(int nameMainId)
        {
            var correlationId = HttpContext.TraceIdentifier;
            var result = await _service.GetAnecdotesByNameMainIdAsync(nameMainId);
            _logger.LogInformation("Получено {Count} анекдотов для имени {NameMainId}. CorrelationId: {CorrelationId}",
                result.Count(), nameMainId, correlationId);
            return Ok(result);
        }

        /// <summary>
        /// Получить анекдоты на конкретном языке
        /// </summary>
        [HttpGet("by-language/{languageId}")]
        [ProducesResponseType(typeof(IEnumerable<AnecdoteDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByLanguageId(int languageId)
        {
            var correlationId = HttpContext.TraceIdentifier;
            var result = await _service.GetAnecdotesByLanguageIdAsync(languageId);
            _logger.LogInformation("Получено {Count} анекдотов для языка {LanguageId}. CorrelationId: {CorrelationId}",
                result.Count(), languageId, correlationId);
            return Ok(result);
        }

        /// <summary>
        /// Получить анекдоты по имени и языку
        /// </summary>
        [HttpGet("by-name-and-language/{nameMainId}/{languageId}")]
        [ProducesResponseType(typeof(IEnumerable<AnecdoteDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByNameAndLanguage(int nameMainId, int languageId)
        {
            var correlationId = HttpContext.TraceIdentifier;
            var result = await _service.GetAnecdotesByNameAndLanguageAsync(nameMainId, languageId);
            _logger.LogInformation(
                "Получено {Count} анекдотов для имени {NameMainId} и языка {LanguageId}. CorrelationId: {CorrelationId}",
                result.Count(), nameMainId, languageId, correlationId);
            return Ok(result);
        }

        /// <summary>
        /// Проверить существование анекдота
        /// </summary>
        [HttpHead("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Exists(int id)
        {
            var correlationId = HttpContext.TraceIdentifier;
            var exists = await _service.AnecdoteExistsAsync(id);

            if (!exists)
            {
                _logger.LogInformation("Анекдот с ID {Id} не существует. CorrelationId: {CorrelationId}", id,
                    correlationId);
                return NotFound();
            }

            _logger.LogInformation("Анекдот с ID {Id} существует. CorrelationId: {CorrelationId}", id, correlationId);
            return Ok();
        }

        #endregion

        #region Control методы (упрощенные CRUD)

        /// <summary>
        /// Получить все анекдоты для контрола (упрощенная версия)
        /// </summary>
        [HttpGet("control/all")]
        [ProducesResponseType(typeof(IEnumerable<AnecdoteControlDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllControl()
        {
            var anecdotes = await _anecdoteRepository.GetAllAsync();
            var dtos = anecdotes.Select(a => new AnecdoteControlDto { Id = a.Id, Name = a.Name }).ToList();
            return Ok(dtos);
        }

        /// <summary>
        /// Создать анекдот (упрощенная версия для контрола)
        /// </summary>
        [HttpPost("control")]
        [ProducesResponseType(typeof(AnecdoteControlDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateControl([FromBody] AnecdoteControlCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest("Название анекдота не может быть пустым.");
            }

            var newAnecdote = new Anecdote
            {
                Name = dto.Name,
                NameMainId = dto.NameMainId,
                LanguageId = dto.LanguageId
            };

            await _anecdoteRepository.AddAsync(newAnecdote);
            await _unitOfWork.SaveChangesAsync();

            var createdDto = new AnecdoteControlDto { Id = newAnecdote.Id, Name = newAnecdote.Name };
            return CreatedAtAction(nameof(GetAllControl), new { id = createdDto.Id }, createdDto);
        }

        #endregion
    }
}