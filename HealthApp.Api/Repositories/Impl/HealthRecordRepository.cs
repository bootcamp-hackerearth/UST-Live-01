using HealthApp.Api.Data;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Repositories.Impl
{
    public class HealthRecordRepository : Repository<HealthRecord>, IHealthRecordRepository
    {
        public HealthRecordRepository(HealthAppDbContext context) : base(context)
        {
        }
    }
}
