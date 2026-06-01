using HAP_Pod4_ConsoleApp_au.Data;
using HAP_Pod4_ConsoleApp_au.Models;
using HAP_Pod4_ConsoleApp_au.Repositories;

namespace HealthAxisTests.RepositoryTests
{
    public class HealthRepositoryTests
    {
        private readonly AppDbContext _dbContext;
        private readonly HealthRepository _repo;

        public HealthRepositoryTests()
        {
            _dbContext = new AppDbContext();
            _repo = new HealthRepository();
        }

        [Fact]
        public void AddRecord_WhenValidRecord_ShouldAddSuccessfully()
        {
            HealthRecord record = new HealthRecord
            {
                RecordId = _dbContext.GetNextHealthRecordId(),
                Patient = _dbContext.Patients[0],
                Doctor = _dbContext.Doctors[0],
                VisitDate = DateTime.Now,
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                Notes = "Take rest"
            };

            var result = _repo.AddRecord(record);

            Assert.NotNull(result);
            Assert.Equal("Fever", result.Diagnosis);
        }

        [Fact]
        public void GetRecordsByPatient_WhenRecordsExist_ShouldReturnPatientRecords()
        {
            HealthRecord record1 = new HealthRecord
            {
                RecordId = _dbContext.GetNextHealthRecordId(),
                Patient = _dbContext.Patients[0],
                Doctor = _dbContext.Doctors[0],
                VisitDate = DateTime.Now,
                Diagnosis = "Cold"
            };

            HealthRecord record2 = new HealthRecord
            {
                RecordId = _dbContext.GetNextHealthRecordId(),
                Patient = _dbContext.Patients[0],
                Doctor = _dbContext.Doctors[1],
                VisitDate = DateTime.Now,
                Diagnosis = "Fever"
            };

            _repo.AddRecord(record1);
            _repo.AddRecord(record2);

            var result =
                _repo.GetRecordsByPatient(
                    _dbContext.Patients[0].PatientId);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetRecordsByDoctor_WhenRecordsExist_ShouldReturnDoctorRecords()
        {
            HealthRecord record1 = new HealthRecord
            {
                RecordId = _dbContext.GetNextHealthRecordId(),
                Patient = _dbContext.Patients[0],
                Doctor = _dbContext.Doctors[0],
                VisitDate = DateTime.Now,
                Diagnosis = "Migraine"
            };

            HealthRecord record2 = new HealthRecord
            {
                RecordId = _dbContext.GetNextHealthRecordId(),
                Patient = _dbContext.Patients[1],
                Doctor = _dbContext.Doctors[0],
                VisitDate = DateTime.Now,
                Diagnosis = "Back Pain"
            };

            _repo.AddRecord(record1);
            _repo.AddRecord(record2);

            var result =
                _repo.GetRecordsByDoctor(
                    _dbContext.Doctors[0].DoctorId);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetRecordsByPatient_WhenNoRecords_ShouldReturnEmptyList()
        {
            var result = _repo.GetRecordsByPatient(999);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetRecordsByDoctor_WhenNoRecords_ShouldReturnEmptyList()
        {
            var result = _repo.GetRecordsByDoctor(999);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}