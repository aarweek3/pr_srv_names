using pr_srv_names.Exceptions;
using pr_srv_names.Models;
using AutoMapper;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using pr_srv_names.Pages.Language.Dtos;
using pr_srv_names.Pages.Language.Intarfaces;
using pr_srv_names.Models.Errors;

namespace pr_srv_names.Controllers
{
    [ApiController]
    [Route("api/v1/languages")]
    [Produces("application/json")]
    [EnableRateLimiting("DefaultPolicy")]
    public class LanguageController : ControllerBase
    {
        private readonly ILanguageService _service;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILanguageRepository _languageRepository;
        private readonly ILogger<LanguageController> _logger;

        public LanguageController(
            ILanguageService service,
            IUnitOfWork unitOfWork,
            ILanguageRepository languageRepository,
            ILogger<LanguageController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _languageRepository = languageRepository ?? throw new ArgumentNullException(nameof(languageRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region Основные CRUD операции

        [HttpGet("all")]
        [ProducesResponseType(typeof(IEnumerable<LanguageDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllForSelector()
        {
            var correlationId = HttpContext.TraceIdentifier;
            var result = await _service.GetAllCategoriesAsync();
            _logger.LogInformation("Все языки успешно получены для селектора. Количество: {Count}. CorrelationId: {CorrelationId}",
                result.Count(), correlationId);
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(LanguagePagedResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll([FromQuery] LanguagePageRequestDto request)
        {
            var correlationId = HttpContext.TraceIdentifier;
            request ??= new LanguagePageRequestDto();
            var result = await _service.GetAllCategoriesAsync(request);
            _logger.LogInformation("Список языков успешно получен. Страница: {PageNumber}, Размер страницы: {PageSize}, Всего: {Total}. CorrelationId: {CorrelationId}",
                request.PageNumber, request.PageSize, result.Total, correlationId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LanguageDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            var correlationId = HttpContext.TraceIdentifier;
            var result = await _service.GetLanguageByIdAsync(id);
            _logger.LogInformation("Язык с ID {Id} успешно получен. CorrelationId: {CorrelationId}", id, correlationId);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(LanguageDetailDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] LanguageCreateRequestDto request)
        {
            var correlationId = HttpContext.TraceIdentifier;
            var result = await _service.CreateLanguageAsync(request);
            _logger.LogInformation("Язык с ID {Id} успешно создан. CorrelationId: {CorrelationId}", result.Id, correlationId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(LanguageDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] LanguageUpdateRequestDto request)
        {
            var correlationId = HttpContext.TraceIdentifier;
            var result = await _service.UpdateLanguageAsync(id, request);
            _logger.LogInformation("Язык с ID {Id} успешно обновлен. CorrelationId: {CorrelationId}", id, correlationId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var correlationId = HttpContext.TraceIdentifier;
            await _service.DeleteLanguageAsync(id);
            _logger.LogInformation("Язык с ID {Id} успешно удален. CorrelationId: {CorrelationId}", id, correlationId);
            return NoContent();
        }
        #endregion
        #region Дополнительные методы

        [HttpGet("with-description")]
        [ProducesResponseType(typeof(IEnumerable<LanguageDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategoriesWithDescription()
        {
            var correlationId = HttpContext.TraceIdentifier;
            var result = await _service.GetCategoriesWithDescriptionAsync();
            _logger.LogInformation("Получено {Count} языков с описанием. CorrelationId: {CorrelationId}",
                result.Count(), correlationId);
            return Ok(result);
        }

        [HttpHead("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Exists(int id)
        {
            var correlationId = HttpContext.TraceIdentifier;
            var exists = await _service.LanguageExistsAsync(id);
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

        [HttpGet("control/all")]
        [ProducesResponseType(typeof(IEnumerable<LanguageControlDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllControl()
        {
            var languages = await _languageRepository.GetAllAsync();
            var dtos = languages.Select(r => new LanguageControlDto { Id = r.Id, Code = r.Code, Name = r.Name }).ToList();
            return Ok(dtos);
        }

        [HttpPost("control")]
        [ProducesResponseType(typeof(LanguageControlDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateControl([FromBody] LanguageControlCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Code))
            {
                return BadRequest("Имя и код языка не могут быть пустыми.");
            }

            var isNameUnique = await _languageRepository.IsLanguageNameUniqueAsync(dto.Name);
            if (!isNameUnique)
            {
                return Conflict("Язык с таким именем уже существует.");
            }

            var isCodeUnique = await _languageRepository.IsLanguageCodeUniqueAsync(dto.Code);
            if (!isCodeUnique)
            {
                return Conflict("Язык с таким кодом уже существует.");
            }

            var newLanguage = new DAL.Models.Language
            {
                Code = dto.Code,
                Name = dto.Name,
                FlagCode = dto.FlagCode
            };
            await _languageRepository.AddAsync(newLanguage);
            await _unitOfWork.SaveChangesAsync();

            var createdDto = new LanguageControlDto { Id = newLanguage.Id, Code = newLanguage.Code, Name = newLanguage.Name };
            return CreatedAtAction(nameof(GetAllControl), new { id = createdDto.Id }, createdDto);
        }

        #endregion
    }
}