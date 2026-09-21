using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BlogPlatform.Models;

namespace BlogPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        // Внедряем все зависимости в едином конструкторе
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        // Главная страница сайта
        public IActionResult Index()
        {
            return View();
        }

        // Страница "О нас" с явным URL /about
        [Route("about")]
        public IActionResult About()
        {
            // Получаем имя из appsettings.json
            string appName = _configuration["AppName"] ?? "Блог-платформа";
            ViewBag.ApplicationName = appName;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}