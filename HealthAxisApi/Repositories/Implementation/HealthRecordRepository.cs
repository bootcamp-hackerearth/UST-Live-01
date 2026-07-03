using Microsoft.EntityFrameworkCore;
using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Models;

namespace HealthAxisCore_Api.Repositories
{
    public class HealthRecordRepository : GenericRepository<HealthRecord>, IHealthRecordRepository
    {
        private readonly HealthAppDbContext _context;

        public HealthRecordRepository(HealthAppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HealthRecord>> GetByPatient(int patientId)
        {
            return await _context.HealthRecords
                .Where(hr => hr.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<bool> ExistsByAppointmentIdAsync(int appointmentId)
        {
            return await _context.HealthRecords
                .AnyAsync(hr => hr.AppointmentId == appointmentId);
        }
    }
}