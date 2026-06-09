using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HealthAxis.Shared.Dtos;
using HealthAxis_Web.Models;

public class PatientServiceImpl : IPatientService
{
    private readonly IPatientRepository _repository;
    private readonly IMapper _mapper;

    public PatientServiceImpl(IPatientRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public List<PatientDto> GetAllPatients()
    {
        var patients = _repository.GetAllPatients();

        return _mapper.Map<List<PatientDto>>(patients);
    }

    public PatientDto GetById(int id)
    {
        var patient = _repository.GetById(id);

        if (patient == null)
            return null;

        return _mapper.Map<PatientDto>(patient);
    }

    public PatientDto AddPatient(PatientDto patientDto)
    {
        var patient = _mapper.Map<Patient>(patientDto);

        patient.CreatedDate = System.DateTime.Now;

        var savedPatient = _repository.AddPatient(patient);

        return _mapper.Map<PatientDto>(savedPatient);
    }

    public PatientDto UpdatePatient(int id, PatientDto patientDto)
    {
        var existing = _repository.GetById(id);

        if (existing == null)
            return null;

        var updated = _mapper.Map(patientDto, existing);

        var saved = _repository.UpdatePatient(id, updated);

        return _mapper.Map<PatientDto>(saved);
    }
}
