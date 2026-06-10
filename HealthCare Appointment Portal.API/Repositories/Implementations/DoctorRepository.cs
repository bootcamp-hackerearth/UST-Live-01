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
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ApplicationDbContext _context;

        public DoctorRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Doctor> GetByIdAsync(
            int id)
        {
            return await _context.Doctors
                .FindAsync(id);
        }

        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            return await _context.Doctors
                .ToListAsync();
        }

        public async Task AddAsync(
            Doctor doctor)
        {
            _context.Doctors
                .Add(doctor);

            await _context
                .SaveChangesAsync();
        }

        public async Task UpdateAsync(
            Doctor doctor)
        {
            _context.Entry(doctor)
                .State = EntityState.Modified;

            await _context
                .SaveChangesAsync();
        }

        public async Task DeleteAsync(
            int id)
        {
            Doctor doctor =
                await _context.Doctors
                    .FindAsync(id);

            if (doctor != null)
            {
                _context.Doctors
                    .Remove(doctor);

                await _context
                    .SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Doctor>>
            GetDoctorsBySpecialisationAsync(
                Specialisation specialisation)
        {
            return await _context.Doctors
                .Where(d =>
                    d.Specialisation ==
                    specialisation)
                .ToListAsync();
        }
    }
}