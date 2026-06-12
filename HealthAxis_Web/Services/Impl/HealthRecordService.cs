using AutoMapper;
using HealthAxis.Api.Repositories;
using HealthAxis.Shared.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace HealthAxis.Api.Services
{
    public class HealthRecordServiceImpl : IHealthRecordService
    {
        private readonly IHealthRecordRepository _repo;
        private readonly IDoctorRepository _doctorRepo;
        private readonly IMapper _mapper;

        public HealthRecordServiceImpl(
            IHealthRecordRepository repo,
            IDoctorRepository doctorRepo,
            IMapper mapper)
        {
            _repo = repo;
            _doctorRepo = doctorRepo;
            _mapper = mapper;
        }

        public List<HealthRecordDto> GetByPatient(int patientId)
        {
            var records = _repo.GetByPatientId(patientId)
                               .OrderByDescending(r => r.VisitDate)
                               .ToList();

            var result = new List<HealthRecordDto>();

            foreach (var r in records)
            {
                var dto = _mapper.Map<HealthRecordDto>(r);

                var doctor = _doctorRepo.GetById(r.DoctorId);

                if (doctor != null)
                {
                    dto.DoctorName = doctor.FullName;
                    dto.Specialisation = doctor.Specialisation;
                }

                result.Add(dto);
            }

            return result;
        }
    }
}