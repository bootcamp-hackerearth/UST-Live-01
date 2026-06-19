using HealthAxisApplicn.Models;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;

namespace HealthAxisApplicn.Repositories
{
    public interface IAppointmentRepository: IRepository<Appointment>
    {
        Task<List<Appointment>> GetAppointmentsByPatientIdAsync(int patientId, CancellationToken ct = default);
        Task<List<Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId, CancellationToken ct = default);
        Task<List<Appointment>> GetAppointmentsByPatientNameAsync(string patientName, CancellationToken ct = default);
        Task<List<Appointment>> GetAppointmentsByDoctorNameAsync(string doctorName, CancellationToken ct = default);

    }
}
