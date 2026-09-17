using WebApplication1.Models.DB;

namespace WebApplication1.Repositories
{
    public interface ILogRepository
    {
        Task AddLogAsync(RequestLog log);
        Task<IEnumerable<RequestLog>> GetAllLogsAsync();
    }
}
