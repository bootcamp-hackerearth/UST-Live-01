using HealthApp.Blazor.Components.Models;

namespace HealthApp.Blazor.Components.Services
{
    public class HealthRecordService
    {
        private readonly List<HealthRecord> _healthRecords = new()
        {
            new HealthRecord
            {
                RecordId = 1,
                PatientId = 1,
                DoctorId = 1,
                VisitDate = DateTime.Today.AddDays(-15),
                Diagnosis = "Mild Hypertension",
                Prescription = "Tablet A once daily",
                Notes = "Reduce salt intake and monitor BP",
                Patient = new Patient
                {
                    PatientId = 1,
                    FullName = "Arun S"
                },
                Doctor = new Doctor
                {
                    DoctorId = 1,
                    FullName = "Dr. Anil Kumar"
                }
            },
            new HealthRecord
            {
                RecordId = 2,
                PatientId = 2,
                DoctorId = 2,
                VisitDate = DateTime.Today.AddDays(-10),
                Diagnosis = "Skin Allergy",
                Prescription = "Antihistamine for 5 days",
                Notes = "Avoid dust exposure",
                Patient = new Patient
                {
                    PatientId = 2,
                    FullName = "Divya R"
                },
                Doctor = new Doctor
                {
                    DoctorId = 2,
                    FullName = "Dr. Meera Nair"
                }
            },
            new HealthRecord
            {
                RecordId = 3,
                PatientId = 3,
                DoctorId = 1,
                VisitDate = DateTime.Today.AddDays(-5),
                Diagnosis = "Routine Follow-up",
                Prescription = "Continue current medication",
                Notes = "Next review after 30 days",
                Patient = new Patient
                {
                    PatientId = 3,
                    FullName = "Kiran P"
                },
                Doctor = new Doctor
                {
                    DoctorId = 1,
                    FullName = "Dr. Anil Kumar"
                }
            }
        };

        public Task<List<HealthRecord>> GetAllHealthRecordsAsync()
        {
            return Task.FromResult(_healthRecords.ToList());
        }

        public Task<int> GetHealthRecordCountAsync()
        {
            return Task.FromResult(_healthRecords.Count);
        }

        public Task<HealthRecord?> GetHealthRecordByIdAsync(int id)
        {
            var record = _healthRecords.FirstOrDefault(h => h.RecordId == id);
            return Task.FromResult(record);
        }

        public Task<List<HealthRecord>> GetHealthRecordsByPatientIdAsync(int patientId)
        {
            var result = _healthRecords.Where(h => h.PatientId == patientId).ToList();
            return Task.FromResult(result);
        }

        public Task<List<HealthRecord>> GetHealthRecordsByDoctorIdAsync(int doctorId)
        {
            var result = _healthRecords.Where(h => h.DoctorId == doctorId).ToList();
            return Task.FromResult(result);
        }
    }
}
