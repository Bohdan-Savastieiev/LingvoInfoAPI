using Microsoft.AspNetCore.Mvc;

namespace LanguageStudyAPI.Controllers
{
    public class GoogleTranslationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
