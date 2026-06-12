using AutoMapper;
using HealthAxis.Api.Repositories;
using HealthAxis.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthAxis.Api.Services
{
    public class PatientServiceImpl : IPatientService
    {
        private readonly IPatientRepository _repo;
        private readonly IMapper _mapper;

        public PatientServiceImpl(IPatientRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public PatientDto Register(PatientDto dto)
        {
            if (_repo.ExistsByEmail(dto.Email))
                return null;

            var patient = _mapper.Map<Patient>(dto);
            patient.CreatedDate = DateTime.Now;
            patient.IsActive = true;

            _repo.Add(patient);
            _repo.Save();

            return _mapper.Map<PatientDto>(patient);
        }

        public List<PatientDto> GetAllPatients()
        {
            return _mapper.Map<List<PatientDto>>(_repo.GetAll());
        }

        public PatientDto GetById(int id)
        {
            var patient = _repo.GetById(id);

            if (patient == null)
                return null;

            var patientDto = _mapper.Map<PatientDto>(patient);

            var count = _repo.GetAppointmentsByPatientId(id)
                .Count(a => a.ScheduledDate >= DateTime.Today
                            && a.Status != "Cancelled");

            patientDto.UpcomingAppointments = count;

            return patientDto;
        }

        public PatientDto Update(int id, PatientDto dto)
        {
            var patient = _repo.GetById(id);
            if (patient == null) return null;

            _mapper.Map(dto, patient);

            _repo.Update(patient);
            _repo.Save();

            return _mapper.Map<PatientDto>(patient);
        }

        public bool Deactivate(int id)
        {
            var patient = _repo.GetById(id);
            if (patient == null)
                return false;

            _repo.Deactivate(id);
            _repo.Save();

            return true;
        }
    }
}