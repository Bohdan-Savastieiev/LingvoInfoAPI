using LanguageStudyAPI.Models;
using LanguageStudyAPI.Services;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

[ApiController]
[Route("api/[controller]")]
public class TranslationController : ControllerBase
{
    private readonly GoogleTranslationAPIService _googleTranslationService;
    private readonly LingueeApiService _lingueeService;
    private readonly LingvoApiService _lingvoService;


    public TranslationController(
        GoogleTranslationAPIService translationService, 
        LingueeApiService lingueeService,
        LingvoApiService lingvoService)
    {
        _googleTranslationService = translationService;
        _lingueeService = lingueeService;
        _lingvoService = lingvoService;
    }

    [HttpGet("translate")]
    public async Task<IActionResult> Translate(string lexeme, string srcLang, string dstLang)
    {
        if (string.IsNullOrEmpty(lexeme) || string.IsNullOrEmpty(srcLang) || string.IsNullOrEmpty(dstLang))
        {
            return BadRequest("Lexeme, source language, and destination language are required.");
        }

        try
        {
            var translations = await _lingueeService.TranslateWordAsync(lexeme, srcLang, dstLang);
            return Ok(translations);
            //if (translations.Count > 0)
            //{
            //    return Ok(translations);
            //}
            //else
            //{
            //    var translation = _googleTranslationService.TranslateTextAsync(lexeme, dstLang);
            //    return Ok(translation);
            //}
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}