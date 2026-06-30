using S3_HealthAxisApi.Models;

namespace S3_HealthAxisApi.Repository.Interface
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<Appointment?> GetByIdAsync(int id);
        Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId);

        Task<IEnumerable<Appointment>> GetDoctorTodayScheduleAsync(int doctorId, DateOnly today);
        Task<IEnumerable<Appointment>> GetDoctorWeekScheduleAsync(int doctorId, DateOnly startDate, DateOnly endDate);

        Task<bool> ExistsSamePatientSameDoctorSameDateAsync(int patientId, int doctorId, DateOnly date);
        Task<bool> ExistsSamePatientSameSlotSameDateAsync(int patientId, DateOnly date, int timeSlot);
        Task<bool> ExistsSameDoctorSameSlotSameDateAsync(int doctorId, DateOnly date, int timeSlot);

        Task<bool> ExistsSamePatientSameDoctorSameDateAsync(int patientId, int doctorId, DateOnly date, int appointmentId);

        Task<bool> ExistsSamePatientSameSlotSameDateAsync(int patientId, DateOnly date, int timeSlot, int appointmentId);

        Task<bool> ExistsSameDoctorSameSlotSameDateAsync(int doctorId, DateOnly date, int timeSlot, int appointmentId);
        Task<IEnumerable<Appointment>> GetDoctorPatientAppointmentsAsync(int doctorId);

        Task AddAsync(Appointment appointment);
        Task UpdateAsync(Appointment appointment);
        Task<bool> ExistsAsync(int id);
        Task SaveChangesAsync();
    }
}