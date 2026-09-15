using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration; // Добавляем поле конфигурации

        // Внедряем IConfiguration через конструктор
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Создаем Action для страницы About с явным URL /about
        [Route("about")]
        public IActionResult About()
        {
            // Получаем имя из appsettings.json
            string appName = _configuration["AppName"] ?? "Неизвестное приложение";

            // Передаем имя в View через ViewBag (или ViewData)
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
