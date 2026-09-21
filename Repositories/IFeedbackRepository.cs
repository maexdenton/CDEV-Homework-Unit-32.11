using WebApplication1.Models.DB;

namespace WebApplication1.Repositories
{
    public interface IFeedbackRepository
    {
        Task<IEnumerable<Feedback>> GetAllAsync();
        Task AddAsync(Feedback feedback);
    }
}
