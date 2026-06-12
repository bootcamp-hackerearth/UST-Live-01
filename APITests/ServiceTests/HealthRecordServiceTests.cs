using System;
using System.Collections.Generic;
using System.Text;

namespace APITests.ServiceTests
{
    using Xunit;
    using Moq;
    using System;
    using System.Collections.Generic;
    using HealthAxisApp.Data;
    using HealthAxisApp.Repositories;
    using HealthAxisApp.Services.Impl;
    using HealthAxisApp.Shared.DTOs;

    public class HealthRecordServiceTests
    {
        private readonly Mock<IHealthRecordRepository> _recordRepo;
        private readonly HealthRecordService _service;

        public HealthRecordServiceTests()
        {
            _recordRepo = new Mock<IHealthRecordRepository>();

            _service = new HealthRecordService(_recordRepo.Object);
        }

        private HealthRecordDto GetValidDto()
        {
            return new HealthRecordDto
            {
                PatientId = 1,
                DoctorId = 1,
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                Notes = "Take rest"
            };
        }

        [Fact]
        public void Create_Valid_ReturnsTrue()
        {
            var dto = GetValidDto();

            var result = _service.Create(dto, out string error);

            Assert.True(result);
            Assert.Equal(string.Empty, error);

            _recordRepo.Verify(x =>
                x.Add(It.IsAny<HealthRecord>()), Times.Once);
        }

        [Fact]
        public void Create_Valid_SavesCorrectData()
        {
            var dto = GetValidDto();

            HealthRecord savedRecord = null;

            _recordRepo.Setup(x => x.Add(It.IsAny<HealthRecord>()))
                       .Callback<HealthRecord>(r => savedRecord = r);

            _service.Create(dto, out _);

            Assert.NotNull(savedRecord);

            Assert.Equal(dto.PatientId, savedRecord.PatientId);
            Assert.Equal(dto.DoctorId, savedRecord.DoctorId);
            Assert.Equal(dto.Diagnosis, savedRecord.Diagnosis);
            Assert.Equal(dto.Prescription, savedRecord.Prescription);
            Assert.Equal(dto.Notes, savedRecord.Notes);

            Assert.True(savedRecord.VisitDate <= DateTime.Now);
        }

        [Fact]
        public void GetByPatient_ReturnsMappedDtos()
        {
            _recordRepo.Setup(x => x.GetByPatient(1))
                .Returns(new List<HealthRecord>
                {
                new HealthRecord
                {
                    RecordId = 1,
                    PatientId = 1,
                    DoctorId = 1,
                    Diagnosis = "Fever",
                    Prescription = "Medicine",
                    Notes = "Rest",
                    VisitDate = DateTime.Today,
                    Patient = new Patient { FullName = "John" },
                    Doctor = new Doctor
                    {
                        FullName = "Dr Smith",
                        Specialisation = "Cardiology"
                    }
                }
                });

            var result = _service.GetByPatient(1);

            Assert.NotNull(result);
            Assert.Single(result);

            var record = Assert.Single(result);

            Assert.Equal(1, record.PatientId);
            Assert.Equal("John", record.PatientName);
            Assert.Equal("Dr Smith", record.DoctorName);
            Assert.Equal("Fever", record.Diagnosis);
        }

        [Fact]
        public void GetByPatient_NoRecords_ReturnsEmptyList()
        {
            _recordRepo.Setup(x => x.GetByPatient(1))
                       .Returns(new List<HealthRecord>());

            var result = _service.GetByPatient(1);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
