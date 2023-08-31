using LanguageStudyAPI.Services;
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

        [HttpGet("Sound")]
        public async Task<IActionResult> LingvoSound(string dictionaryName, string fileName)
        {
            if (string.IsNullOrEmpty(dictionaryName) || string.IsNullOrEmpty(fileName))
            {
                return BadRequest("dictionaryName or/and fileName are null or empty.");
            }

            var result = await _lingvoService.GetSoundAsync(dictionaryName, fileName);

            // TODO Make Audio Service
            byte[] audioBytes = Convert.FromBase64String(result.Replace("\"", ""));

            string filePath = @"C:\Users\leisu\Desktop\audio.wav";  // Correct this path
            System.IO.File.WriteAllBytes(filePath, audioBytes);

            return Ok(result);
        }

        private string FindMissingSymbols(string original, string cleaned)
        {
            StringBuilder missingSymbols = new StringBuilder();

            foreach (char c in original)
            {
                if (!cleaned.Contains(c.ToString()))
                {
                    missingSymbols.Append(c);
                }
            }

            return missingSymbols.ToString();
        }
    }
}
