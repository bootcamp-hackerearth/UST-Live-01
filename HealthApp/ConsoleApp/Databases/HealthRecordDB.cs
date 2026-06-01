using HealthApp.ConsoleApp.Databases;
using HealthApp.ConsoleApp.Models;

namespace HealthApp.ConsoleApp.Databases
{
    public class HealthRecordDb
    {
        public List<HealthRecord> Records = new List<HealthRecord>();
        //No database seeding for health records as they are created dynamically when patients visit doctors and get diagnosed.

    }
}