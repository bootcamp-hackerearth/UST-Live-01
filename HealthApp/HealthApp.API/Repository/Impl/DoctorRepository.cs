using HealthApp.API.Data;
using HealthApp.API.Repository.Interface;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HealthApp.API.Repository.Impl
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HealthAppEntities _db;

        public DoctorRepository(HealthAppEntities context)
        {
            _db = context;
        }

        // ✅ ADD
        public async Task AddAsync(Doctor doctor)
        {
            _db.Doctors.Add(doctor);
            await _db.SaveChangesAsync();
        }

        // ✅ GET ALL
        public async Task<List<Doctor>> GetAllAsync()
        {
            return await _db.Doctors.ToListAsync();
        }

        // ✅ GET BY ID
        public async Task<Doctor> GetByIdAsync(int id)
        {
            return await _db.Doctors
                .FirstOrDefaultAsync(d => d.DoctorId == id);
        }

        // ✅ UPDATE
        public async Task UpdateAsync(Doctor doctor)
        {
            var existingDoctor = await _db.Doctors
                .FirstOrDefaultAsync(d => d.DoctorId == doctor.DoctorId);

            if (existingDoctor != null)
            {
                existingDoctor.IsActive = doctor.IsActive;
            }

            await _db.SaveChangesAsync();
        }
    }
}