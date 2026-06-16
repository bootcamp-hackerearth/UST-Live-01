using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Models;
using HealthCare.Api.Data;

namespace HealthCare.Api.Repositories.Implementations
{
    public class HealthRecordRepository : Repository<HealthRecord>,IHealthRecordRepository
    {
        public HealthRecordRepository(HealthCareDbContext context) : base(context)
        {
        }
    }
}
