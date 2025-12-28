/*
 * HealthCheckController — это стандартный, лёгкий и рекомендуемый подход для health checks в ASP.NET Core.
 * Подходит для автоматических проверок оркестраторами (Kubernetes, Docker, Azure и т.д.).
 */
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace pr_srv_names.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class HealthCheckController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;
        private readonly ILogger<HealthCheckController> _logger;

        public HealthCheckController(
            HealthCheckService healthCheckService,
            ILogger<HealthCheckController> logger)
        {
            _healthCheckService = healthCheckService;
            _logger = logger;
        }

        /// <summary>
        /// Полная проверка здоровья системы (все проверки)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(HealthCheckResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(HealthCheckResponse), StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> Get()
        {
            var report = await _healthCheckService.CheckHealthAsync();
            return FormatHealthResponse(report);
        }

        /// <summary>
        /// Проверка готовности (только проверки с тегом 'ready')
        /// </summary>
        [HttpGet("ready")]
        [ProducesResponseType(typeof(HealthCheckResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(HealthCheckResponse), StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetReady()
        {
            var report = await _healthCheckService.CheckHealthAsync(
                predicate: check => check.Tags.Contains("ready"));

            return FormatHealthResponse(report);
        }

        /// <summary>
        /// Проверка жизнеспособности (минимальная проверка)
        /// </summary>
        [HttpGet("live")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetLive()
        {
            // Live check обычно не выполняет реальных проверок
            return Ok(new HealthCheckResponse
            {
                Status = HealthStatus.Healthy.ToString(),
                Checks = Array.Empty<HealthCheckItem>()
            });
        }

        private IActionResult FormatHealthResponse(HealthReport report)
        {
            var response = new HealthCheckResponse
            {
                Status = report.Status.ToString(),
                Checks = report.Entries.Select(e => new HealthCheckItem
                {
                    Name = e.Key,
                    Status = e.Value.Status.ToString(),
                    Duration = e.Value.Duration.TotalMilliseconds,
                    Description = e.Value.Description
                })
            };

            _logger.LogInformation("Health check completed with status: {Status}", report.Status);

            return report.Status == HealthStatus.Healthy
                ? Ok(response)
                : StatusCode(StatusCodes.Status503ServiceUnavailable, response);
        }
    }

    // Модели для ответа
    public class HealthCheckResponse
    {
        public string Status { get; set; }
        public IEnumerable<HealthCheckItem> Checks { get; set; }
    }

    public class HealthCheckItem
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public double Duration { get; set; }
        public string Description { get; set; }
    }
}