using System.Diagnostics;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICaptchaService _captchaService;

        public HomeController(ILogger<HomeController> logger,
                  ICaptchaService captchaService)
        {
            _logger = logger;
            _captchaService = captchaService;
        }

        public IActionResult Index()
        {
            var text = _captchaService.RandomString(5, RandomCharactersType.All);
            TempData["CaptchaCode"] = text;
            ViewData["CaptchaImage"] = _captchaService.CreateCaptcha(text);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string captcha)
        {
            var captchaCode = TempData["CaptchaCode"].ToString();

            if (string.IsNullOrEmpty(captchaCode) || captcha != captchaCode)
            {
                return StatusCode(500, "Error de Captcha");
            }

            return Ok("Captcha aceptado");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}