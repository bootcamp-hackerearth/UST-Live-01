using AutoMapper;
using HealthAxis.Api.Models;
using HealthAxis.Api.Repositories;
using HealthAxis.Shared.Dtos;
using System.Collections.Generic;

namespace HealthAxis.Api.Services
{
    public class DoctorServiceImpl : IDoctorService
    {
        private readonly IDoctorRepository _repo;
        private readonly IMapper _mapper;

        public DoctorServiceImpl(IDoctorRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public DoctorDto AddDoctor(DoctorDto dto)
        {
            var doctor = _mapper.Map<Doctor>(dto);
            doctor.IsActive = true;

            _repo.Add(doctor);
            _repo.Save();

            return _mapper.Map<DoctorDto>(doctor);
        }

        public List<DoctorDto> GetAllDoctors()
        {
            return _mapper.Map<List<DoctorDto>>(_repo.GetAll());
        }

        public DoctorDto GetById(int doctorId)
        {
            var doctor = _repo.GetById(doctorId);
            return doctor == null ? null : _mapper.Map<DoctorDto>(doctor);
        }

        public DoctorDto UpdateDoctor(int id, DoctorDto dto)
        {
            var doctor = _repo.GetById(id);
            if (doctor == null) return null;

            _mapper.Map(dto, doctor);
            _repo.Update(doctor);
            _repo.Save();

            return _mapper.Map<DoctorDto>(doctor);
        }
    }
}