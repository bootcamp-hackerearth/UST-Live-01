using System.Security.Claims;
using AutoMapper;
using HealthApp.API.Exceptions;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Interface;
using HealthApp.Shared.Constants;
using HealthApp.Shared.DTOs;
using Microsoft.AspNetCore.Http;

namespace HealthApp.API.Service.Impl;

public class PatientService(
    IPatientRepository repository,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper) : IPatientService
{
    public async Task<List<PatientDto>> GetAllPatientsAsync()
        => mapper.Map<List<PatientDto>>(await repository.GetAllAsync());

    public async Task<PatientDto> GetPatientByIdAsync(int patientId)
    {
        ValidatePatientId(patientId);

        await EnsurePatientAccessAsync(patientId);

        var patient = await repository.GetByIdAsync(patientId)
            ?? throw new EntityNotFoundException("Patient", patientId);

        return mapper.Map<PatientDto>(patient);
    }

    public async Task<PatientDto> RegisterPatientAsync(CreatePatientDto dto)
    {
        Validate(
            dto.FullName,
            dto.DateOfBirth,
            dto.Email,
            dto.PhoneNumber);

        var patient = mapper.Map<Patient>(dto);
        patient.DateOfBirth = dto.DateOfBirth.Date;
        patient.CreatedDate = DateTime.Now;

        return mapper.Map<PatientDto>(await repository.AddAsync(patient));
    }

    public async Task<PatientDto> UpdatePatientAsync(
        int patientId,
        UpdatePatientDto dto)
    {
        ValidatePatientId(patientId);

        await EnsurePatientAccessAsync(patientId);

        var existing = await repository.GetByIdAsync(patientId)
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

        var updated = await repository.UpdateAsync(patientId, patient)
            ?? throw new EntityNotFoundException("Patient", patientId);

        return mapper.Map<PatientDto>(updated);
    }

    public async Task EnsurePatientAccessAsync(int patientId)
    {
        ValidatePatientId(patientId);

        var user = httpContextAccessor.HttpContext?.User;

        if (user is null || user.Identity?.IsAuthenticated != true)
        {
            throw new ForbiddenAccessException("Access denied.");
        }

        if (user.IsInRole(Roles.Admin))
        {
            return;
        }

        if (user.IsInRole(Roles.Doctor))
        {
            return;
        }

        if (user.IsInRole(Roles.Patient))
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ForbiddenAccessException("Unable to identify logged-in user.");
            }

            var loggedInPatient = await repository.GetByUserIdAsync(userId);

            if (loggedInPatient is null)
            {
                throw new EntityNotFoundException("Patient", userId);
            }

            if (loggedInPatient.PatientId != patientId)
            {
                throw new ForbiddenAccessException(
                    "You are not allowed to access another patient's information.");
            }

            return;
        }

        throw new ForbiddenAccessException("Access denied.");
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