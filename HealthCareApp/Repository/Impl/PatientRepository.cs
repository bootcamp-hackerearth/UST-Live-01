using HealthCareApp.Data;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HealthCareApp.Repository.Impl
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        private readonly HealthAxisDbContext _context;

        public PatientRepository(HealthAxisDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsDuplicatePatientAsync(
            string patientName,
            string email,
            string phoneNumber,
            DateTime dateOfBirth,
            int? excludePatientId = null,
            CancellationToken ct = default)
        {
            string normalizedPatientName = patientName.Trim().ToLower();
            string normalizedEmail = email.Trim().ToLower();

            return await _context.Patients.AnyAsync(patient =>
                patient.PatientName == normalizedPatientName
                && patient.Email == normalizedEmail
                && patient.PhoneNumber == phoneNumber
                && patient.DateOfBirth.Date == dateOfBirth.Date
                && (!excludePatientId.HasValue || patient.PatientId != excludePatientId.Value),
                ct);
        }

        public async Task<Patient?> GetByIdentityUserIdAsync(
            string identityUserId,
            CancellationToken ct = default)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(patient => patient.IdentityUserId == identityUserId, ct);
        }
    }
}