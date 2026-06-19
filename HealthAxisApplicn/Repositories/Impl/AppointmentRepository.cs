using HealthAxisApplicn.Data;
using HealthAxisApplicn.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisApplicn.Repositories.Impl
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        private readonly AppDbContext _context;
        public AppointmentRepository(AppDbContext context): base(context)
        {
            _context = context;
        }

        public async Task<List<Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId, CancellationToken ct = default)
        {
            var appointments = await _context.Set<Appointment>().Where(a => a.DoctorId == doctorId).ToListAsync(ct);
            return appointments;
        }

        public async Task<List<Appointment>> GetAppointmentsByPatientIdAsync(int patientId, CancellationToken ct = default)
        {
            var appointments = await _context.Set<Appointment>().Where(a => a.PatientId == patientId).ToListAsync(ct);
            return appointments;
        }

        public async Task<List<Appointment>> GetAppointmentsByDoctorNameAsync(string doctorName, CancellationToken ct = default)
        {
            return await _context.Set<Appointment>()
                .Include(a => a.Doctor)
                .Where(a => a.Doctor.DoctorName.ToLower() == doctorName.ToLower())
                .ToListAsync(ct);
        }

        public async Task<List<Appointment>> GetAppointmentsByPatientNameAsync(string patientName, CancellationToken ct = default)
        {
            return await _context.Set<Appointment>()
                .Include(a => a.Patient)
                .Where(a => a.Patient.PatientName.ToLower() == patientName.ToLower())
                .ToListAsync(ct);
        }

    }
}
