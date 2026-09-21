using BlogPlatform.Models.DB;

namespace BlogPlatform.Services
{
    public interface IFeedbackService
    {
        Task<IEnumerable<Feedback>> GetFeedbacksAsync();
        Task<Feedback> CreateFeedbackAsync(string fromUser, string text);
    }
}