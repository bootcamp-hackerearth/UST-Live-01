namespace HealthApp.API.Models.DTOs;

public class ErrorResponse { public int StatusCode { get; set; } public string Message { get; set; } = string.Empty; public DateTime TimeStamp { get; set; } public string? Path { get; set; } }
