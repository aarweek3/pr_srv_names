/*
 * HealthEnhancedController — это кастомное расширение для более детальной диагностики, 
мониторинга производительности и проверок с учётом аутентификации. Подходит для администраторов и внутренних инструментов.

 */
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pr_srv_names.Pages.Health.Models;
using pr_srv_names.Pages.Health.Services;

namespace pr_srv_names.Controllers
{
    [ApiController]
    [Route("api/health-enhanced")] // Явный путь
    public class HealthEnhancedController : ControllerBase
    {
        private readonly IHealthCheckEnhancedService _healthService;
        private readonly ILogger<HealthEnhancedController> _logger;
        public HealthEnhancedController(IHealthCheckEnhancedService healthService, ILogger<HealthEnhancedController> logger)
        {
            _healthService = healthService;
            _logger = logger;
        }
        [HttpGet("detailed")]
        [AllowAnonymous]
        public async Task<ActionResult<DetailedHealthResponse>> GetDetailedStatus()
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                string? token = authHeader.StartsWith("Bearer ") ? authHeader.Substring(7) : null;
                var status = await _healthService.GetDetailedHealthAsync(token);
                return Ok(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting enhanced health status");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }
        [HttpGet("metrics")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServerMetrics>> GetMetrics()
        {
            var metrics = await _healthService.GetServerMetricsAsync();
            return Ok(metrics);
        }
    }
}
