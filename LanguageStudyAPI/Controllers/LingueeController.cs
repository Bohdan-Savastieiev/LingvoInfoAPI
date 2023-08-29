using LanguageStudyAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LanguageStudyAPI.Controllers
{
    public class LingueeController : Controller
    {
        private LingueeApiService _lingueeService;
        public LingueeController(LingueeApiService lingueeService)
        {
            _lingueeService = lingueeService;
        }

        [HttpGet("Translations")]
        public async Task<IActionResult> LingvoTestMini(string text, string srcLang, string dstLang)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(srcLang) || string.IsNullOrEmpty(dstLang))
            {
                return BadRequest("Lexeme, source language, and destination language are required.");
            }

            var result = await _lingueeService.TranslateWordAsync(text, srcLang, dstLang);

            return Ok(result);

        }
    }
}
