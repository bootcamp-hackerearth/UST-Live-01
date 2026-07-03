namespace HealthAxis.Shared.DTOs.Common
{
    public class ErrorResponseDto
    {
        public bool Success { get; set; } = false;

        public string Message { get; set; } = string.Empty;

        public int StatusCode { get; set; }
    }
}
