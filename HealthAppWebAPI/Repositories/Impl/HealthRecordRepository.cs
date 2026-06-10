using System;
using System.Collections.Generic;
using System.Linq;
using HealthAppWebAPI.Repositories.Interfaces;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;

namespace HealthAppWebAPI.Repositories.Impl
{
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly HealthAppDbContext _context;

        public HealthRecordRepository(HealthAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<HealthRecord>> GetAllAsync()
        {
            return await _context.HealthRecords
                .Include(h => h.Appointment.Patient)
                .Include(h => h.Appointment.Doctor)
                .ToListAsync();
        }

        public async Task<HealthRecord> GetByIdAsync(int id)
        {
            return await _context.HealthRecords
                .Include(h => h.Appointment.Patient)
                .Include(h => h.Appointment.Doctor)
                .FirstOrDefaultAsync(h => h.HealthRecordId == id);
        }

        public async Task<HealthRecord> GetByAppointmentIdAsync(int appointmentId)
        {
            return await _context.HealthRecords
                .FirstOrDefaultAsync(h => h.AppointmentId == appointmentId);
        }

        public async Task AddAsync(HealthRecord record)
        {
            _context.HealthRecords.Add(record);
            await _context.SaveChangesAsync();
        }
        public async Task<List<HealthRecord>>GetByPatientIdAsync(
                        int patientId)
        {
            return await _context.HealthRecords
                .Include(h => h.Appointment)
                .Include(h => h.Appointment.Patient)
                .Include(h => h.Appointment.Doctor)
                .Where(h =>
                    h.Appointment.PatientId
                        == patientId)
                .OrderByDescending(
                    h => h.VisitDate)
                .ToListAsync();
        }
    }
}