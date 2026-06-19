using System.ComponentModel.DataAnnotations;

namespace HealthAxisApplicn.Models
{
    public class ApplicationUser
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
