using Healthcare.Shared.DTOs.Patient;
using HealthCare.Api.Data;
using HealthCare.Api.Exceptions;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Api.Repositories.Implementations
{
    public class PatientRepository : Repository<Patient>,IPatientRepository
    {
  
        public PatientRepository(HealthCareDbContext context) : base(context) { }
        public async Task<Patient?> GetByUserIdAsync(string? userId)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<PatientListDto?> GetMyProfileAsync(int id)
        {
            var patient = await (
                from p in _context.Patients
                join u in _context.Users on p.UserId equals u.Id
                where p.PatientId == id
                select new PatientListDto
                {
                    PatientId = p.PatientId,
                    FullName = p.FullName,
                    PhoneNumber = p.PhoneNumber,
                    Gender = p.Gender,
                    HasInsurance = p.InsuranceId != null,
                    Email = u.Email 
        }
            ).FirstOrDefaultAsync();

            if (patient == null)
                throw new PatientNotFoundException("Patient Not Found");

            return patient;
        }




    }
}
