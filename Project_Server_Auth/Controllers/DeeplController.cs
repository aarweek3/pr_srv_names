using pr_srv_names.Supports.Deepl;

using Microsoft.AspNetCore.Mvc;
using pr_srv_names.Deepl;

namespace pr_srv_names.Controllers
{
    [ApiController]
    [Route("api/deepl")]
    public class DeeplController : ControllerBase
    {
        private readonly DeepLTranslationService _translationService;

        public DeeplController(DeepLTranslationService translationService)
        {
            _translationService = translationService;
        }

        [HttpPost("translate")]
        public async Task<IActionResult> Translate([FromBody] TranslateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Input))
            {
                return BadRequest(new DeepLTranslationResponse
                {
                    Success = false,
                    Body = string.Empty,
                    MessageDeep = "Входная строка пуста.",
                    StatusCode = null
                });
            }

            var result = await _translationService.TranslateAsync(request.Input, request.SourceLang, request.TargetLang);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("usage")]
        public async Task<IActionResult> GetUsage()
        {
            var result = await _translationService.GetUsageAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
public class TranslateRequest
{
    public string Input { get; set; } = string.Empty;
    public string? SourceLang { get; set; } // Может быть null, если DeepL определяет язык автоматически
    public string TargetLang { get; set; } = "EN";
}
