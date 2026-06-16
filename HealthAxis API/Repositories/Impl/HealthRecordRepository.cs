using HealthAxis.API.Data;
using HealthAxis.API.Models;

namespace HealthAxis.API.Repositories
{
    public class HealthRecordRepository : Repository<HealthRecord>, IHealthRecordRepository
    {
        public HealthRecordRepository(HealthAxisDbContext context)
            : base(context)
        {
        }
    }
}
