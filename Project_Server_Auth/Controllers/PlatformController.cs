using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using pr_srv_names.Models.Errors;
using pr_srv_names.Pages.Platform.Dtos;
using pr_srv_names.Pages.Platform.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;

namespace pr_srv_names.Controllers
{
    /// <summary>
    /// Контроллер для управления платформами (многоязычность + SEO)
    /// </summary>
    [ApiController]
    [Route("api/v1/platforms")]
    [Produces("application/json")]
    [EnableRateLimiting("DefaultPolicy")]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformService _service;
        private readonly ILogger<PlatformController> _logger;

        public PlatformController(IPlatformService service, ILogger<PlatformController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PlatformPagedResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged([FromQuery] PlatformPageRequestDto request)
        {
            request ??= new PlatformPageRequestDto();
            var result = await _service.GetPagedAsync(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PlatformDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PlatformDetailDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] PlatformCreateDto request)
        {
            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PlatformDetailDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, [FromBody] PlatformUpdateDto request)
        {
            request.Id = id;
            var result = await _service.UpdateAsync(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
