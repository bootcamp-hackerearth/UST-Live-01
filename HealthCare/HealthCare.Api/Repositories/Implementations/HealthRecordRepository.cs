using HealthCare.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using HealthCare.Api.Models;
using HealthCare.Api.Data;

namespace HealthCare.Api.Repositories.Implementations
{
    public class HealthRecordRepository : Repository<HealthRecord>,IHealthRecordRepository
    {
        public HealthRecordRepository(HealthCareDbContext context) : base(context) { }

        public async Task<List<HealthRecord>> GetHealthRecordByPatient(int id) =>
           await _dbSet
               .Where(hr => hr.PatientId == id)
               .ToListAsync();

        public async Task<List<HealthRecord>> GetHealthRecordByAppointment(int id) =>
            await _dbSet
                .Where(hr => hr.AppointmentId == id)
                .ToListAsync();
    }
}
