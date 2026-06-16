using AutoMapper;
using HealthAxis.API.DTOs.Patients;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;

namespace HealthAxis.API.Services
{
    public class PatientService
        : Service<Patient, PatientReadDto, PatientCreateDto, PatientUpdateDto>, IPatientService
    {
        public PatientService(
            IPatientRepository patientRepository,
            IMapper mapper)
            : base(patientRepository, mapper)
        {
        }
    }
}
