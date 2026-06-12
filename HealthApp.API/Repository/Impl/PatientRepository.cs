using HealthApp.API.Data;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Interface;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HealthApp.API.Repository.Impl
{
    public class PatientRepository : IPatientRepository
    {
        private readonly HealthAppDBEntities _db;

        public PatientRepository(HealthAppDBEntities context)
        {
            _db = context;
        }

        // ✅ ADD
        public async Task AddAsync(Patient patient)
        {
            _db.Patients.Add(patient);
            await _db.SaveChangesAsync();
        }

        // ✅ GET ALL
        public async Task<List<Patient>> GetAllAsync()
        {
            return await _db.Patients.ToListAsync();
        }

        // ✅ GET BY ID
        public async Task<Patient> GetByIdAsync(int id)
        {
            return await _db.Patients
                .FirstOrDefaultAsync(pa => pa.PatientId == id);
        }

        // ✅ UPDATE
        public async Task UpdatePatientAsync(int id, Patient patient)
        {
            var existing = await _db.Patients
                .FirstOrDefaultAsync(pa => pa.PatientId == id);

            if (existing == null)
                return;

            existing.FullName = patient.FullName;
            existing.DateOfBirth = patient.DateOfBirth;
            existing.Gender = patient.Gender;
            existing.PhoneNumber = patient.PhoneNumber;
            existing.Email = patient.Email;
            existing.InsuranceId = patient.InsuranceId;

            await _db.SaveChangesAsync();
        }
    }
}