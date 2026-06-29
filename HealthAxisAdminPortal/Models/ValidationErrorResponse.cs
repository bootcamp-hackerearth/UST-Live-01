namespace HealthAxisAdminPortal.Models
{
    public class ValidationErrorResponse
    {
        public Dictionary<string, string[]> Errors { get; set; } = new();
    }
}
