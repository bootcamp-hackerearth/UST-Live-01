using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisCore_Api.Repositories.Implementation
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<List<Doctor>> GetDoctorsAsync(
            string? specialisation,
            CancellationToken ct = default)
        {
            IQueryable<Doctor> query = _context.Doctors
                .Where(d => d.IsActive);

            if (!string.IsNullOrWhiteSpace(specialisation))
            {
                query = query.Where(d => d.Specialisation == specialisation);
            }

            return await query
                .OrderBy(d => d.DoctorName)
                .ToListAsync(ct);
        }

        public async Task<int> CountDoctorsAsync(
            string? specialisation,
            CancellationToken ct = default)
        {
            IQueryable<Doctor> query = _context.Doctors
                .Where(d => d.IsActive);

            if (!string.IsNullOrWhiteSpace(specialisation))
            {
                query = query.Where(d => d.Specialisation == specialisation);
            }

            return await query.CountAsync(ct);
        }

        public async Task<List<Doctor>> GetPagedDoctorsAsync(
            string? specialisation,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            IQueryable<Doctor> query = _context.Doctors
                .Where(d => d.IsActive);

            if (!string.IsNullOrWhiteSpace(specialisation))
            {
                query = query.Where(d => d.Specialisation == specialisation);
            }

            return await query
                .OrderBy(d => d.DoctorName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<List<string>> GetAvailableSlotsAsync(
            int doctorId,
            DateTime date,
            CancellationToken ct = default)
        {
            List<string> slots =
            [
                "09:00",
                "10:00",
                "11:00",
                "12:00",
                "13:00",
                "14:00",
                "15:00",
                "16:00"
            ];

            var booked = await _context.Appointments
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate.Date == date.Date &&
                    a.Status != "Cancelled")
                .Select(a => a.TimeSlot)
                .ToListAsync(ct);

            return slots
                .Except(booked)
                .ToList();
        }
    }
}