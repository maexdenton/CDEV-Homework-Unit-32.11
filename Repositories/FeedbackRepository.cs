using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.DB;

namespace WebApplication1.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly BlogDbContext _context;

        public FeedbackRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Feedback>> GetAllAsync()
        {
            return await _context.Feedbacks
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Feedback feedback)
        {
            await _context.Feedbacks.AddAsync(feedback);
            await _context.SaveChangesAsync();
        }
    }
}
