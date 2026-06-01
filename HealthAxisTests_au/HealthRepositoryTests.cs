using System;
using System.Collections.Generic;
using HAP_Pod4_ConsoleApp_au.Models;
using HAP_Pod4_ConsoleApp_au.Repositories;
using Xunit;

namespace HealthAxisTests.RepositoryTests
{
    public class HealthRepositoryTests
    {
        private readonly HealthRepository _repository;

        public HealthRepositoryTests()
        {
            _repository = new HealthRepository();
        }

        [Fact]
        public void AddRecord_WhenValid_ShouldAddAndReturnRecord()
        {
            HealthRecord newRecord = new HealthRecord
            {
                RecordId = 1,
                Patient = new Patient { PatientId = 101 },
                Doctor = new Doctor { DoctorId = 201 },
                Diagnosis = "Common Cold"
            };

            HealthRecord result = _repository.AddRecord(newRecord);

            Assert.NotNull(result);
            Assert.Equal(1, result.RecordId);
            Assert.Equal("Common Cold", result.Diagnosis);
            Assert.Single(_repository.GetRecordsByPatient(101));
        }

        [Fact]
        public void AddRecord_WhenMultipleRecordsAdded_ShouldKeepAllRecords()
        {
            HealthRecord record1 = new HealthRecord { RecordId = 1, Patient = new Patient { PatientId = 101 }, Doctor = new Doctor { DoctorId = 201 } };
            HealthRecord record2 = new HealthRecord { RecordId = 2, Patient = new Patient { PatientId = 101 }, Doctor = new Doctor { DoctorId = 201 } };

            _repository.AddRecord(record1);
            _repository.AddRecord(record2);

            List<HealthRecord> result = _repository.GetRecordsByPatient(101);

            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].RecordId);
            Assert.Equal(2, result[1].RecordId);
        }

        [Fact]
        public void GetRecordsByPatient_WhenRecordsExist_ShouldReturnMatchingRecords()
        {
            _repository.AddRecord(new HealthRecord { RecordId = 1, Patient = new Patient { PatientId = 101 }, Doctor = new Doctor { DoctorId = 201 } });
            _repository.AddRecord(new HealthRecord { RecordId = 2, Patient = new Patient { PatientId = 102 }, Doctor = new Doctor { DoctorId = 201 } });
            _repository.AddRecord(new HealthRecord { RecordId = 3, Patient = new Patient { PatientId = 101 }, Doctor = new Doctor { DoctorId = 202 } });

            List<HealthRecord> result = _repository.GetRecordsByPatient(101);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, r => Assert.Equal(101, r.Patient!.PatientId));
        }

        [Fact]
        public void GetRecordsByPatient_WhenNoRecordsExist_ShouldReturnEmptyList()
        {
            _repository.AddRecord(new HealthRecord { RecordId = 1, Patient = new Patient { PatientId = 101 }, Doctor = new Doctor { DoctorId = 201 } });

            List<HealthRecord> result = _repository.GetRecordsByPatient(999);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetRecordsByPatient_WhenPatientHasMultipleDoctors_ShouldReturnAllForPatient()
        {
            _repository.AddRecord(new HealthRecord { RecordId = 1, Patient = new Patient { PatientId = 105 }, Doctor = new Doctor { DoctorId = 301 } });
            _repository.AddRecord(new HealthRecord { RecordId = 2, Patient = new Patient { PatientId = 105 }, Doctor = new Doctor { DoctorId = 302 } });

            List<HealthRecord> result = _repository.GetRecordsByPatient(105);

            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.Doctor!.DoctorId == 301);
            Assert.Contains(result, r => r.Doctor!.DoctorId == 302);
        }

        [Fact]
        public void GetRecordsByDoctor_WhenRecordsExist_ShouldReturnMatchingRecords()
        {
            _repository.AddRecord(new HealthRecord { RecordId = 1, Patient = new Patient { PatientId = 101 }, Doctor = new Doctor { DoctorId = 201 } });
            _repository.AddRecord(new HealthRecord { RecordId = 2, Patient = new Patient { PatientId = 102 }, Doctor = new Doctor { DoctorId = 202 } });
            _repository.AddRecord(new HealthRecord { RecordId = 3, Patient = new Patient { PatientId = 103 }, Doctor = new Doctor { DoctorId = 201 } });

            List<HealthRecord> result = _repository.GetRecordsByDoctor(201);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, r => Assert.Equal(201, r.Doctor!.DoctorId));
        }

        [Fact]
        public void GetRecordsByDoctor_WhenNoRecordsExist_ShouldReturnEmptyList()
        {
            _repository.AddRecord(new HealthRecord { RecordId = 1, Patient = new Patient { PatientId = 101 }, Doctor = new Doctor { DoctorId = 201 } });

            List<HealthRecord> result = _repository.GetRecordsByDoctor(999);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetRecordsByDoctor_WhenDoctorSeesSamePatientMultipleTimes_ShouldReturnAllRecords()
        {
            _repository.AddRecord(new HealthRecord { RecordId = 1, Patient = new Patient { PatientId = 110 }, Doctor = new Doctor { DoctorId = 400 } });
            _repository.AddRecord(new HealthRecord { RecordId = 2, Patient = new Patient { PatientId = 110 }, Doctor = new Doctor { DoctorId = 400 } });

            List<HealthRecord> result = _repository.GetRecordsByDoctor(400);

            Assert.Equal(2, result.Count);
            Assert.All(result, r => Assert.Equal(110, r.Patient!.PatientId));
        }
    }
}