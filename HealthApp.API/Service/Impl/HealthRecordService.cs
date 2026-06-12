using AutoMapper;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Interface;
using HealthApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using HealthApp.API.Data;

namespace HealthApp.API.Service.Impl
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IHealthRecordRepository _repo;
        private readonly IMapper _mapper;

        public HealthRecordService(IHealthRecordRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // ✅ CREATE
        public async Task AddRecord(HealthRecordDto dto)
        {
            if (dto == null)
                throw new Exception("Invalid record");

            var record = _mapper.Map<HealthRecord>(dto);

            await _repo.AddAsync(record);
        }

        // ✅ GET ALL
        public async Task<List<HealthRecordDto>> GetAllRecords()
        {
            var list = await _repo.GetAllAsync();
            return _mapper.Map<List<HealthRecordDto>>(list);
        }

        // ✅ GET BY PATIENT
        public async Task<List<HealthRecordDto>> GetPatientRecords(int patientId)
        {
            var list = await _repo.GetAllAsync();

            var filtered = list
                .Where(r => r.PatientId == patientId)
                .ToList();

            return _mapper.Map<List<HealthRecordDto>>(filtered);
        }

        // ✅ FILTER BY DOCTOR + PATIENT
        public async Task<List<HealthRecordDto>> GetHealthRecordsByDoctor(int doctorId, int patientId)
        {
            var list = await _repo.GetAllAsync();

            var filtered = list
                .Where(r => r.DoctorId == doctorId &&
                            r.PatientId == patientId)
                .OrderByDescending(r => r.VisitDate)
                .ToList();

            return _mapper.Map<List<HealthRecordDto>>(filtered);
        }
    }
}