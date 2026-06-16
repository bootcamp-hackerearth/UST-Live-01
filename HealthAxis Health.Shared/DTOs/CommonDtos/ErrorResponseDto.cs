namespace HealthAxisHealth.Shared.DTOs.CommonDtos
{
    public class ErrorResponseDto
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }
            = string.Empty;
    }
}
