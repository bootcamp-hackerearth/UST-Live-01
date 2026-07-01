using System.Security.Claims;
using AutoMapper;
using HealthApp.API.Exceptions;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Interface;
using HealthApp.Shared.Constants;
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;
using Microsoft.AspNetCore.Http;

namespace HealthApp.API.Service.Impl;

public class PatientService(
    IPatientRepository patientRepository,
    IDoctorRepository doctorRepository,
    IAppointmentRepository appointmentRepository,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper) : IPatientService
{
    public async Task<List<PatientDto>> GetAllPatientsAsync()
    {
        return mapper.Map<List<PatientDto>>(
            await patientRepository.GetAllAsync());
    }

    public async Task<PatientDto> GetPatientByIdAsync(int patientId)
    {
        ValidatePatientId(patientId);

        await EnsurePatientAccessAsync(patientId);

        var patient = await patientRepository.GetByIdAsync(patientId)
            ?? throw new EntityNotFoundException("Patient", patientId);

        return mapper.Map<PatientDto>(patient);
    }

    public async Task<PatientDto> RegisterPatientAsync(CreatePatientDto dto)
    {
        if (dto is null)
        {
            throw new BusinessRuleException("Patient details are required.");
        }

        Validate(
            dto.FullName,
            dto.DateOfBirth,
            dto.Email,
            dto.PhoneNumber);

        var patient = mapper.Map<Patient>(dto);

        patient.DateOfBirth = dto.DateOfBirth.Date;
        patient.CreatedDate = DateTime.Now;

        return mapper.Map<PatientDto>(
            await patientRepository.AddAsync(patient));
    }

    public async Task<PatientDto> UpdatePatientAsync(
        int patientId,
        UpdatePatientDto dto)
    {
        ValidatePatientId(patientId);

        if (dto is null)
        {
            throw new BusinessRuleException("Patient details are required.");
        }

        await EnsurePatientAccessAsync(patientId);

        var existing = await patientRepository.GetByIdAsync(patientId)
            ?? throw new EntityNotFoundException("Patient", patientId);

        Validate(
            dto.FullName,
            dto.DateOfBirth,
            dto.Email,
            dto.PhoneNumber);

        var patient = mapper.Map<Patient>(dto);

        patient.PatientId = patientId;
        patient.UserId = existing.UserId;
        patient.CreatedDate = existing.CreatedDate;

        var updated = await patientRepository.UpdateAsync(patientId, patient)
            ?? throw new EntityNotFoundException("Patient", patientId);

        return mapper.Map<PatientDto>(updated);
    }

    public async Task EnsurePatientAccessAsync(int patientId)
    {
        ValidatePatientId(patientId);

        if (await patientRepository.GetByIdAsync(patientId) is null)
        {
            throw new EntityNotFoundException("Patient", patientId);
        }

        if (IsAdmin())
        {
            return;
        }

        if (IsPatient())
        {
            var loggedInPatient = await GetLoggedInPatientAsync();

            if (loggedInPatient.PatientId != patientId)
            {
                throw new ForbiddenAccessException(
                    "You are not allowed to access another patient's information.");
            }

            return;
        }

        if (IsDoctor())
        {
            var loggedInDoctor = await GetLoggedInDoctorAsync();

            var doctorAppointments = await appointmentRepository
                .GetByDoctorIdAsync(loggedInDoctor.DoctorId);

            var hasAppointmentRelation = doctorAppointments.Any(appointment =>
                appointment.PatientId == patientId &&
                appointment.Status != AppointmentStatus.Cancelled.ToString());

            if (!hasAppointmentRelation)
            {
                throw new ForbiddenAccessException(
                    "You are not allowed to access this patient's information.");
            }

            return;
        }

        throw new ForbiddenAccessException("Access denied.");
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

    private static void ValidatePatientId(int id)
    {
        if (id <= 0)
        {
            throw new BusinessRuleException(
                "Please provide a valid patient reference.");
        }
    }

    private static void Validate(
        string name,
        DateTime dob,
        string email,
        string phone)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessRuleException(
                "Patient name is required.");
        }

        if (dob.Date < new DateTime(1900, 1, 1) ||
            dob.Date > DateTime.Today)
        {
            throw new BusinessRuleException(
                "Invalid date of birth.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new BusinessRuleException(
                "Email is required.");
        }

        if (string.IsNullOrWhiteSpace(phone))
        {
            throw new BusinessRuleException(
                "Phone number is required.");
        }
    }
}