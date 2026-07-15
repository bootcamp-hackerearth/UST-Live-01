using System.ComponentModel.DataAnnotations;

namespace HealthAxisCore_Api.Models
{
    public class AdminHandoffCode
    {
        [Key]
        public int AdminHandoffCodeId { get; set; }

        [Required]
        public string CodeHash { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public bool IsUsed { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}