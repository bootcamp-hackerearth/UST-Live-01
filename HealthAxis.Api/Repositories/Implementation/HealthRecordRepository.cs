using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories.Interface;

namespace HealthAxisCore_Api.Repositories.Implementation
{
    public class HealthRecordRepository : Repository<HealthRecord>, IHealthRecordRepository
    {
        public HealthRecordRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}
