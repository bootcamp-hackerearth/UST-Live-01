using HealthAxis.API.Data;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Implementations;
using HealthAxis.API.Repositories.Interfaces;

namespace HealthAxis.API.Repositories.Implementations
{
    public class HealthRecordRepository : Repository<HealthRecord>, IHealthRecordRepository
    {
        public HealthRecordRepository(HealthAxisDbContext context) : base(context)
        {
        }
    }
}