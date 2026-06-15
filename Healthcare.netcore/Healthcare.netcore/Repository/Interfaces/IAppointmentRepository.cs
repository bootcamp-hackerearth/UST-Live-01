using HealthAxis.API.Models;

namespace HealthAxis.API.Repositories.Interfaces;

public interface IAppointmentRepository : IRepository<Appointment>
{
    Task<List<Appointment>> GetByPatientIdAsync(int patientId);

    Task<List<Appointment>> GetByDoctorIdAsync(int doctorId);
}