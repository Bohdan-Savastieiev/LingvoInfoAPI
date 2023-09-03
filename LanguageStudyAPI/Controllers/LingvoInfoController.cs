using LanguageStudyAPI.Models;
using LanguageStudyAPI.Services;
using LingvoInfoAPI.Services;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

[ApiController]
[Route("api/[controller]")]
public class LingvoInfoController : ControllerBase
{
    private readonly LingvoInfoService _lingvoInfoService;
    public LingvoInfoController(LingvoInfoService lingvoInfoService)
    {
        _lingvoInfoService = lingvoInfoService;
    }

    [HttpGet("translations")]
    public async Task<IActionResult> GetTranslations(string text, string srcLang, string dstLang)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(srcLang) || string.IsNullOrEmpty(dstLang))
        {
            return BadRequest("Text, source language, and destination language are required.");
        }

        try
        {
            var result = await _lingvoInfoService.GetTranslationsAsync(text, srcLang, dstLang);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}