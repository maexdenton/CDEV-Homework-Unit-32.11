using WebApplication1.Models.DB;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksAsync()
        {
            // Бизнес-логика: получение отзывов
            return await _feedbackRepository.GetAllAsync();
        }

        public async Task<Feedback> CreateFeedbackAsync(string fromUser, string text)
        {
            // Бизнес-логика: валидация и подготовка объекта
            if (string.IsNullOrWhiteSpace(fromUser) || string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Имя и текст отзыва не могут быть пустыми.");
            }

            var feedback = new Feedback
            {
                Id = Guid.NewGuid(),
                FromUser = fromUser.Trim(),
                Text = text.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            await _feedbackRepository.AddAsync(feedback);
            return feedback;
        }
    }
}