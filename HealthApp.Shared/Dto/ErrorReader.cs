namespace HealthApp.Shared.Dto

{
    public class ErrorReader
    {
        public int StatusCode { get; set; }
        public string Message {  get; set; }=string.Empty;
        public string? Detail { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }

        public string Path { get; set; }= string.Empty;

    }
}
