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
        private readonly HealthAppDBEntities _db;

        public DoctorRepository(HealthAppDBEntities context)
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
        public async Task UpdateDoctorAsync(Doctor doctor)
        {
            var existingDoctor = await _db.Doctors
                .FirstOrDefaultAsync(d => d.DoctorId == doctor.DoctorId);

            if (existingDoctor != null)
            {
                existingDoctor.IsActive = doctor.IsActive;
            }

            await _db.SaveChangesAsync();
        }

        public async Task UpdateDoctorAsync(int id, Doctor doctor)
        {
            var existingDoctor = await _db.Doctors
                .FirstOrDefaultAsync(d => d.DoctorId == id);
            if (existingDoctor != null)
            {
                existingDoctor.FullName = doctor.FullName;
                existingDoctor.Specialisation = doctor.Specialisation;
                existingDoctor.IsActive = doctor.IsActive;
                existingDoctor.ConsultationFee = doctor.ConsultationFee;
                existingDoctor.YearsOfExperience = doctor.YearsOfExperience;
            }
            await _db.SaveChangesAsync();
        }
    }
}