using HealthCare_Appointment_Portal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>>
            GetAllAsync();

        Task<Appointment>
            GetByIdAsync(
                int appointmentId);

        Task
            AddAsync(
                Appointment appointment);

        Task
            UpdateAsync(
                Appointment appointment);

        Task
            DeleteAsync(
                int appointmentId);

        Task<IEnumerable<Appointment>>
            GetAppointmentsByPatientAsync(
                int patientId);

        Task<IEnumerable<Appointment>>
            GetAppointmentsByDoctorAsync(
                int doctorId);

        Task<IEnumerable<Appointment>>
            GetTodayScheduleAsync(
                int doctorId);

        Task<IEnumerable<Appointment>>
            GetWeeklyScheduleAsync(
                int doctorId);

        Task<Appointment>
            GetNextAppointmentByPatientAsync(
                int patientId);
        Task<bool>
            IsSlotAvailableAsync(
                int doctorId,
                DateTime scheduledDate,
                string timeSlot);

        Task<bool>
            IsSlotAvailableForUpdateAsync(
                int appointmentId,
                int doctorId,
                DateTime scheduledDate,
                string timeSlot);
    }
}