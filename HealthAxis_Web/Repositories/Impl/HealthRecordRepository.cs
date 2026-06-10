using HealthAxis.Api.Models;
using System.Collections.Generic;
using System.Linq;
using HealthAxis.Api.Database;

namespace HealthAxis.Api.Repositories
{
    public class HealthRecordRepositoryImpl : IHealthRecordRepository
    {
        private readonly AppDBContext _context;

        public HealthRecordRepositoryImpl(AppDBContext context)
        {
            _context = context;
        }

        public List<HealthRecord> GetByPatient(int patientId)
        {
            return _context.HealthRecords
                .Where(h => h.PatientId == patientId)
                .OrderByDescending(h => h.VisitDate)
                .ToList();
        }

        public HealthRecord GetById(int id)
        {
            return _context.HealthRecords.Find(id);
        }

        // ✅ one record per appointment
        public bool ExistsByAppointment(int appointmentId)
        {
            return _context.HealthRecords
                .Any(h => h.AppointmentId == appointmentId);
        }

        public void Add(HealthRecord record)
        {
            _context.HealthRecords.Add(record);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}