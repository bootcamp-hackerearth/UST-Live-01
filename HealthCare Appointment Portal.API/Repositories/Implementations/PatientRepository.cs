using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Repositories
{
    public class PatientRepository
        : IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patient>>
            GetAllAsync()
        {
            return await _context.Patients
                .ToListAsync();
        }

        public async Task<Patient>
            GetByIdAsync(
                int patientId)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p =>
                    p.PatientId == patientId);
        }

        public async Task
            AddAsync(
                Patient patient)
        {
            _context.Patients.Add(patient);

            await _context.SaveChangesAsync();
        }

        public async Task
            UpdateAsync(
                Patient patient)
        {
            _context.Entry(patient)
                .State =
                EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task
            DeleteAsync(
                int patientId)
        {
            var patient =
                await GetByIdAsync(
                    patientId);

            if (patient != null)
            {
                _context.Patients.Remove(patient);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<Patient>
            GetPatientByEmailAsync(
                string email)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p =>
                    p.Email == email);
        }

        public async Task<IEnumerable<Patient>>
            GetPatientsByInsuranceStatusAsync(
                InsuranceStatus status)
        {
            return await _context.Patients
                .Include(p => p.Insurances)
                .Where(p =>
                    p.Insurances.Any(i =>
                        i.Status == status))
                .ToListAsync();
        }
    }
}