using System.Diagnostics.CodeAnalysis;

namespace HealthAxisHealth.Shared.DTOs.CommonDtos
{
    [ExcludeFromCodeCoverage]
    public class ErrorResponseDto
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }
            = string.Empty;
    }
}
