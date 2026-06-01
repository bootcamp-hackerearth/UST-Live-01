using HealthApp.ConsoleApp.Databases;
using HealthApp.ConsoleApp.Models;

namespace HealthApp.ConsoleApp.Databases
{
    public class HealthRecordDB
    {
        public List<HealthRecord> Records { get; set; } = new List<HealthRecord>();
    }
}