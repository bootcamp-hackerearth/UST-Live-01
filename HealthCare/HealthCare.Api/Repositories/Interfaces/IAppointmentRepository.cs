using HealthCare.Api.Models;


namespace HealthCare.Api.Repositories.Interfaces
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        //Task<bool> SlotExistsAsync(int doctorId, string timeSlot);
        //Task<bool> IsSlotBookedAsync(int doctorId, DateTime date, string timeSlot);
        //Task<bool> PatientExistsAsync(int patientId);
        //Task<bool> DoctorExistsAsync(int doctorId);

        //Task<PagedResult<Appointment>> GetPatientAppointmentsAsync(
        // int patientId,
        // string status,
        // int pageNumber,
        // int pageSize);
        //Task<PagedResult<Appointment>> GetDoctorAppointmentsAsync(
        // int doctorId,
        // string status,
        // int pageNumber,
        // int pageSize);

        //Task<PagedResult<Appointment>> GetUpcomingAppointmentsAsync(
        //int? patientId,
        // int? doctorId,
        // int pageNumber,
        // int pageSize);
    
    }
}
