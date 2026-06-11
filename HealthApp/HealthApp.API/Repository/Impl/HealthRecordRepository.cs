using HealthApp.API.Data;
using HealthApp.API.Repository.Interface;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace HealthApp.API.Repository.Impl
{
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly HealthAppEntities _db;

        public HealthRecordRepository(HealthAppEntities db)
        {
            _db = db;
        }

        // ✅ ADD (ASYNC)
        public async Task AddAsync(HealthRecord record)
        {
            _db.HealthRecords.Add(record);
            await _db.SaveChangesAsync();
        }

        // ✅ GET ALL (ASYNC)
        public async Task<List<HealthRecord>> GetAllAsync()
        {
            return await _db.HealthRecords.ToListAsync();
        }
    }
}