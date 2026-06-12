using HealthApp.API.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthApp.API.Repository.Interface
{
    public interface IAppointmentRepository
    {
        Task AddAsync(Appointment appointment);

        Task<List<Appointment>> GetAllAsync();

        Task<Appointment> GetByIdAsync(int id);

        Task SaveAsync();

        Task<bool> ExistsPatientBookingAsync(
            int patientId,
            int doctorId,
            DateTime date,
            string slot);

        Task<bool> IsSlotBookedAsync(
            int doctorId,
            DateTime date,
            string slot);

        Task<List<Appointment>> GetAppointmentsByPatientAsync(int patientId);

        Task<List<Appointment>> GetPendingAppointmentsByDoctorAsync(int doctorId);

        Task<List<Appointment>> GetUpcomingAppointmentsByDoctorAsync(
            int doctorId,
            DateTime fromDate,
            DateTime toDate);

        Task<List<string>> GetBookedSlotsAsync(
            int doctorId,
            DateTime date);
    }
}
