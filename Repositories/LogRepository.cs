using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.DB;

namespace WebApplication1.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly BlogDbContext _context;

        public LogRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task AddLogAsync(RequestLog log)
        {
            await _context.RequestLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<RequestLog>> GetAllLogsAsync()
        {
            // Возвращаем последние 200 логов, отсортированных от самых свежих к старым
            return await _context.RequestLogs
                .OrderByDescending(l => l.Timestamp)
                .Take(200)
                .ToListAsync();
        }
    }
}
