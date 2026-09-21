using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.DB;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService _feedbackService;

        // Внедряем сервис бизнес-логики
        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        // GET: /Feedback
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var feedbacks = await _feedbackService.GetFeedbacksAsync();
            return View(feedbacks);
        }

        // POST: /Feedback/Add (AJAX)
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Feedback feedbackDto)
        {
            try
            {
                var createdFeedback = await _feedbackService.CreateFeedbackAsync(feedbackDto.FromUser, feedbackDto.Text);

                return Json(new
                {
                    success = true,
                    feedback = new
                    {
                        fromUser = createdFeedback.FromUser,
                        text = createdFeedback.Text,
                        createdAt = createdFeedback.CreatedAt.ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss")
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}