using WebApplication1.Models.DB;

namespace WebApplication1.Services
{
    public interface IFeedbackService
    {
        Task<IEnumerable<Feedback>> GetFeedbacksAsync();
        Task<Feedback> CreateFeedbackAsync(string fromUser, string text);
    }
}