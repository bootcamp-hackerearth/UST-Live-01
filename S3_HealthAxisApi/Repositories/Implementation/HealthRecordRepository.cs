using HealthAxis.API.Data;
using Microsoft.EntityFrameworkCore;
using S3_HealthAxisApi.Models;
using S3_HealthAxisApi.Repository.Interface;
using System.Diagnostics.CodeAnalysis;

namespace S3_HealthAxisApi.Repository.Implementation
{
    [ExcludeFromCodeCoverage]
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly HealthAxisDbContext _context;

        public HealthRecordRepository(HealthAxisDbContext context)
        {
            _context = context;
        }

        public async Task<HealthRecord?> GetByIdAsync(int id)
        {
            return await _context.HealthRecords
                .Include(r => r.Doctor)
                .Include(r => r.Patient)
                .Include(r => r.Appointment)
                .FirstOrDefaultAsync(r => r.HealthRecordId == id);
        }

        public async Task<HealthRecord?> GetByAppointmentIdAsync(int appointmentId)
        {
            return await _context.HealthRecords
                .Include(r => r.Doctor)
                .Include(r => r.Patient)
                .Include(r => r.Appointment)
                .FirstOrDefaultAsync(r => r.AppointmentId == appointmentId);
        }

        public async Task<IEnumerable<HealthRecord>> GetByPatientIdAsync(int patientId)
        {
            return await _context.HealthRecords
                .Include(r => r.Doctor)
                .Include(r => r.Patient)
                .Include(r => r.Appointment)
                .Where(r => r.PatientId == patientId)
                .OrderByDescending(r => r.CreatedOn)
                .ToListAsync();
        }

        public async Task AddAsync(HealthRecord record)
        {
            await _context.HealthRecords.AddAsync(record);
        }

        public Task UpdateAsync(HealthRecord record)
        {
            _context.HealthRecords.Update(record);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}