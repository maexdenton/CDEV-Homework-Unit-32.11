using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogPlatform.Models.DB
{
    [Table("UserPosts")]
    public class UserPost
    {
        // Уникальный идентификатор поста
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // Заголовок и содержание
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "nvarchar(max)")] // Текст поста большого объема
        public string Content { get; set; } = string.Empty;

        // Временные метки
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Внешний ключ для связи с MS SQL Server
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        // Навигационное свойство на автора поста
        public User? User { get; set; }
    }
}
