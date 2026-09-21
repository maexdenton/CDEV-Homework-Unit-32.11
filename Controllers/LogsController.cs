using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting; // Требуется для расширения .IsDevelopment()
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    public class LogsController : Controller
    {
        private readonly ILogRepository _logRepository;
        private readonly IWebHostEnvironment _env; // Добавляем сервис окружения

        public LogsController(ILogRepository logRepository, IWebHostEnvironment env)
        {
            _logRepository = logRepository;
            _env = env;
        }

        [Route("logs")]
        public async Task<IActionResult> Index()
        {
            // Если приложение запущено НЕ в режиме Development (например, в Production) - возвращаем 404
            if (!_env.IsDevelopment())
            {
                return NotFound();
            }

            var logs = await _logRepository.GetAllLogsAsync();
            return View(logs);
        }
    }
}