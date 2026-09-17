using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackController(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        // GET: /Feedback — Отображение страницы с отзывами
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var feedbacks = await _feedbackRepository.GetAllAsync();
            return View(feedbacks);
        }

        // POST: /Feedback/Add - AJAX-обработчик для добавления отзыва
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Feedback feedback)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Заполните все поля корректно." });
            }

            feedback.CreatedAt = DateTime.UtcNow;
            await _feedbackRepository.AddAsync(feedback);

            // Возвращаем успех и данные созданного отзыва в формате JSON
            return Json(new
            {
                success = true,
                feedback = new
                {
                    fromUser = feedback.FromUser,
                    text = feedback.Text,
                    createdAt = feedback.CreatedAt.ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss")
                }
            });
        }
    }
}
