using LanguageStudyAPI.Services;
using LingvoInfoAPI.Clients;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace LanguageStudyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LingvoController : Controller
    {
        private LingvoApiClient _lingvoApiClient;
        public LingvoController(LingvoApiClient lingvoApiClient)
        {
            _lingvoApiClient = lingvoApiClient;
        }

        [HttpGet("Translation")]
        public async Task<IActionResult> LingvoTest(string text, string srcLang, string dstLang, bool isCaseSensitive)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(srcLang) || string.IsNullOrEmpty(dstLang))
            {
                return BadRequest("Lexeme, source language, and destination language are required.");
            }

            var result = await _lingvoApiClient.GetTranslationAsync(text, srcLang, dstLang, isCaseSensitive);

            return Ok(result);

        }
    }
}
