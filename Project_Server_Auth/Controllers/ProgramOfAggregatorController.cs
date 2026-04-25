using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using pr_srv_names.Models.Errors;
using pr_srv_names.Pages.AGGREGATOR.ProgramOfAggregator.Dtos;
using pr_srv_names.Pages.AGGREGATOR.ProgramOfAggregator.Interfaces;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace pr_srv_names.Controllers
{
    /// <summary>
    /// Контроллер для управления программами агрегатора (центр вселенной Aurora)
    /// </summary>
    [ApiController]
    [Route("api/v1/aggregator/programs")]
    [Produces("application/json")]
    [EnableRateLimiting("DefaultPolicy")]
    public class ProgramOfAggregatorController : ControllerBase
    {
        private readonly IProgramOfAggregatorService _service;
        private readonly ILogger<ProgramOfAggregatorController> _logger;

        public ProgramOfAggregatorController(
            IProgramOfAggregatorService service,
            ILogger<ProgramOfAggregatorController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region Program Endpoints

        [HttpGet]
        [ProducesResponseType(typeof(ProgramOfAggregatorPagedResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged([FromQuery] ProgramOfAggregatorPageRequestDto request)
        {
            request ??= new ProgramOfAggregatorPageRequestDto();
            var result = await _service.GetPagedAsync(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProgramOfAggregatorDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] ProgramOfAggregatorCreateDto request)
        {
            var id = await _service.CreateAsync(request);
            _logger.LogInformation("Агрегатор: Создана программа {Name} (ID: {Id})", request.CanonicalName, id);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, [FromBody] ProgramOfAggregatorUpdateDto request)
        {
            request.Id = id;
            await _service.UpdateAsync(request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id, [FromQuery] bool hardDelete = false)
        {
            if (hardDelete) await _service.HardDeleteAsync(id);
            else await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            await _service.RestoreAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Полная очистка таблицы программ (Обслуживание)
        /// </summary>
        [HttpPost("maintenance/clear")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ClearAll()
        {
            await _service.ClearAllAsync();
            return NoContent();
        }

        /// <summary>
        /// Сидинг данных из JSON (Обслуживание)
        /// </summary>
        [HttpPost("maintenance/seed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Seed()
        {
            await _service.SeedFromJsonAsync();
            return Ok(new { message = "Сидинг завершен." });
        }

        #endregion

        #region Version Endpoints

        [HttpGet("{programId}/versions")]
        public async Task<IActionResult> GetVersions(int programId)
        {
            var result = await _service.GetVersionsAsync(programId);
            return Ok(result);
        }

        [HttpGet("versions/{id}")]
        public async Task<IActionResult> GetVersionById(int id)
        {
            var result = await _service.GetVersionByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("versions")]
        public async Task<IActionResult> CreateVersion([FromBody] VersionOfAggregatorCreateDto request)
        {
            var id = await _service.CreateVersionAsync(request);
            return CreatedAtAction(nameof(GetVersionById), new { id }, id);
        }

        [HttpPut("versions/{id}")]
        public async Task<IActionResult> UpdateVersion(int id, [FromBody] VersionOfAggregatorUpdateDto request)
        {
            request.Id = id;
            await _service.UpdateVersionAsync(request);
            return NoContent();
        }

        [HttpDelete("versions/{id}")]
        public async Task<IActionResult> DeleteVersion(int id)
        {
            await _service.DeleteVersionAsync(id);
            return NoContent();
        }

        #endregion
    }
}
