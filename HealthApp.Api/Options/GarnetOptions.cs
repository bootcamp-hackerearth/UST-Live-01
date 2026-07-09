namespace HealthApp.Api.Options
{
    public class GarnetOptions
    {
        public string ConnectionString { get; set; } = "127.0.0.1:6379";
        public string InstanceName { get; set; } = "HealthApp:";
    }
}