using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogPlatform.Models.DB
{
    [Table("Users")] // Явное имя таблицы в MS SQL Server
    public class User
    {
        // Уникальный идентификатор пользователя
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // Основная информация
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        // Дополнительные данные
        [MaxLength(500)]
        public string? Bio { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Навигационное свойство: один пользователь может иметь много постов
        public List<UserPost> Posts { get; set; } = new List<UserPost>();
    }
}
