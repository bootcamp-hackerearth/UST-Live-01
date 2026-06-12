using HealthAxis.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthAxis.Api.Database;
using System.Linq;
using System.Data.Entity;
using HealthAxis.Api.Models;
using HealthAxis.Api.Repositories;


namespace HealthAxis.Api.Repositories
{
    public class HealthRecordRepositoryImpl : IHealthRecordRepository
    {
        private readonly AppDBContext _context;

        public HealthRecordRepositoryImpl(AppDBContext context)
        {
            _context = context;
        }

        public void Add(HealthRecord record)
        {
            _context.HealthRecords.Add(record);
        }
        public List<HealthRecord> GetByPatient(int patientId)
        {
            return _context.HealthRecords
                .Where(x => x.PatientId == patientId)
                .ToList();
        }

        public bool ExistsByAppointment(int appointmentId)
        {
            return _context.HealthRecords.Any(x => x.AppointmentId == appointmentId);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}