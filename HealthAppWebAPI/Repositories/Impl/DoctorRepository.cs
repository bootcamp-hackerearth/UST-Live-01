using HealthAppMVC.Enums;
using HealthAppWebAPI.Enums;
using HealthAppWebAPI.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HealthAppWebAPI.Repositories.Impl
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HealthAppDbContext _context;

        public DoctorRepository(HealthAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Doctor>> GetAllAsync()
        {
            return await _context.Doctors
                .ToListAsync();
        }

        public async Task<Doctor> GetByIdAsync(int id)
        {
            return await _context.Doctors
                .FindAsync(id);
        }

        public async Task AddAsync(Doctor doctor)
        {
            _context.Doctors.Add(doctor);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Doctor doctor)
        {
            _context.Entry(doctor).State =
                EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task ChangeStatusAsync(int id, bool isActive)
        {
            var doctor =
                await _context.Doctors.FindAsync(id);

            if (doctor == null)
            {
                return;
            }

            doctor.IsActive = isActive;

            await _context.SaveChangesAsync();
        }

        public async Task<List<Doctor>> GetBySpecialisationAsync(
            SpecialisationType specialisation)
        {
            string specialisationValue =
                specialisation.ToString();

            return await _context.Doctors
                .Where(d => d.Specialisation == specialisationValue)
                .ToListAsync();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            string normalizedEmail =
                email.Trim().ToLower();

            return await _context.Doctors
                .AnyAsync(d =>
                    d.DoctorEmail.ToLower() == normalizedEmail);
        }

        public async Task<bool> EmailExistsForOtherDoctorAsync(
            int doctorId,
            string email)
        {
            string normalizedEmail =
                email.Trim().ToLower();

            return await _context.Doctors
                .AnyAsync(d =>
                    d.DoctorId != doctorId &&
                    d.DoctorEmail.ToLower() == normalizedEmail);
        }
    }
}