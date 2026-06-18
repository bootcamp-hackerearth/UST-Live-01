namespace HealthCare.Api.Models
{
    public class ErrorResponse
    {
        public int StatusCode {  get; set; }
        public string? Message { get; set; }
        public string? Detials { get; set; }

        public DateTime TimeStamp { get; set; }
        public string? Path { get; set; }
    }
}
