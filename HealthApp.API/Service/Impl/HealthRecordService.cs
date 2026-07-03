using AutoMapper;
using HealthApp.API.Exceptions;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Interface;
using HealthApp.Shared.Constants;
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HealthApp.API.Service.Impl;

public class HealthRecordService(
    IHealthRecordRepository healthRecordRepository,
    IPatientRepository patientRepository,
    IDoctorRepository doctorRepository,
    IAppointmentRepository appointmentRepository,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper) : IHealthRecordService
{
    public async Task<List<HealthRecordDto>> GetAllHealthRecordsAsync()
    {
        if (IsAdmin())
        {
            throw new ForbiddenAccessException(
                "Admins are not allowed to access private health records.");
        }

        if (IsPatient())
        {
            var patient = await GetLoggedInPatientAsync();

            return mapper.Map<List<HealthRecordDto>>(
                await healthRecordRepository.GetByPatientIdAsync(patient.PatientId));
        }

        if (IsDoctor())
        {
            var accessiblePatientIds = await GetAccessiblePatientIdsForLoggedInDoctorAsync();

            var records = new List<HealthRecord>();

            foreach (var patientId in accessiblePatientIds)
            {
                var patientRecords = await healthRecordRepository.GetByPatientIdAsync(patientId);
                records.AddRange(patientRecords);
            }

            return mapper.Map<List<HealthRecordDto>>(records);
        }

        throw new ForbiddenAccessException(
            "You are not allowed to access health records.");
    }

    public async Task<HealthRecordDto> GetHealthRecordByIdAsync(int healthRecordId)
    {
        if (healthRecordId <= 0)
        {
            throw new HealthRecordRuleException("Invalid health record.");
        }

        var healthRecord = await healthRecordRepository.GetByIdAsync(healthRecordId)
            ?? throw new EntityNotFoundException("HealthRecord", healthRecordId);

        await EnsureHealthRecordViewAccessAsync(healthRecord);

        return mapper.Map<HealthRecordDto>(healthRecord);
    }

    public async Task<List<HealthRecordDto>> GetHealthRecordsByPatientIdAsync(
        int patientId)
    {
        if (patientId <= 0)
        {
            throw new HealthRecordRuleException("Invalid patient.");
        }

        if (await patientRepository.GetByIdAsync(patientId) is null)
        {
            throw new EntityNotFoundException("Patient", patientId);
        }

        await EnsureHealthRecordPatientViewAccessAsync(patientId);

        return mapper.Map<List<HealthRecordDto>>(
            await healthRecordRepository.GetByPatientIdAsync(patientId));
    }

    public async Task<HealthRecordDto> AddHealthRecordAsync(
        AddHealthRecordDto dto)
    {
        if (dto is null)
        {
            throw new HealthRecordRuleException(
                "Health record details are required.");
        }

        if (!IsDoctor())
        {
            throw new ForbiddenAccessException(
                "Only doctors are allowed to create health records.");
        }

        var loggedInDoctor = await GetLoggedInDoctorAsync();

        var appointment = await appointmentRepository.GetByIdAsync(dto.AppointmentId)
            ?? throw new EntityNotFoundException(
                "Appointment",
                dto.AppointmentId);

        if (appointment.DoctorId != loggedInDoctor.DoctorId)
        {
            throw new ForbiddenAccessException(
                "You are not allowed to create a health record for another doctor's appointment.");
        }

        if (appointment.PatientId != dto.PatientId)
        {
            throw new HealthRecordRuleException(
                "Appointment does not belong to selected patient.");
        }

        if (appointment.Status != AppointmentStatus.Confirmed.ToString())
        {
            throw new HealthRecordRuleException(
                "Health record can be added only for confirmed appointments.");
        }

        if (await healthRecordRepository.ExistsByAppointmentIdAsync(dto.AppointmentId))
        {
            throw new ConflictException(
                "Health record already exists for this appointment.");
        }

        var healthRecord = mapper.Map<HealthRecord>(dto);

        healthRecord.PatientId = appointment.PatientId;
        healthRecord.DoctorId = appointment.DoctorId;
        healthRecord.AppointmentId = appointment.AppointmentId;
        healthRecord.CreatedDate = DateTime.Now;

        var saved = await healthRecordRepository.AddAsync(healthRecord);

        appointment.Status = AppointmentStatus.Completed.ToString();

        await appointmentRepository.UpdateAsync(
            appointment.AppointmentId,
            appointment);

        return mapper.Map<HealthRecordDto>(saved);
    }

    private async Task EnsureHealthRecordPatientViewAccessAsync(int requestedPatientId)
    {
        if (IsAdmin())
        {
            throw new ForbiddenAccessException(
                "Admins are not allowed to access private health records.");
        }

        if (IsPatient())
        {
            var loggedInPatient = await GetLoggedInPatientAsync();

            if (loggedInPatient.PatientId != requestedPatientId)
            {
                throw new ForbiddenAccessException(
                    "You are not allowed to access another patient's health records.");
            }

            return;
        }

        if (IsDoctor())
        {
            var hasAccess = await DoctorHasPatientAccessAsync(requestedPatientId);

            if (!hasAccess)
            {
                throw new ForbiddenAccessException(
                    "You are not allowed to access health records for this patient.");
            }

            return;
        }

        throw new ForbiddenAccessException(
            "You are not allowed to access health records.");
    }

    private async Task EnsureHealthRecordViewAccessAsync(HealthRecord healthRecord)
    {
        if (IsAdmin())
        {
            throw new ForbiddenAccessException(
                "Admins are not allowed to access private health records.");
        }

        var patientId = GetHealthRecordPatientId(healthRecord);

        if (IsPatient())
        {
            var loggedInPatient = await GetLoggedInPatientAsync();

            if (loggedInPatient.PatientId != patientId)
            {
                throw new ForbiddenAccessException(
                    "You are not allowed to access another patient's health record.");
            }

            return;
        }

        if (IsDoctor())
        {
            var hasAccess = await DoctorHasPatientAccessAsync(patientId);

            if (!hasAccess)
            {
                throw new ForbiddenAccessException(
                    "You are not allowed to access this patient's health record.");
            }

            return;
        }

        throw new ForbiddenAccessException(
            "You are not allowed to access this health record.");
    }

    private async Task<bool> DoctorHasPatientAccessAsync(int patientId)
    {
        var loggedInDoctor = await GetLoggedInDoctorAsync();

        var doctorAppointments = await appointmentRepository
            .GetByDoctorIdAsync(loggedInDoctor.DoctorId);

        return doctorAppointments.Any(appointment =>
            appointment.PatientId == patientId &&
            appointment.Status != AppointmentStatus.Cancelled.ToString());
    }

    private async Task<HashSet<int>> GetAccessiblePatientIdsForLoggedInDoctorAsync()
    {
        var loggedInDoctor = await GetLoggedInDoctorAsync();

        var doctorAppointments = await appointmentRepository
            .GetByDoctorIdAsync(loggedInDoctor.DoctorId);

        return doctorAppointments
            .Where(appointment =>
                appointment.Status != AppointmentStatus.Cancelled.ToString())
            .Select(appointment => appointment.PatientId)
            .ToHashSet();
    }

    private static int GetHealthRecordPatientId(HealthRecord healthRecord)
    {
        if (!healthRecord.PatientId.HasValue)
        {
            throw new HealthRecordRuleException(
                "Health record patient details are missing.");
        }

        return healthRecord.PatientId.Value;
    }

    private async Task<Patient> GetLoggedInPatientAsync()
    {
        var userId = CurrentUser?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ForbiddenAccessException("Unable to identify logged-in user.");
        }

        var patient = await patientRepository.GetByUserIdAsync(userId);

        if (patient is null)
        {
            throw new EntityNotFoundException("Patient", userId);
        }

        return patient;
    }

    private async Task<Doctor> GetLoggedInDoctorAsync()
    {
        var userId = CurrentUser?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ForbiddenAccessException("Unable to identify logged-in user.");
        }

        var doctor = await doctorRepository.GetByUserIdAsync(userId);

        if (doctor is null)
        {
            throw new EntityNotFoundException("Doctor", userId);
        }

        return doctor;
    }

    private ClaimsPrincipal? CurrentUser =>
        httpContextAccessor.HttpContext?.User;

    private bool IsPatient()
    {
        return CurrentUser?.IsInRole(Roles.Patient) == true;
    }

    private bool IsDoctor()
    {
        return CurrentUser?.IsInRole(Roles.Doctor) == true;
    }

    private bool IsAdmin()
    {
        return CurrentUser?.IsInRole(Roles.Admin) == true;
    }
}