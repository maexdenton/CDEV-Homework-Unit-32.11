using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    public class LogsController : Controller
    {
        private readonly ILogRepository _logRepository;

        public LogsController(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        [Route("logs")]
        public async Task<IActionResult> Index()
        {
            var logs = await _logRepository.GetAllLogsAsync();
            return View(logs);
        }
    }
}
