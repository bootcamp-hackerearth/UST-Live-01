using HealthCare.Api.Data;
using HealthCare.Api.DTOs.Doctor;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Api.Repositories.Implementations
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        public readonly HealthCareDbContext _context;
        public DoctorRepository(HealthCareDbContext context) : base(context) { }

        public async Task<Doctor?> GetByUserIdAsync(string userId)
        {
            return await _context.Doctors
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task CreateSlots(int doctorId, List<string> timeslots)
        {
            var slots = timeslots.Select(t => new AvailableSlots
            {
                DoctorId = doctorId,
                TimeSlot = t
            });

            await _context.AvailableSlots.AddRangeAsync(slots);
        }

        public async Task<List<string>> GetSlots(int doctorId) =>
            await _context.AvailableSlots
                .Where(s => s.DoctorId == doctorId)
                .Select(s => s.TimeSlot)
                .ToListAsync();

        public async Task<List<DoctorLeaves>> GetLeavesByDoctorId(int doctorId) =>
            await _context.DoctorLeaves
                .Where(l => l.DoctorId == doctorId)
                .ToListAsync();

        public async Task CreateLeaves(int doctorId, List<CreateLeaveDto> leaves)
        {
            var entities = leaves.Select(l => new DoctorLeaves
            {
                DoctorId = doctorId,
                LeaveDate = l.LeaveDate,
                Reason = l.Reason
            });

            await _context.DoctorLeaves.AddRangeAsync(entities);
        }
        public async Task<List<DoctorListDto>> AvailableDoctors(string specialisation, DateOnly date)
        {
            return await _dbSet
                .Where(d => d.Specialisation == specialisation &&
                            d.IsActive &&
                            !d.DoctorLeaves.Any(l => l.LeaveDate == date))

                .Where(d =>
                    d.AvailableSlots.Any(slot =>
                        !d.Appointments.Any(a =>
                            a.TimeSlot == slot.TimeSlot &&
                            a.ScheduledDate == date &&
                            a.Status != "Cancelled"
                        )
                    )
                )

                .Select(d => new DoctorListDto
                {
                    DoctorId = d.DoctorId,
                    FullName = d.FullName,
                    Specialisation = d.Specialisation,
                    ConsultationFee = d.ConsultationFee,
                    IsActive = d.IsActive
                })
                .ToListAsync();
        }
    }


}
