using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using pr_srv_names.Models.Errors;
using pr_srv_names.Pages.SampleMainSeo.Dtos;
using pr_srv_names.Pages.SampleMainSeo.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;

namespace pr_srv_names.Controllers
{
    /// <summary>
    /// Контроллер для управления многоязычными записями SampleMainSeo (Образцовая модель с универсальным SEO)
    /// </summary>
    [ApiController]
    [Route("api/v1/samples-main-seo")]
    [Produces("application/json")]
    [EnableRateLimiting("DefaultPolicy")]
    public class SampleMainSeoController : ControllerBase
    {
        private readonly ISampleMainSeoService _service;
        private readonly ILogger<SampleMainSeoController> _logger;

        public SampleMainSeoController(
            ISampleMainSeoService service,
            ILogger<SampleMainSeoController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Получить список SampleMainSeo с пагинацией и SEO данными
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(SampleMainSeoPagedResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged([FromQuery] SampleMainSeoPageRequestDto request)
        {
            request ??= new SampleMainSeoPageRequestDto();
            var result = await _service.GetPagedAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Получить детальную информацию о SampleMainSeo (с SEO данными для каждого языка)
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SampleMainSeoDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Создать новую запись с универсальным SEO
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(SampleMainSeoDetailDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] SampleMainSeoCreateDto request)
        {
            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Обновить запись и её SEO данные
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SampleMainSeoDetailDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(int id, [FromBody] SampleMainSeoUpdateDto request)
        {
            request.Id = id;
            var result = await _service.UpdateAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Удалить запись (SEO данные удалятся автоматически каскадно)
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
