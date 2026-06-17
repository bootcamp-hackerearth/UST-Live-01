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
        public async Task<Appointment?> DeleteAppointmentAsync(int appointmentId, CancellationToken ct = default)
        {
            var existing = await _context.Set<Appointment>().FindAsync([appointmentId], ct);
            if (existing == null) return null;
            _context.Set<Appointment>().Remove(existing);
            await _context.SaveChangesAsync(ct);
            return existing;
  
        }

        public async Task<List<Appointment>> GetAppointmentByDoctorIdAsync(int doctorId, CancellationToken ct = default)
        {
            var appointments = await _context.Set<Appointment>().Where(a => a.DoctorID == doctorId).ToListAsync(ct);
            return appointments;
        }

        public async Task<List<Appointment>> GetAppointmentByPatientIdAsync(int patientId, CancellationToken ct = default)
        {
            var appointments = await _context.Set<Appointment>().Where(a => a.PatientID == patientId).ToListAsync(ct);
            return appointments;
        }

        public async Task<List<Appointment>> GetAppointmentsByDoctorNameAsync(string doctorName, CancellationToken ct = default)
        {
            var appointmentsByDoctor = await _context.Set<Appointment>().Where(a => a.Doctor.DoctorName == doctorName).ToListAsync(ct);
            return appointmentsByDoctor;
        }

        public async Task<List<Appointment>> GetAppointmentsByPatientNameAsync(string patientName, CancellationToken ct = default)
        {
            var appointmentsByPatient = await _context.Set<Appointment>().Where(a => a.Patient.PatientName == patientName).ToListAsync(ct);
            return appointmentsByPatient;
        }
    }
}
