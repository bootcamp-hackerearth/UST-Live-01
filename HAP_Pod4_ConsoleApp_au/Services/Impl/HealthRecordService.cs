using System;
using System.Collections.Generic;
using System.Linq;
using HAP_Pod4_ConsoleApp_au.Models;
using HAP_Pod4_ConsoleApp_au.Repositories;

namespace HAP_Pod4_ConsoleApp_au.Services.Impl
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IHealthRepository _repository;

        public HealthRecordService(IHealthRepository repository)
        {
            _repository = repository;
        }

        public HealthRecord AddRecord(HealthRecord record)
        {
            // Replaced the manual null check with ThrowIfNull
            ArgumentNullException.ThrowIfNull(record);

            if (string.IsNullOrWhiteSpace(record.Diagnosis))
            {
                throw new ArgumentException("Diagnosis cannot be empty.", nameof(record));
            }

            _repository.AddRecord(record);

            return record;
        }

        public List<HealthRecord> GetRecordsByPatient(int patientId)
        {
            return _repository
                .GetRecordsByPatient(patientId)
                .OrderByDescending(record => record.VisitDate)
                .ToList();
        }

        public List<HealthRecord> GetRecordsByDoctor(int doctorId)
        {
            return _repository
                .GetRecordsByDoctor(doctorId)
                .OrderByDescending(record => record.VisitDate)
                .ToList();
        }
    }
}