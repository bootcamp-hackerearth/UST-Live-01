using AutoMapper;
using HealthApp.API.Exceptions;
using HealthApp.API.Models;
using HealthApp.API.Models.DTOs;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Interface;

namespace HealthApp.API.Service.Impl;

public class PatientService(
    IPatientRepository repository,
    IMapper mapper) : IPatientService
{
    public async Task<List<PatientDto>> GetAllPatientsAsync()
        => mapper.Map<List<PatientDto>>(await repository.GetAllAsync());

    public async Task<PatientDto> GetPatientByIdAsync(int patientId)
    {
        ValidatePatientId(patientId);

        var p = await repository.GetByIdAsync(patientId)
            ?? throw new EntityNotFoundException("Patient", patientId);

        return mapper.Map<PatientDto>(p);
    }

    public async Task<PatientDto> RegisterPatientAsync(CreatePatientDto dto)
    {
        Validate(
            dto.FullName,
            dto.DateOfBirth,
            dto.Email,
            dto.PhoneNumber);

        var p = mapper.Map<Patient>(dto);
        p.DateOfBirth = dto.DateOfBirth.Date;
        p.CreatedDate = DateTime.Now;

        return mapper.Map<PatientDto>(await repository.AddAsync(p));
    }

    public async Task<PatientDto> UpdatePatientAsync(
        int patientId,
        UpdatePatientDto dto)
    {
        ValidatePatientId(patientId);

        var existing = await repository.GetByIdAsync(patientId)
            ?? throw new EntityNotFoundException("Patient", patientId);

        Validate(
            dto.FullName,
            dto.DateOfBirth,
            dto.Email,
            dto.PhoneNumber);

        var p = mapper.Map<Patient>(dto);
        p.PatientId = patientId;
        p.CreatedDate = existing.CreatedDate;

        var updated = await repository.UpdateAsync(patientId, p)
            ?? throw new EntityNotFoundException("Patient", patientId);

        return mapper.Map<PatientDto>(updated);
    }

    private static void ValidatePatientId(int id)
    {
        if (id <= 0)
            throw new BusinessRuleException(
                "Please provide a valid patient reference.");
    }

    private static void Validate(
        string name,
        DateTime dob,
        string email,
        string phone)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessRuleException(
                "Patient name is required.");

        if (dob.Date < new DateTime(1900, 1, 1) ||
            dob.Date > DateTime.Today)
        {
            throw new BusinessRuleException(
                "Invalid date of birth.");
        }

        if (string.IsNullOrWhiteSpace(email))
            throw new BusinessRuleException(
                "Email is required.");

        if (string.IsNullOrWhiteSpace(phone))
            throw new BusinessRuleException(
                "Phone number is required.");
    }
}