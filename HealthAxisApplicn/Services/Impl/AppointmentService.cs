using AutoMapper;
using HealthAxisApplicn.Dto.Appointments;
using HealthAxisApplicn.Messaging.Contracts;
using HealthAxisApplicn.Models;
using HealthAxisApplicn.Repositories;
using MassTransit;
using System.Globalization;

namespace HealthAxisApplicn.Services.Impl
{

    public class AppointmentService(
        IAppointmentRepository repository,
        IHealthRecordService healthRecordService,
        IMapper mapper,
        IPublishEndpoint publishEndpoint)
        : IAppointmentService
    {
        private const string Pending = "Pending";
        private const string Confirmed = "Confirmed";
        private const string Cancelled = "Cancelled";
        private const string Completed = "Completed";
        private const string PatientRole = "Patient";
        private const string DoctorRole = "Doctor";
        public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto entity, int patientId)
        {
            //(no past booking)
            if (entity.ScheduledDate.Date < DateTime.UtcNow.Date)
                throw new InvalidOperationException("Cannot book appointment in the past");

            //Validate time format

            if (!TimeSpan.TryParse(
                    entity.TimeSlot,
                    CultureInfo.InvariantCulture,
                    out _))
            {
                throw new ArgumentException("Invalid time format. Use HH:mm");
            }


            //Doctor availability check
            var doctorConflict = await repository.DoctorHasConflictAsync(
                entity.DoctorId,
                entity.ScheduledDate,
                entity.TimeSlot
            );

            if (doctorConflict)
                throw new InvalidOperationException("Doctor already booked for this time slot");

            //Patient same-slot conflict
            var patientConflict = await repository.PatientHasConflictAsync(
                patientId,
                entity.ScheduledDate,
                entity.TimeSlot
            );

            if (patientConflict)
                throw new InvalidOperationException("You already have an appointment at this time");

            //limit one appointment per day
            var dailyLimit = await repository.PatientHasAppointmentOnDateAsync(
                patientId,
                entity.ScheduledDate
            );

            if (dailyLimit)
                throw new InvalidOperationException("You already have an appointment on this date");

            //Create entity
            var appointment = mapper.Map<Appointment>(entity);

            appointment.PatientId = patientId;

            //add status
            appointment.Status = Pending;

            var savedEntity = await repository.CreateAsync(appointment);

            await publishEndpoint.Publish(
                new BookAppointmentEvent
                {
                    EventId = Guid.NewGuid(),

                    EventType = "AppointmentBooked",

                    OccurredAt = DateTime.UtcNow,

                    Source = "HealthAxis.API",

                    AppointmentId = savedEntity.AppointmentId,

                    PatientId = savedEntity.PatientId,

                    DoctorId = savedEntity.DoctorId,

                    ScheduledDate = savedEntity.ScheduledDate,

                    TimeSlot = savedEntity.TimeSlot
                });

            return mapper.Map<AppointmentDto>(savedEntity);
        }


        public async Task<bool> DeleteAppointmentAsync(int appointmentId)
        {
            var deleted = await repository.DeleteAsync(appointmentId);
            return deleted;
        }

        public async Task<List<AppointmentDto>> GetAllAsync(int page,int pageSize)
        {
            return mapper.Map<List<AppointmentDto>>(
                await repository.GetAllAppointmentsAsync(
                    page,
                    pageSize));
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId,int page,int pageSize)
        {
            return mapper.Map<List<AppointmentDto>>(
                await repository.GetUpcomingAppointmentsByDoctorIdAsync(
                    doctorId,
                    page,
                    pageSize));
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByPatientIdAsync(int patientId, int page, int pageSize)
        {
            return mapper.Map<List<AppointmentDto>>(
                await repository.GetAppointmentsByPatientIdAsync(
                    patientId,
                    page,
                    pageSize));
        }
        public async Task<List<AppointmentDto>> GetAppointmentsByDoctorNameAsync(string doctorName)
        {
            return mapper.Map<List<AppointmentDto>>(await repository.GetAppointmentsByDoctorNameAsync(doctorName));
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByPatientNameAsync(string patientName)
        {
            return mapper.Map<List<AppointmentDto>>(await repository.GetAppointmentsByPatientNameAsync(patientName));
        }

        public async Task<AppointmentDto?> GetByIdAsync(int id)
        {
            var appointment = await repository.GetByIdAsync(id);
            return mapper.Map<AppointmentDto?>(appointment);
        }

        public async Task<AppointmentDto?> UpdateAsync(int id, UpdateAppointmentStatusDto entity, string role)
        {
            var existing = await repository.GetByIdAsync(id);

            if (existing == null)
                return null;

            var previousStatus = existing.Status;

            // ✅ BLOCK invalid base cases
            if (previousStatus == Completed)
                throw new InvalidOperationException("Completed appointment cannot be modified");

            if (previousStatus == Cancelled)
                throw new InvalidOperationException("Cancelled appointment cannot be modified");

            // ✅ VALID STATUS CHECK
            if (!new[] { Pending, Confirmed, Cancelled, Completed }
                .Contains(entity.Status))
            {
                throw new InvalidOperationException("Invalid status value");
            }

            // ✅ PATIENT RESTRICTION
            if (role == PatientRole)
            {
                if (entity.Status != Cancelled)
                    throw new InvalidOperationException("Patient can only cancel appointments");

                if (previousStatus != Pending)
                    throw new InvalidOperationException("Patient can only cancel pending appointments");
            }

            // ✅ STATE TRANSITIONS (for doctor)
            if (role == DoctorRole)
            {
                if (previousStatus == Pending && entity.Status != Confirmed && entity.Status != Cancelled)
                {
                    throw new InvalidOperationException(
                        "Pending appointment can only be Confirmed or Cancelled");
                }

                if (previousStatus == Confirmed && entity.Status != Completed && entity.Status != Cancelled)
                {
                    throw new InvalidOperationException(
                        "Confirmed appointment can only be Completed or Cancelled");
                }
            }

            // ✅ CANCELLATION RULE
            if (entity.Status == Cancelled)
            {
                if (string.IsNullOrWhiteSpace(entity.CancellationReason))
                    throw new InvalidOperationException("Cancellation reason is required");

                existing.CancellationReason = entity.CancellationReason;
            }
            else
            {
                existing.CancellationReason = null;
            }

            // ✅ UPDATE
            existing.Status = entity.Status;

            var updated = await repository.UpdateAsync(id, existing);

            // ✅ HEALTH RECORD CREATION
            if (previousStatus != Completed && entity.Status == Completed)
            {
                await healthRecordService.CreateFromAppointment(existing);
            }

            return mapper.Map<AppointmentDto>(updated);
        }

        public async Task<List<AppointmentDto>> GetTodayAppointmentsAsync(int doctorId,int page,int pageSize)
        {
            var appointments =
                await repository.GetTodayAppointmentsAsync(
                    doctorId,
                    page,
                    pageSize);

            return mapper.Map<List<AppointmentDto>>(appointments);
        }

    }
}
