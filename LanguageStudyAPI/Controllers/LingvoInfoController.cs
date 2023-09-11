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
    private readonly ILingvoInfoService _lingvoInfoService;
    public LingvoInfoController(ILingvoInfoService lingvoInfoService)
    {
        _lingvoInfoService = lingvoInfoService;
    }

    [HttpGet("translations")]
    public async Task<IActionResult> GetTranslations(string text, string srcLang, string dstLang, bool includeSound)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(srcLang) || string.IsNullOrEmpty(dstLang))
        {
            return BadRequest("Text, source language, and destination language are required.");
        }

        try
        {
            var result = await _lingvoInfoService.GetTranslationsAsync(text, srcLang, dstLang, includeSound);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}