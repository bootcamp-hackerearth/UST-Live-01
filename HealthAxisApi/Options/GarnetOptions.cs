namespace HealthAxisCore_Api.Options
{
    public class GarnetOptions
    {
        public const string SectionName = "Garnet";

        public string ConnectionString { get; set; } = string.Empty;

        public string InstanceName { get; set; } = string.Empty;
    }
}