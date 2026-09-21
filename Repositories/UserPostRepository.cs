using Microsoft.EntityFrameworkCore;
using BlogPlatform.Data;
using BlogPlatform.Models.DB;

namespace BlogPlatform.Repositories
{
    public class UserPostRepository : IUserPostRepository
    {
        private readonly BlogDbContext _context;

        public UserPostRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserPost>> GetAllAsync()
        {
            return await _context.UserPosts.Include(p => p.User).ToListAsync();
        }

        public async Task<UserPost?> GetByIdAsync(Guid id)
        {
            return await _context.UserPosts.Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<UserPost>> GetByUserIdAsync(Guid userId)
        {
            return await _context.UserPosts
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task AddAsync(UserPost post)
        {
            await _context.UserPosts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserPost post)
        {
            post.UpdatedAt = DateTime.UtcNow; // Автообновление даты редактирования
            _context.UserPosts.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var post = await _context.UserPosts.FindAsync(id);
            if (post != null)
            {
                _context.UserPosts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }
    }
}
