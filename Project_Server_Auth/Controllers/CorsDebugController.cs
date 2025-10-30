// Controllers/CorsDebugController.cs
using Microsoft.AspNetCore.Mvc;

namespace pr_srv_names.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CorsDebugController : ControllerBase
    {
        private readonly ILogger<CorsDebugController> _logger;

        public CorsDebugController(ILogger<CorsDebugController> logger)
        {
            _logger = logger;
        }

        [HttpGet("test")]
        public IActionResult TestCors()
        {
            var origin = Request.Headers["Origin"].FirstOrDefault();
            var userAgent = Request.Headers["User-Agent"].FirstOrDefault();
            var referer = Request.Headers["Referer"].FirstOrDefault();

            _logger.LogInformation("🔍 CORS Test Request:");
            _logger.LogInformation("   Origin: {Origin}", origin ?? "NOT SET");
            _logger.LogInformation("   Referer: {Referer}", referer ?? "NOT SET");
            _logger.LogInformation("   User-Agent: {UserAgent}", userAgent ?? "NOT SET");
            _logger.LogInformation("   Method: {Method}", Request.Method);
            _logger.LogInformation("   Path: {Path}", Request.Path);
            _logger.LogInformation("   QueryString: {QueryString}", Request.QueryString);

            // Выводим все заголовки
            _logger.LogInformation("📋 All Headers:");
            foreach (var header in Request.Headers)
            {
                _logger.LogInformation("   {Key}: {Value}", header.Key, string.Join(", ", header.Value));
            }

            // Выводим response headers
            _logger.LogInformation("📤 Response Headers:");
            foreach (var header in Response.Headers)
            {
                _logger.LogInformation("   {Key}: {Value}", header.Key, string.Join(", ", header.Value));
            }

            return Ok(new
            {
                message = "CORS test successful",
                timestamp = DateTime.UtcNow,
                origin = origin,
                method = Request.Method,
                path = Request.Path.Value,
                headers = Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToArray()),
                responseHeaders = Response.Headers.ToDictionary(h => h.Key, h => h.Value.ToArray())
            });
        }

        [HttpOptions("test")]
        public IActionResult PreflightTest()
        {
            var origin = Request.Headers["Origin"].FirstOrDefault();
            var requestMethod = Request.Headers["Access-Control-Request-Method"].FirstOrDefault();
            var requestHeaders = Request.Headers["Access-Control-Request-Headers"].FirstOrDefault();

            _logger.LogInformation("🚀 PREFLIGHT Request:");
            _logger.LogInformation("   Origin: {Origin}", origin ?? "NOT SET");
            _logger.LogInformation("   Requested Method: {Method}", requestMethod ?? "NOT SET");
            _logger.LogInformation("   Requested Headers: {Headers}", requestHeaders ?? "NOT SET");

            return Ok();
        }

        [HttpPost("test")]
        public IActionResult PostTest([FromBody] object data)
        {
            var origin = Request.Headers["Origin"].FirstOrDefault();

            _logger.LogInformation("📝 POST Test Request:");
            _logger.LogInformation("   Origin: {Origin}", origin ?? "NOT SET");
            _logger.LogInformation("   Content-Type: {ContentType}", Request.ContentType ?? "NOT SET");
            _logger.LogInformation("   Data: {Data}", data?.ToString() ?? "NULL");

            return Ok(new { message = "POST test successful", receivedData = data });
        }
    }
}