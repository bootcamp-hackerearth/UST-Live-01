using HealthCareApp.Data;

using HealthCareApp.Models;

using HealthCareApp.Repository.Interface;

using Microsoft.EntityFrameworkCore;

using HealthCareApp.Enums;

namespace HealthCareApp.Repository.Impl
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        private readonly HealthAxisDbContext _context;

        public DoctorRepository(HealthAxisDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Doctor>> GetAllActiveAsync(CancellationToken ct = default)
        {
            return await _context.Doctors
                .Where(d => d.IsActive)
                .ToListAsync(ct);
        }

        public async Task<List<Doctor>> GetBySpecialisationAsync(SpecialisationType specialisation, CancellationToken ct = default)
        {
            return await _context.Doctors
                .Where(d => d.Specialisation == specialisation)
                .ToListAsync(ct);
        }

        public async Task<List<Doctor>> GetActiveBySpecialisationAsync(SpecialisationType specialisation, CancellationToken ct = default)
        {
            return await _context.Doctors
                .Where(d => d.IsActive && d.Specialisation == specialisation)
                .ToListAsync(ct);

        }
        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
        {
            return await _context.Doctors
                .AnyAsync(d => d.Email.ToLower() == email.ToLower(), ct);
        }


        public async Task<List<Doctor>> GetPendingDoctorsAsync(CancellationToken ct = default)
        {
            return await _context.Doctors
                .Where(d => d.VerificationStatus == DoctorVerificationStatus.Pending)
                .ToListAsync(ct);
        } 
    }
    }
