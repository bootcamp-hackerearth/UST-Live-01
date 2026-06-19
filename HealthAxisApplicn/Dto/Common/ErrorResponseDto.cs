namespace HealthAxisApplicn.Dto.Common
{
    public class ErrorResponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Path { get; set; }
    }
}
