using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Feedback
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Укажите ваше имя")]
        public string FromUser { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите текст отзыва")]
        public string Text { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
