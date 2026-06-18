namespace HealthAxisCore_Api.DTOs.Common
{
    public class ErrorResponse
    {
        public bool Success { get; set; } = false;

        public string Message { get; set; } = string.Empty;

        public int StatusCode { get; set; }

        
    }
}
