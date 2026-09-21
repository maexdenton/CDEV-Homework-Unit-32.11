using BlogPlatform.Models.DB;

namespace BlogPlatform.Repositories
{
    public interface ILogRepository
    {
        Task AddLogAsync(RequestLog log);
        Task<IEnumerable<RequestLog>> GetAllLogsAsync();
    }
}
