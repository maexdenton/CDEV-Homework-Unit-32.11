using BlogPlatform.Models.DB;

namespace BlogPlatform.Repositories
{
    public interface IUserPostRepository
    {
        Task<IEnumerable<UserPost>> GetAllAsync();
        Task<UserPost?> GetByIdAsync(Guid id);
        Task<IEnumerable<UserPost>> GetByUserIdAsync(Guid userId); // Получить все посты конкретного юзера
        Task AddAsync(UserPost post);
        Task UpdateAsync(UserPost post);
        Task DeleteAsync(Guid id);
    }
}
