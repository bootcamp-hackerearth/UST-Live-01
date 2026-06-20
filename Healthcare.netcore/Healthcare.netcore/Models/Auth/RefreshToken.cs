namespace HealthAxis.API.Models.Auth
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime ExpiresAt { get; set; }

        public bool IsRevoked { get; set; } = false;
    }
}