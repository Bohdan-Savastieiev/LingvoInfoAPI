using LanguageStudyAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LanguageStudyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LingvoController : Controller
    {
        private ILingvoApiService _lingvoService;
        public LingvoController(ILingvoApiService lingvoService)
        {
            _lingvoService = lingvoService;
        }

        [HttpGet("Translation")]
        public async Task<IActionResult> LingvoTest(string text, string srcLang, string dstLang, bool isCaseSensitive)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(srcLang) || string.IsNullOrEmpty(dstLang))
            {
                return BadRequest("Lexeme, source language, and destination language are required.");
            }

            var result = await _lingvoService.GetTranslationAsync(text, srcLang, dstLang, isCaseSensitive);

            return Ok(result);

        }

        [HttpGet("Examples")]
        public async Task<IActionResult> LingvoExamples(string text, string srcLang, string dstLang, bool isCaseSensitive)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(srcLang) || string.IsNullOrEmpty(dstLang))
            {
                return BadRequest("Lexeme, source language, and destination language are required.");
            }

            var examples = await _lingvoService.GetExamplesFromTranslationAsync(text, srcLang, dstLang, isCaseSensitive);

            var result = JsonConvert.SerializeObject(examples);
            return Ok(result);

        }
    }
}
