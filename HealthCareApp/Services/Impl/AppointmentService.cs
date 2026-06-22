using AutoMapper;
using HealthCareApp.Constants;
using HealthCareApp.Dtos;
using HealthCareApp.Enums;
using HealthCareApp.Exceptions;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;

namespace HealthCareApp.Services.Impl
{
    public class AppointmentService(
        IAppointmentRepository appointmentRepository,
        IPatientRepository patientRepository,
        IDoctorRepository doctorRepository,
        IHealthRecordRepository healthRecordRepository,
        IMapper mapper) : IAppointmentService
    {
        private const string AppointmentEntityName = "Appointment";
        public async Task<List<AppointmentDto>> GetAllAppointmentsAsync()
        {
            var appointments = await appointmentRepository.GetAllAsync();

            return mapper.Map<List<AppointmentDto>>(appointments);
        }

        public async Task<PagedResponse<AppointmentDto>> GetAllAppointmentsPagedAsync(AppointmentPaginationQueryDto query)
{
            query ??= new AppointmentPaginationQueryDto();

            int pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;

    int pageSize = query.PageSize <= 0 ? 10 : query.PageSize;

    pageSize = pageSize > 100 ? 100 : pageSize;

    var appointments = await appointmentRepository.GetAllAsync();

    var filteredAppointments = appointments.AsEnumerable();

    if (!string.IsNullOrWhiteSpace(query.SearchTerm))
    {
        string searchTerm = query.SearchTerm.Trim();

        filteredAppointments = filteredAppointments.Where(a =>
            (a.Patient != null &&
             a.Patient.PatientName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
            (a.Doctor != null &&
             a.Doctor.DoctorName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
            a.TimeSlot.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            (!string.IsNullOrWhiteSpace(a.CancellationReason) &&
             a.CancellationReason.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
    }

    if (query.PatientId is not null)
    {
        filteredAppointments = filteredAppointments.Where(a =>
            a.PatientId == query.PatientId.Value);
    }

    if (query.DoctorId is not null)
    {
        filteredAppointments = filteredAppointments.Where(a =>
            a.DoctorId == query.DoctorId.Value);
    }

    if (query.Status is not null)
    {
        filteredAppointments = filteredAppointments.Where(a =>
            a.Status == query.Status.Value);
    }

    if (query.ScheduledDate is not null)
    {
        filteredAppointments = filteredAppointments.Where(a =>
            a.ScheduledDate.Date == query.ScheduledDate.Value.Date);
    }

    if (query.UpcomingOnly is not null && query.UpcomingOnly.Value)
    {
        filteredAppointments = filteredAppointments.Where(a =>
            a.ScheduledDate.Date >= DateTime.Today &&
            a.Status != AppointmentStatus.Cancelled &&
            a.Status != AppointmentStatus.Completed);
    }

    int totalRecords = filteredAppointments.Count();

    var pagedAppointments = filteredAppointments
        .OrderByDescending(a => a.ScheduledDate)
        .ThenBy(a => a.TimeSlot)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToList();

    var mappedAppointments = mapper.Map<List<AppointmentDto>>(pagedAppointments);

    return new PagedResponse<AppointmentDto>
    {
        Items = mappedAppointments,
        PageNumber = pageNumber,
        PageSize = pageSize,
        TotalRecords = totalRecords,
        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
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
                throw new AppointmentRuleException("Appointment details are required.");
            }

            await ValidatePatientExistsAsync(dto.PatientId);

            var doctor = await ValidateDoctorExistsAsync(dto.DoctorId);

            ValidateDoctorAvailability(doctor);

            ValidateAppointmentDate(dto.ScheduledDate);

            ValidateTimeSlot(dto.TimeSlot);

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

            var savedAppointment = await appointmentRepository.CreateAsync(appointment);

            return mapper.Map<AppointmentDto>(savedAppointment);
        }

        public async Task<AppointmentDto> UpdateAppointmentAsync(int appointmentId, UpdateAppointmentDto dto)
        {
            ValidateAppointmentId(appointmentId);

            if (dto is null)
            {
                throw new AppointmentRuleException("Appointment details are required.");
            }

            var existingAppointment = await appointmentRepository.GetByIdAsync(appointmentId);

            if (existingAppointment is null)
            {
                throw new EntityNotFoundException("Appointment", appointmentId);
            }

            await ValidatePatientExistsAsync(dto.PatientId);

            var doctor = await ValidateDoctorExistsAsync(dto.DoctorId);

            ValidateDoctorAvailability(doctor);

            ValidateAppointmentDate(dto.ScheduledDate);

            ValidateTimeSlot(dto.TimeSlot);

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

            var updatedAppointment = await appointmentRepository.UpdateAsync(
                appointmentId,
                existingAppointment);

            if (updatedAppointment is null)
            {
                throw new EntityNotFoundException("Appointment", appointmentId);
            }

            return mapper.Map<AppointmentDto>(updatedAppointment);
        }

        public async Task<AppointmentDto> ConfirmAppointmentAsync(int appointmentId)
        {
            ValidateAppointmentId(appointmentId);

            var appointment = await appointmentRepository.GetByIdAsync(appointmentId);

            if (appointment is null)
            {
                throw new EntityNotFoundException("Appointment", appointmentId);
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
                throw new EntityNotFoundException("Appointment", appointmentId);
            }

            return mapper.Map<AppointmentDto>(updatedAppointment);
        }

        public async Task<AppointmentDto> CompleteAppointmentAsync(int appointmentId)
        {
            ValidateAppointmentId(appointmentId);

            var appointment = await appointmentRepository.GetByIdAsync(appointmentId);

            if (appointment is null)
            {
                throw new EntityNotFoundException("Appointment", appointmentId);
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
                throw new EntityNotFoundException("Appointment", appointmentId);
            }

            return mapper.Map<AppointmentDto>(updatedAppointment);
        }

        public async Task<AppointmentDto> CancelAppointmentAsync(CancelAppointmentDto dto)
        {
            if (dto is null)
            {
                throw new AppointmentRuleException("Cancellation details are required.");
            }

            ValidateAppointmentId(dto.AppointmentId);

            ValidateCancellationReason(dto.Reason);

            var appointment = await appointmentRepository.GetByIdAsync(dto.AppointmentId);

            if (appointment is null)
            {
                throw new EntityNotFoundException("Appointment", dto.AppointmentId);
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
                throw new EntityNotFoundException("Appointment", dto.AppointmentId);
            }

            return mapper.Map<AppointmentDto>(updatedAppointment);
        }

        public async Task<AppointmentDto> DeleteAppointmentAsync(int appointmentId)
        {
            ValidateAppointmentId(appointmentId);

            var appointment = await appointmentRepository.GetByIdAsync(appointmentId);

            if (appointment is null)
            {
                throw new EntityNotFoundException("Appointment", appointmentId);
            }

            var hasHealthRecord = await healthRecordRepository.ExistsByAppointmentIdAsync(appointmentId);

            if (hasHealthRecord)
            {
                throw new ConflictException("This appointment cannot be deleted because it has an associated health record.");
            }

            var deletedAppointment = await appointmentRepository.DeleteAsync(appointmentId);

            if (deletedAppointment is null)
            {
                throw new EntityNotFoundException("Appointment", appointmentId);
            }

            return mapper.Map<AppointmentDto>(deletedAppointment);
        }

        public async Task<List<AppointmentDto>> GetMyAppointmentsForPatientAsync(string identityUserId)
        {
            var patient = await GetLoggedInPatientAsync(identityUserId);

            var appointments = await appointmentRepository.GetByPatientIdAsync(patient.PatientId);

            return mapper.Map<List<AppointmentDto>>(appointments);
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
                throw new EntityNotFoundException("Appointment", appointmentId);
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
                throw new AppointmentRuleException("Appointment details are required.");
            }

            var patient = await GetLoggedInPatientAsync(identityUserId);

            // Important ownership fix:
            // Ignore any patientId sent from body and force logged-in patient's PatientId.
            dto.PatientId = patient.PatientId;

            return await BookAppointmentAsync(dto);
        }

        public async Task<AppointmentDto> CancelAppointmentForPatientAsync(
            CancelAppointmentDto dto,
            string identityUserId)
        {
            if (dto is null)
            {
                throw new AppointmentRuleException("Cancellation details are required.");
            }

            ValidateAppointmentId(dto.AppointmentId);

            var patient = await GetLoggedInPatientAsync(identityUserId);

            var appointment = await appointmentRepository.GetByIdAsync(dto.AppointmentId);

            if (appointment is null)
            {
                throw new EntityNotFoundException("Appointment", dto.AppointmentId);
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
                throw new EntityNotFoundException("Appointment", appointmentId);
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
                throw new EntityNotFoundException("Appointment", appointmentId);
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
                throw new EntityNotFoundException("Appointment", appointmentId);
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
                throw new AppointmentRuleException("Cancellation details are required.");
            }

            ValidateAppointmentId(dto.AppointmentId);

            var doctor = await GetLoggedInDoctorAsync(identityUserId);

            var appointment = await appointmentRepository.GetByIdAsync(dto.AppointmentId);

            if (appointment is null)
            {
                throw new EntityNotFoundException("Appointment", dto.AppointmentId);
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

        private async Task ValidatePatientExistsAsync(int patientId)
        {
            ValidatePatientId(patientId);

            var patient = await patientRepository.GetByIdAsync(patientId);

            if (patient is null)
            {
                throw new EntityNotFoundException("Patient", patientId);
            }
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