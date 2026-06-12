using HealthCare.Shared;
using HealthCareApi.Repositories.Interfaces;
using HealthCareApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HealthCareApi.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            HealthAppDbContext context)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<Appointment> BookAppointmentAsync(Appointment appointment)
        {
            // Validate patient
            var patientExists = await _appointmentRepository.PatientExistsAsync(appointment.PatientId);
           

            if (!patientExists)
                throw new KeyNotFoundException("Invalid or inactive patient");

            // Validate doctor
            var doctorExists = await _appointmentRepository.DoctorExistsAsync(appointment.DoctorId);
            

            if (!doctorExists)
                throw new KeyNotFoundException("Invalid or inactive patient");

            // Validate timeslot exists
            var slotExists = await _appointmentRepository
                .SlotExistsAsync(appointment.DoctorId, appointment.TimeSlot);

            if (!slotExists)
                throw new KeyNotFoundException("Invalid time slot for this doctor");

            // Prevent booking in the past
            if (appointment.ScheduledDate.Date < DateTime.Today)
                throw new KeyNotFoundException("Cannot book appointment in the past");

            // Check slot availability
            var isBooked = await _appointmentRepository
                .IsSlotBookedAsync(appointment.DoctorId,
                                   appointment.ScheduledDate,
                                   appointment.TimeSlot);

            if (isBooked)
                throw new KeyNotFoundException("Time slot already booked");

            // Set default status
            appointment.Status = "Pending";

            // Save
            await _appointmentRepository.AddAsync(appointment);

            return appointment;
        }

        public async Task<PagedResult<Appointment>> GetPatientAppointmentsAsync(
            int patientId,
            string status = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            return await _appointmentRepository.GetPatientAppointmentsAsync(
                patientId,
                status,
                pageNumber,
                pageSize);
        }

        public async Task<PagedResult<Appointment>> GetDoctorAppointmentsAsync(
            int doctorId,
            string status = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            return await _appointmentRepository.GetDoctorAppointmentsAsync(
                doctorId,
                status,
                pageNumber,
                pageSize);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime date)
        {
            return await _appointmentRepository.GetAppointmentsByDateAsync(date);
        }

        public async Task<List<string>> GetAvailableSlotsAsync(int doctorId, DateTime date)
        {
            // Get working slots
            var allSlots = await _appointmentRepository
                .GetDoctorSlotsAsync(doctorId);

            // Get booked slots
            var bookedSlots = await _appointmentRepository
                .GetBookedSlotsAsync(doctorId, date);

            // Remove booked slots
            var availableSlots = allSlots
                .Except(bookedSlots)
                .ToList();

            return availableSlots;
        }


        public async Task<Appointment> ConfirmAppointmentAsync(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);

            if (appointment == null)
                throw new KeyNotFoundException("Appointment not found");

            if (appointment.Status == "Cancelled")
                throw new KeyNotFoundException("Cannot confirm a cancelled appointment");

            if (appointment.Status == "Completed")
                throw new KeyNotFoundException("Appointment already completed");

            // Update status
            appointment.Status = "Confirmed";

            await _appointmentRepository.UpdateAsync(appointment);

            return appointment;
        }

        public async Task<Appointment> CancelAppointmentAsync(int appointmentId, string reason)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);

            if (appointment == null)
                throw new KeyNotFoundException("Appointment not found");

            if (appointment.Status == "Cancelled")
                throw new KeyNotFoundException("Appointment already cancelled");

            if (string.IsNullOrWhiteSpace(reason))
                throw new KeyNotFoundException("Cancellation reason is required");

            // Update fields
            appointment.Status = "Cancelled";
            appointment.CancellationReason = reason;

            await _appointmentRepository.UpdateAsync(appointment);

            return appointment;
        }

        public async Task<PagedResult<Appointment>> GetUpcomingAppointmentsAsync(
    int? patientId,
    int? doctorId,
    int pageNumber,
    int pageSize)
        {
            return await _appointmentRepository.GetUpcomingAppointmentsAsync(
                patientId,
                doctorId,
                pageNumber,
                pageSize);
        }
    }
}