using BlogPlatform.Models.DB;

namespace BlogPlatform.Repositories
{
    public interface IFeedbackRepository
    {
        Task<IEnumerable<Feedback>> GetAllAsync();
        Task AddAsync(Feedback feedback);
    }
}
