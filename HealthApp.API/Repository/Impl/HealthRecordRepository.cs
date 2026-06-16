using HealthApp.API.Data;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;

namespace HealthApp.API.Repository.Impl
{
    public class HealthRecordRepository : Repository<HealthRecord>, IHealthRecordRepository
    {
        public HealthRecordRepository(HealthAppDbContext context) : base(context)
        {
        }
    }
}
