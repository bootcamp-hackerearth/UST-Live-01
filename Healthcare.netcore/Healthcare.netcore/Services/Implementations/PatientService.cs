using AutoMapper;
using HealthAxis.API.Dtos.PatientDtos;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;

namespace HealthAxis.API.Services.Impl;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;

    public PatientService(
        IPatientRepository patientRepository,
        IMapper mapper)
    {
        _patientRepository = patientRepository;
        _mapper = mapper;
    }

    public async Task<List<PatientDto>> GetAllPatientsAsync()
    {
        var patients = await _patientRepository.GetAllAsync();

        return _mapper.Map<List<PatientDto>>(patients);
    }

    public async Task<PatientDto?> GetPatientByIdAsync(int id)
    {
        var patient = await _patientRepository.GetByIdAsync(id);

        if (patient == null)
        {
            return null;
        }

        return _mapper.Map<PatientDto>(patient);
    }

    public async Task<PatientDto> CreatePatientAsync(CreatePatientDto dto)
    {
        var patient = _mapper.Map<Patient>(dto);

        await _patientRepository.AddAsync(patient);

        return _mapper.Map<PatientDto>(patient);
    }

    public async Task<PatientDto?> UpdatePatientAsync(int id, UpdatePatientDto dto)
    {
        var patient = await _patientRepository.GetByIdAsync(id);

        if (patient == null)
        {
            return null;
        }

        _mapper.Map(dto, patient);

        await _patientRepository.UpdateAsync(patient);

        return _mapper.Map<PatientDto>(patient);
    }

    public async Task<bool> DeletePatientAsync(int id)
    {
        var patient = await _patientRepository.DeleteAsync(id);

        return patient != null;
    }
}