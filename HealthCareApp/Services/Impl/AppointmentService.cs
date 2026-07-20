using AutoMapper;
using HealthCareApp.Data;
using HealthCareApp.Exceptions;
using HealthCareApp.Messaging.Events;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Services.Interface;
using HealthCareApp.Shared.Constants;
using HealthCareApp.Shared.Dtos.Appointments;
using HealthCareApp.Shared.Dtos.Pagination;
using HealthCareApp.Shared.Enums;
using MassTransit;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace HealthCareApp.Services.Impl
{
    public class AppointmentService : IAppointmentService
    {
        private const string AppointmentEntityName = "Appointment";
        private const string AppointmentDetailsRequiredMessage = "Appointment details are required.";
        private const string CancellationDetailsRequiredMessage = "Cancellation details are required.";
        private const string DateFormat = "yyyy-MM-dd";
        private const string AppointmentBookedEventType = "AppointmentBooked";
        private const string EventStageSavedToOutbox = "SavedToOutbox";

        private const string ConcurrentSlotBookedMessage =
            "This slot was just booked by another patient. Please choose another available slot.";

        private readonly IAppointmentRepository appointmentRepository;
        private readonly IPatientRepository patientRepository;
        private readonly IDoctorRepository doctorRepository;
        private readonly IHealthRecordRepository healthRecordRepository;
        private readonly IDoctorLeaveService doctorLeaveService;
        private readonly IMapper mapper;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly HealthAxisDbContext dbContext;
        private readonly ILogger<AppointmentService> logger;

        public AppointmentService(AppointmentServiceDependencies dependencies)
        {
            appointmentRepository = dependencies.AppointmentRepository;
            patientRepository = dependencies.PatientRepository;
            doctorRepository = dependencies.DoctorRepository;
            healthRecordRepository = dependencies.HealthRecordRepository;
            doctorLeaveService = dependencies.DoctorLeaveService;
            mapper = dependencies.Mapper;
            publishEndpoint = dependencies.PublishEndpoint;
            dbContext = dependencies.DbContext;
            logger = dependencies.Logger;
        }

        public async Task<AppointmentDailyStatusSummaryDto> GetDailyStatusSummaryAsync(
            DateTime date)
        {
            var selectedDate = date.Date;

            var appointments = await appointmentRepository.GetAppointmentsByDateAsync(selectedDate);

            return new AppointmentDailyStatusSummaryDto
            {
                Date = FormatDate(selectedDate),
                Total = appointments.Count,
                Pending = appointments.Count(appointment =>
                    appointment.Status == AppointmentStatus.Pending),
                Confirmed = appointments.Count(appointment =>
                    appointment.Status == AppointmentStatus.Confirmed),
                Completed = appointments.Count(appointment =>
                    appointment.Status == AppointmentStatus.Completed),
                Cancelled = appointments.Count(appointment =>
                    appointment.Status == AppointmentStatus.Cancelled)
            };
        }

        public async Task<AppointmentFilterOptionsDto> GetAppointmentFilterOptionsAsync()
        {
            var appointments = await appointmentRepository.GetAppointmentsForFilterOptionsAsync();

            var patients = appointments
                .Where(appointment => appointment.Patient is not null)
                .GroupBy(appointment => new
                {
                    appointment.PatientId,
                    PatientName = appointment.Patient!.PatientName
                })
                .Select(group => new AppointmentFilterPersonDto
                {
                    Id = group.Key.PatientId,
                    Name = group.Key.PatientName
                })
                .OrderBy(patient => patient.Name)
                .ToList();

            var doctors = appointments
                .Where(appointment => appointment.Doctor is not null)
                .GroupBy(appointment => new
                {
                    appointment.DoctorId,
                    DoctorName = appointment.Doctor!.DoctorName
                })
                .Select(group => new AppointmentFilterPersonDto
                {
                    Id = group.Key.DoctorId,
                    Name = group.Key.DoctorName
                })
                .OrderBy(doctor => doctor.Name)
                .ToList();

            return new AppointmentFilterOptionsDto
            {
                Patients = patients,
                Doctors = doctors
            };
        }

        public async Task<List<AppointmentDto>> GetAllAppointmentsAsync()
        {
            var appointments = await appointmentRepository.GetAllAsync();

            return mapper.Map<List<AppointmentDto>>(appointments);
        }

        public async Task<PagedResponse<AppointmentDto>> GetAllAppointmentsPagedAsync(
            AppointmentPaginationQueryDto query)
        {
            query ??= new AppointmentPaginationQueryDto();

            int pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;

            int pageSize = query.PageSize <= 0 ? 10 : query.PageSize;

            pageSize = pageSize > 100 ? 100 : pageSize;

            query.PageNumber = pageNumber;
            query.PageSize = pageSize;

            var pagedResult = await appointmentRepository.GetPagedAppointmentsAsync(query);

            var mappedAppointments = mapper.Map<List<AppointmentDto>>(pagedResult.Items);

            return new PagedResponse<AppointmentDto>
            {
                Items = mappedAppointments,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = pagedResult.TotalRecords,
                TotalPages = (int)Math.Ceiling(pagedResult.TotalRecords / (double)pageSize)
            };
        }

        public async Task<AppointmentDto> GetAppointmentByIdAsync(int appointmentId)
        {
            ValidateAppointmentId(appointmentId);

            var appointment = await appointmentRepository.GetByIdAsync(appointmentId);

            if (appointment is null)
            {
                throw new EntityNotFoundException(AppointmentEntityName, appointmentId);
            }

            return mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByPatientIdAsync(int patientId)
        {
            await ValidatePatientExistsAsync(patientId);

            var appointments = await appointmentRepository.GetByPatientIdAsync(patientId);

            return mapper.Map<List<AppointmentDto>>(appointments);
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId)
        {
            await ValidateDoctorExistsAsync(doctorId);

            var appointments = await appointmentRepository.GetByDoctorIdAsync(doctorId);

            return mapper.Map<List<AppointmentDto>>(appointments);
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByStatusAsync(AppointmentStatus status)
        {
            var appointments = await appointmentRepository.GetByStatusAsync(status);

            return mapper.Map<List<AppointmentDto>>(appointments);
        }

        public async Task<List<AppointmentDto>> GetUpcomingAppointmentsAsync()
        {
            var appointments = await appointmentRepository.GetUpcomingAppointmentsAsync();

            return mapper.Map<List<AppointmentDto>>(appointments);
        }

        public async Task<List<AppointmentDto>> GetUpcomingAppointmentsByPatientIdAsync(int patientId)
        {
            await ValidatePatientExistsAsync(patientId);

            var appointments = await appointmentRepository.GetUpcomingAppointmentsByPatientIdAsync(patientId);

            return mapper.Map<List<AppointmentDto>>(appointments);
        }

        public async Task<List<AppointmentDto>> GetUpcomingAppointmentsByDoctorIdAsync(int doctorId)
        {
            await ValidateDoctorExistsAsync(doctorId);

            var appointments = await appointmentRepository.GetUpcomingAppointmentsByDoctorIdAsync(doctorId);

            return mapper.Map<List<AppointmentDto>>(appointments);
        }

        public async Task<List<AppointmentDto>> GetPendingAppointmentsByPatientIdAsync(int patientId)
        {
            await ValidatePatientExistsAsync(patientId);

            var appointments = await appointmentRepository.GetPendingAppointmentsByPatientIdAsync(patientId);

            return mapper.Map<List<AppointmentDto>>(appointments);
        }

        public async Task<List<AppointmentDto>> GetPendingAppointmentsByDoctorIdAsync(int doctorId)
        {
            await ValidateDoctorExistsAsync(doctorId);

            var appointments = await appointmentRepository.GetPendingAppointmentsByDoctorIdAsync(doctorId);

            return mapper.Map<List<AppointmentDto>>(appointments);
        }

        public async Task<List<AppointmentDto>> GetTodayConfirmedAppointmentsByDoctorIdAsync(int doctorId)
        {
            await ValidateDoctorExistsAsync(doctorId);

            var appointments = await appointmentRepository.GetTodayConfirmedAppointmentsByDoctorIdAsync(doctorId);

            return mapper.Map<List<AppointmentDto>>(appointments);
        }

        public async Task<AppointmentDto> BookAppointmentAsync(BookAppointmentDto dto)
        {
            if (dto is null)
            {
                throw new AppointmentRuleException(AppointmentDetailsRequiredMessage);
            }

            var patient = await ValidatePatientExistsAsync(dto.PatientId);

            var doctor = await ValidateDoctorExistsAsync(dto.DoctorId);

            ValidateDoctorAvailability(doctor);

            ValidateAppointmentDate(dto.ScheduledDate);

            ValidateTimeSlot(dto.TimeSlot);

            await ValidateDoctorLeaveAvailabilityAsync(
                dto.DoctorId,
                dto.ScheduledDate.Date);

            var isSlotBooked = await appointmentRepository.IsSlotBookedAsync(
                dto.DoctorId,
                dto.ScheduledDate.Date,
                dto.TimeSlot);

            if (isSlotBooked)
            {
                throw new ConflictException("This time slot is already booked for the selected doctor.");
            }

            var patientHasSameSlot = await appointmentRepository.PatientHasActiveAppointmentOnDateAndSlotAsync(
                dto.PatientId,
                dto.ScheduledDate.Date,
                dto.TimeSlot);

            if (patientHasSameSlot)
            {
                throw new ConflictException("Patient already has an active appointment in this time slot.");
            }

            var patientHasAppointmentWithDoctor = await appointmentRepository.PatientHasActiveAppointmentWithDoctorOnDateAsync(
                dto.PatientId,
                dto.DoctorId,
                dto.ScheduledDate.Date);

            if (patientHasAppointmentWithDoctor)
            {
                throw new ConflictException("Patient already has an active appointment with this doctor on the selected date.");
            }

            var appointment = mapper.Map<Appointment>(dto);

            appointment.ScheduledDate = dto.ScheduledDate.Date;
            appointment.Status = AppointmentStatus.Pending;
            appointment.CancellationReason = null;
            appointment.CreatedDate = DateTime.Now;

            Appointment savedAppointment;

            await using var transaction =
                await dbContext.Database.BeginTransactionAsync();

            try
            {
                savedAppointment = await appointmentRepository.CreateAsync(appointment);

                var appointmentBookedEvent = new AppointmentBookedEvent
                {
                    AppointmentId = savedAppointment.AppointmentId,
                    PatientName = patient.PatientName,
                    DoctorId = savedAppointment.DoctorId,
                    ScheduledDate = savedAppointment.ScheduledDate.Date,
                    TimeSlot = savedAppointment.TimeSlot
                };

                var outboxMessageId = Guid.NewGuid();

                var outboxMessage = new OutboxMessage
                {
                    OutboxMessageId = outboxMessageId,
                    EventType = nameof(AppointmentBookedEvent),
                    Payload = JsonSerializer.Serialize(appointmentBookedEvent),
                    CreatedDate = DateTime.Now,
                    PublishedDate = null,
                    Status = OutboxMessageStatuses.Pending,
                    RetryCount = 0,
                    ErrorMessage = null
                };

                await dbContext.OutboxMessages.AddAsync(outboxMessage);

                await dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                LogAppointmentBookedEventSavedToOutbox(
                    savedAppointment,
                    outboxMessageId);
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();

                if (IsUniqueAppointmentSlotViolation(ex))
                {
                    LogConcurrentAppointmentBookingBlocked(
                        ex,
                        dto);

                    throw new ConflictException(ConcurrentSlotBookedMessage);
                }

                throw;
            }
            catch
            {
                await transaction.RollbackAsync();

                throw;
            }

            return mapper.Map<AppointmentDto>(savedAppointment);
        }

        public async Task<AppointmentDto> UpdateAppointmentAsync(int appointmentId, UpdateAppointmentDto dto)
        {
            ValidateAppointmentId(appointmentId);

            if (dto is null)
            {
                throw new AppointmentRuleException(AppointmentDetailsRequiredMessage);
            }

            var existingAppointment = await appointmentRepository.GetByIdAsync(appointmentId);

            if (existingAppointment is null)
            {
                throw new EntityNotFoundException(AppointmentEntityName, appointmentId);
            }

            await ValidatePatientExistsAsync(dto.PatientId);

            var doctor = await ValidateDoctorExistsAsync(dto.DoctorId);

            ValidateDoctorAvailability(doctor);

            ValidateAppointmentDate(dto.ScheduledDate);

            ValidateTimeSlot(dto.TimeSlot);

            await ValidateDoctorLeaveAvailabilityAsync(
                dto.DoctorId,
                dto.ScheduledDate.Date);

            bool slotTaken = await appointmentRepository.IsSlotBookedAsync(
                dto.DoctorId,
                dto.ScheduledDate.Date,
                dto.TimeSlot);

            bool sameExistingSlot =
                existingAppointment.DoctorId == dto.DoctorId &&
                existingAppointment.ScheduledDate.Date == dto.ScheduledDate.Date &&
                existingAppointment.TimeSlot == dto.TimeSlot;

            if (slotTaken && !sameExistingSlot)
            {
                throw new ConflictException("This time slot is already booked for the selected doctor.");
            }

            mapper.Map(dto, existingAppointment);

            existingAppointment.AppointmentId = appointmentId;
            existingAppointment.ScheduledDate = dto.ScheduledDate.Date;

            Appointment? updatedAppointment;

            try
            {
                updatedAppointment = await appointmentRepository.UpdateAsync(
                    appointmentId,
                    existingAppointment);
            }
            catch (DbUpdateException ex) when (IsUniqueAppointmentSlotViolation(ex))
            {
                LogAppointmentUpdateBlocked(
                    ex,
                    appointmentId,
                    dto);

                throw new ConflictException(ConcurrentSlotBookedMessage);
            }

            if (updatedAppointment is null)
            {
                throw new EntityNotFoundException(AppointmentEntityName, appointmentId);
            }

            return mapper.Map<AppointmentDto>(updatedAppointment);
        }

        public async Task<AppointmentDto> ConfirmAppointmentAsync(int appointmentId)
        {
            ValidateAppointmentId(appointmentId);

            var appointment = await appointmentRepository.GetByIdAsync(appointmentId);

            if (appointment is null)
            {
                throw new EntityNotFoundException(AppointmentEntityName, appointmentId);
            }

            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new ConflictException("Cancelled appointment cannot be confirmed.");
            }

            if (appointment.Status == AppointmentStatus.Completed)
            {
                throw new ConflictException("Completed appointment cannot be confirmed again.");
            }

            if (appointment.Status == AppointmentStatus.Confirmed)
            {
                throw new ConflictException("Appointment is already confirmed.");
            }

            appointment.Status = AppointmentStatus.Confirmed;
            appointment.CancellationReason = null;

            var updatedAppointment = await appointmentRepository.UpdateAsync(
                appointmentId,
                appointment);

            if (updatedAppointment is null)
            {
                throw new EntityNotFoundException(AppointmentEntityName, appointmentId);
            }

            return mapper.Map<AppointmentDto>(updatedAppointment);
        }

        public async Task<AppointmentDto> CompleteAppointmentAsync(int appointmentId)
        {
            ValidateAppointmentId(appointmentId);

            var appointment = await appointmentRepository.GetByIdAsync(appointmentId);

            if (appointment is null)
            {
                throw new EntityNotFoundException(AppointmentEntityName, appointmentId);
            }

            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new ConflictException("Cancelled appointment cannot be completed.");
            }

            if (appointment.Status == AppointmentStatus.Completed)
            {
                throw new ConflictException("Appointment is already completed.");
            }

            if (appointment.Status != AppointmentStatus.Confirmed)
            {
                throw new AppointmentRuleException("Only confirmed appointments can be completed.");
            }

            appointment.Status = AppointmentStatus.Completed;

            var updatedAppointment = await appointmentRepository.UpdateAsync(
                appointmentId,
                appointment);

            if (updatedAppointment is null)
            {
                throw new EntityNotFoundException(AppointmentEntityName, appointmentId);
            }

            return mapper.Map<AppointmentDto>(updatedAppointment);
        }

        public async Task<AppointmentDto> CancelAppointmentAsync(CancelAppointmentDto dto)
        {
            if (dto is null)
            {
                throw new AppointmentRuleException(CancellationDetailsRequiredMessage);
            }

            ValidateAppointmentId(dto.AppointmentId);

            ValidateCancellationReason(dto.Reason);

            var appointment = await appointmentRepository.GetByIdAsync(dto.AppointmentId);

            if (appointment is null)
            {
                throw new EntityNotFoundException(AppointmentEntityName, dto.AppointmentId);
            }

            if (appointment.Status == AppointmentStatus.Completed)
            {
                throw new ConflictException("Completed appointment cannot be cancelled.");
            }

            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new ConflictException("Appointment is already cancelled.");
            }

            appointment.Status = AppointmentStatus.Cancelled;
            appointment.CancellationReason = dto.Reason.Trim();

            var updatedAppointment = await appointmentRepository.UpdateAsync(
                dto.AppointmentId,
                appointment);

            if (updatedAppointment is null)
            {
                throw new EntityNotFoundException(AppointmentEntityName, dto.AppointmentId);
            }

            return mapper.Map<AppointmentDto>(updatedAppointment);
        }

        public async Task<AppointmentDto> DeleteAppointmentAsync(int appointmentId)
        {
            ValidateAppointmentId(appointmentId);

            var appointment = await appointmentRepository.GetByIdAsync(appointmentId);

            if (appointment is null)
            {
                throw new EntityNotFoundException(AppointmentEntityName, appointmentId);
            }

            var hasHealthRecord = await healthRecordRepository.ExistsByAppointmentIdAsync(appointmentId);

            if (hasHealthRecord)
            {
                throw new ConflictException("This appointment cannot be deleted because it has an associated health record.");
            }

            var deletedAppointment = await appointmentRepository.DeleteAsync(appointmentId);

            if (deletedAppointment is null)
            {
                throw new EntityNotFoundException(AppointmentEntityName, appointmentId);
            }

            return mapper.Map<AppointmentDto>(deletedAppointment);
        }

        public async Task<List<AppointmentDto>> GetMyAppointmentsForPatientAsync(string identityUserId)
        {
            var patient = await GetLoggedInPatientAsync(identityUserId);

            var appointments = await appointmentRepository.GetByPatientIdAsync(patient.PatientId);

            return mapper.Map<List<AppointmentDto>>(appointments);
        }

        public async Task<PagedResponse<AppointmentDto>> GetMyAppointmentsForPatientPagedAsync(
            string identityUserId,
            AppointmentPaginationQueryDto query)
        {
            var patient = await GetLoggedInPatientAsync(identityUserId);

            query ??= new AppointmentPaginationQueryDto();

            int pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;

            int pageSize = query.PageSize <= 0 ? 10 : query.PageSize;

            pageSize = pageSize > 100 ? 100 : pageSize;

            query.PatientId = patient.PatientId;
            query.PageNumber = pageNumber;
            query.PageSize = pageSize;

            var pagedResult = await appointmentRepository.GetPagedAppointmentsAsync(query);

            var mappedAppointments = mapper.Map<List<AppointmentDto>>(pagedResult.Items);

            return new PagedResponse<AppointmentDto>
            {
                Items = mappedAppointments,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = pagedResult.TotalRecords,
                TotalPages = (int)Math.Ceiling(pagedResult.TotalRecords / (double)pageSize)
            };
        }

        public async Task<PagedResponse<AppointmentDto>> GetMyAppointmentsForDoctorPagedAsync(
            string identityUserId,
            AppointmentPaginationQueryDto query)
        {
            var doctor = await GetLoggedInDoctorAsync(identityUserId);

            query ??= new AppointmentPaginationQueryDto();

            int pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;

            int pageSize = query.PageSize <= 0 ? 10 : query.PageSize;

            pageSize = pageSize > 100 ? 100 : pageSize;

            query.DoctorId = doctor.DoctorId;
            query.PageNumber = pageNumber;
            query.PageSize = pageSize;

            var pagedResult = await appointmentRepository.GetPagedAppointmentsAsync(query);

            var mappedAppointments = mapper.Map<List<AppointmentDto>>(pagedResult.Items);

            return new PagedResponse<AppointmentDto>
            {
                Items = mappedAppointments,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = pagedResult.TotalRecords,
                TotalPages = (int)Math.Ceiling(pagedResult.TotalRecords / (double)pageSize)
            };
        }

        public async Task<List<AppointmentDto>> GetMyUpcomingAppointmentsForPatientAsync(string identityUserId)
        {
            var patient = await GetLoggedInPatientAsync(identityUserId);

            var appointments = await appointmentRepository.GetUpcomingAppointmentsByPatientIdAsync(patient.PatientId);

            return mapper.Map<List<AppointmentDto>>(appointments);
        }

        public async Task<List<AppointmentDto>> GetMyPendingAppointmentsForPatientAsync(string identityUserId)
        {
            var patient = await GetLoggedInPatientAsync(identityUserId);

            var appointments = await appointmentRepository.GetPendingAppointmentsByPatientIdAsync(patient.PatientId);

            return mapper.Map<List<AppointmentDto>>(appointments);
        }

        public async Task<AppointmentDto> GetAppointmentByIdForPatientAsync(
            int appointmentId,
            string identityUserId)
        {
            ValidateAppointmentId(appointmentId);

            var patient = await GetLoggedInPatientAsync(identityUserId);

            var appointment = await appointmentRepository.GetByIdAsync(appointmentId);

            if (appointment is null)
            {
                throw new EntityNotFoundException(AppointmentEntityName, appointmentId);
            }

            if (appointment.PatientId != patient.PatientId)
            {
                throw new ForbiddenAccessException("Patients can access only their own appointments.");
            }

            return mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<AppointmentDto> BookAppointmentForPatientAsync(
            BookAppointmentDto dto,
            string identityUserId)
        {
            if (dto is null)
            {
                throw new AppointmentRuleException(AppointmentDetailsRequiredMessage);
            }

            var patient = await GetLoggedInPatientAsync(identityUserId);

            dto.PatientId = patient.PatientId;

            return await BookAppointmentAsync(dto);
        }

        public async Task<AppointmentDto> CancelAppointmentForPatientAsync(
            CancelAppointmentDto dto,
            string identityUserId)
        {
            if (dto is null)
            {
                throw new AppointmentRuleException(CancellationDetailsRequiredMessage);
            }

            ValidateAppointmentId(dto.AppointmentId);

            var patient = await GetLoggedInPatientAsync(identityUserId);

            var appointment = await appointmentRepository.GetByIdAsync(dto.AppointmentId);

            if (appointment is null)
            {
                throw new EntityNotFoundException(AppointmentEntityName, dto.AppointmentId);
            }

            if (appointment.PatientId != patient.PatientId)
            {
                throw new ForbiddenAccessException("Patients can cancel only their own appointments.");
            }

            return await CancelAppointmentAsync(dto);
        }

        public async Task<List<AppointmentDto>> GetMyAppointmentsForDoctorAsync(string identityUserId)
        {
            var doctor = await GetLoggedInDoctorAsync(identityUserId);

            var appointments = await appointmentRepository.GetByDoctorIdAsync(doctor.DoctorId);

            return mapper.Map<List<AppointmentDto>>(appointments);
        }

        public async Task<List<AppointmentDto>> GetMyUpcomingAppointmentsForDoctorAsync(string identityUserId)
        {
            var doctor = await GetLoggedInDoctorAsync(identityUserId);

            var appointments = await appointmentRepository.GetUpcomingAppointmentsByDoctorIdAsync(doctor.DoctorId);

            return mapper.Map<List<AppointmentDto>>(appointments);
        }

        public async Task<List<AppointmentDto>> GetMyPendingAppointmentsForDoctorAsync(string identityUserId)
        {
            var doctor = await GetLoggedInDoctorAsync(identityUserId);

            var appointments = await appointmentRepository.GetPendingAppointmentsByDoctorIdAsync(doctor.DoctorId);

            return mapper.Map<List<AppointmentDto>>(appointments);
        }

        public async Task<List<AppointmentDto>> GetMyTodayConfirmedAppointmentsForDoctorAsync(string identityUserId)
        {
            var doctor = await GetLoggedInDoctorAsync(identityUserId);

            var appointments = await appointmentRepository.GetTodayConfirmedAppointmentsByDoctorIdAsync(doctor.DoctorId);

            return mapper.Map<List<AppointmentDto>>(appointments);
        }

        public async Task<AppointmentDto> GetAppointmentByIdForDoctorAsync(
            int appointmentId,
            string identityUserId)
        {
            ValidateAppointmentId(appointmentId);

            var doctor = await GetLoggedInDoctorAsync(identityUserId);

            var appointment = await appointmentRepository.GetByIdAsync(appointmentId);

            if (appointment is null)
            {
                throw new EntityNotFoundException(AppointmentEntityName, appointmentId);
            }

            if (appointment.DoctorId != doctor.DoctorId)
            {
                throw new ForbiddenAccessException("Doctors can access only their own appointments.");
            }

            return mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<AppointmentDto> ConfirmAppointmentForDoctorAsync(
            int appointmentId,
            string identityUserId)
        {
            ValidateAppointmentId(appointmentId);

            var doctor = await GetLoggedInDoctorAsync(identityUserId);

            var appointment = await appointmentRepository.GetByIdAsync(appointmentId);

            if (appointment is null)
            {
                throw new EntityNotFoundException(AppointmentEntityName, appointmentId);
            }

            if (appointment.DoctorId != doctor.DoctorId)
            {
                throw new ForbiddenAccessException("Doctors can confirm only their own appointments.");
            }

            return await ConfirmAppointmentAsync(appointmentId);
        }

        public async Task<AppointmentDto> CompleteAppointmentForDoctorAsync(
            int appointmentId,
            string identityUserId)
        {
            ValidateAppointmentId(appointmentId);

            var doctor = await GetLoggedInDoctorAsync(identityUserId);

            var appointment = await appointmentRepository.GetByIdAsync(appointmentId);

            if (appointment is null)
            {
                throw new EntityNotFoundException(AppointmentEntityName, appointmentId);
            }

            if (appointment.DoctorId != doctor.DoctorId)
            {
                throw new ForbiddenAccessException("Doctors can complete only their own appointments.");
            }

            return await CompleteAppointmentAsync(appointmentId);
        }

        public async Task<AppointmentDto> CancelAppointmentForDoctorAsync(
            CancelAppointmentDto dto,
            string identityUserId)
        {
            if (dto is null)
            {
                throw new AppointmentRuleException(CancellationDetailsRequiredMessage);
            }

            ValidateAppointmentId(dto.AppointmentId);

            var doctor = await GetLoggedInDoctorAsync(identityUserId);

            var appointment = await appointmentRepository.GetByIdAsync(dto.AppointmentId);

            if (appointment is null)
            {
                throw new EntityNotFoundException(AppointmentEntityName, dto.AppointmentId);
            }

            if (appointment.DoctorId != doctor.DoctorId)
            {
                throw new ForbiddenAccessException("Doctors can cancel only their own appointments.");
            }

            return await CancelAppointmentAsync(dto);
        }

        private async Task<Doctor> GetLoggedInDoctorAsync(string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                throw new BusinessRuleException("Invalid logged-in user.");
            }

            var doctor = await doctorRepository.GetByIdentityUserIdAsync(identityUserId);

            if (doctor is null)
            {
                throw new EntityNotFoundException("Doctor profile for logged-in user", 0);
            }

            return doctor;
        }

        private async Task<Patient> GetLoggedInPatientAsync(string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                throw new BusinessRuleException("Invalid logged-in user.");
            }

            var patient = await patientRepository.GetByIdentityUserIdAsync(identityUserId);

            if (patient is null)
            {
                throw new EntityNotFoundException("Patient profile for logged-in user", 0);
            }

            return patient;
        }

        private void LogConcurrentAppointmentBookingBlocked(
            DbUpdateException exception,
            BookAppointmentDto dto)
        {
            if (!logger.IsEnabled(LogLevel.Warning))
            {
                return;
            }

            logger.LogWarning(
                exception,
                "Concurrent appointment booking blocked by unique active slot index. DoctorId: {DoctorId}, ScheduledDate: {ScheduledDate}, TimeSlot: {TimeSlot}, PatientId: {PatientId}",
                dto.DoctorId,
                FormatDate(dto.ScheduledDate),
                dto.TimeSlot,
                dto.PatientId);
        }

        private void LogAppointmentUpdateBlocked(
            DbUpdateException exception,
            int appointmentId,
            UpdateAppointmentDto dto)
        {
            if (!logger.IsEnabled(LogLevel.Warning))
            {
                return;
            }

            logger.LogWarning(
                exception,
                "Appointment update blocked by unique active slot index. AppointmentId: {AppointmentId}, DoctorId: {DoctorId}, ScheduledDate: {ScheduledDate}, TimeSlot: {TimeSlot}",
                appointmentId,
                dto.DoctorId,
                FormatDate(dto.ScheduledDate),
                dto.TimeSlot);
        }

        private void LogAppointmentBookedEventSavedToOutbox(
            Appointment appointment,
            Guid outboxMessageId)
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            using var appointmentBookedEventLogScope =
                BeginAppointmentBookedEventLogScope(
                    appointment,
                    outboxMessageId);

            logger.LogInformation(
                "Appointment booked event saved to outbox. EventStage: {EventStage}, OutboxMessageId: {OutboxMessageId}",
                EventStageSavedToOutbox,
                outboxMessageId);
        }

        private IDisposable? BeginAppointmentBookedEventLogScope(
            Appointment appointment,
            Guid outboxMessageId)
        {
            return logger.BeginScope(new Dictionary<string, object>
            {
                ["EventType"] = AppointmentBookedEventType,
                ["EventStage"] = EventStageSavedToOutbox,
                ["AppointmentId"] = appointment.AppointmentId,
                ["PatientId"] = appointment.PatientId,
                ["DoctorId"] = appointment.DoctorId,
                ["ScheduledDate"] = FormatDate(appointment.ScheduledDate),
                ["TimeSlot"] = appointment.TimeSlot,
                ["OutboxMessageId"] = outboxMessageId
            });
        }

        private static bool IsUniqueAppointmentSlotViolation(DbUpdateException exception)
        {
            Exception? currentException = exception;

            while (currentException is not null)
            {
                if (currentException is SqlException sqlException)
                {
                    return sqlException.Number == 2601 ||
                           sqlException.Number == 2627;
                }

                currentException = currentException.InnerException;
            }

            return false;
        }

        private static string FormatDate(DateTime date)
        {
            return date.ToString(DateFormat);
        }

        private static void ValidateAppointmentId(int appointmentId)
        {
            if (appointmentId <= 0)
            {
                throw new AppointmentRuleException("Please provide a valid appointment reference.");
            }
        }

        private static void ValidatePatientId(int patientId)
        {
            if (patientId <= 0)
            {
                throw new AppointmentRuleException("Please provide a valid patient reference.");
            }
        }

        private static void ValidateDoctorId(int doctorId)
        {
            if (doctorId <= 0)
            {
                throw new AppointmentRuleException("Please provide a valid doctor reference.");
            }
        }

        private async Task<Patient> ValidatePatientExistsAsync(int patientId)
        {
            ValidatePatientId(patientId);

            var patient = await patientRepository.GetByIdAsync(patientId);

            if (patient is null)
            {
                throw new EntityNotFoundException("Patient", patientId);
            }

            return patient;
        }

        private async Task<Doctor> ValidateDoctorExistsAsync(int doctorId)
        {
            ValidateDoctorId(doctorId);

            var doctor = await doctorRepository.GetByIdAsync(doctorId);

            if (doctor is null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            return doctor;
        }

        private static void ValidateDoctorAvailability(Doctor doctor)
        {
            if (!doctor.IsActive)
            {
                throw new AppointmentRuleException("Doctor is inactive. Appointment cannot be booked.");
            }
        }

        private async Task ValidateDoctorLeaveAvailabilityAsync(
            int doctorId,
            DateTime scheduledDate)
        {
            var doctorOnLeave = await doctorLeaveService.IsDoctorOnLeaveAsync(
                doctorId,
                scheduledDate.Date);

            if (doctorOnLeave)
            {
                throw new AppointmentRuleException(
                    "Doctor is on leave on the selected date. Appointment cannot be booked.");
            }
        }

        private static void ValidateAppointmentDate(DateTime scheduledDate)
        {
            if (scheduledDate.Date < DateTime.Today)
            {
                throw new AppointmentRuleException("Appointment date cannot be in the past.");
            }
        }

        private static void ValidateTimeSlot(string timeSlot)
        {
            if (string.IsNullOrWhiteSpace(timeSlot))
            {
                throw new AppointmentRuleException("Time slot is required.");
            }

            if (!TimeSlots.Slots.Contains(timeSlot))
            {
                throw new AppointmentRuleException("Invalid time slot selected.");
            }
        }

        private static void ValidateCancellationReason(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new AppointmentRuleException("Cancellation reason is required.");
            }

            if (reason.Trim().Length > 200)
            {
                throw new AppointmentRuleException("Cancellation reason cannot exceed 200 characters.");
            }
        }
    }
}