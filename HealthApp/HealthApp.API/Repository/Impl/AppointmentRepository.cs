using HealthApp.API.Data;
using HealthApp.API.Repository.Interface;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HealthApp.API.Repository.Impl
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly HealthAppEntities _db;

        public AppointmentRepository(HealthAppEntities db)
        {
            _db = db;
        }

        // ✅ ADD
        public async Task AddAsync(Appointment appointment)
        {
            _db.Appointments.Add(appointment);
            await _db.SaveChangesAsync();
        }

        // ✅ SAVE
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        // ✅ GET ALL
        public async Task<List<Appointment>> GetAllAsync()
        {
            return await _db.Appointments.ToListAsync();
        }

        // ✅ GET BY ID
        public async Task<Appointment> GetByIdAsync(int id)
        {
            return await _db.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == id);
        }
    }
}