using HealthCareApp.Data;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace HealthCareApp.Repository.Impl
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        private readonly HealthAxisDbContext _context;

        public DoctorRepository(HealthAxisDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<List<Doctor>> GetAllActiveAsync(CancellationToken ct = default)
        {
            return await _context.Doctors
                .Where(doctor => doctor.IsActive)
                .ToListAsync(ct);
        }

        public async Task<List<Doctor>> GetBySpecialisationAsync(
            SpecialisationType specialisation,
            CancellationToken ct = default)
        {
            return await _context.Doctors
                .Where(doctor => doctor.Specialisation == specialisation)
                .ToListAsync(ct);
        }

        public async Task<List<Doctor>> GetActiveBySpecialisationAsync(
            SpecialisationType specialisation,
            CancellationToken ct = default)
        {
            return await _context.Doctors
                .Where(doctor =>
                    doctor.IsActive &&
                    doctor.Specialisation == specialisation)
                .ToListAsync(ct);
        }

        public async Task<bool> ExistsByEmailAsync(
            string email,
            CancellationToken ct = default)
        {
            string normalizedEmail = email.Trim().ToLower();

            return await _context.Doctors
                .AnyAsync(doctor => doctor.Email == normalizedEmail, ct);
        }

        public async Task<Doctor?> GetByIdentityUserIdAsync(
            string identityUserId,
            CancellationToken ct = default)
        {
            return await _context.Doctors
                .FirstOrDefaultAsync(
                    doctor => doctor.IdentityUserId == identityUserId,
                    ct);
        }
    }
}