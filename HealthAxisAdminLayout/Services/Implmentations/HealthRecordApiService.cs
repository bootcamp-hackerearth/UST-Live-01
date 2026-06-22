using HealthAxisAdminLayout.DTOs.HealthRecord;
using HealthAxisAdminLayout.Services.Interfaces;

namespace HealthAxisAdminLayout.Services.Implementations
{
    public class HealthRecordApiService : IHealthRecordApiService
    {
        private readonly List<HealthRecordResponseDTO> _records = new()
        {
            new HealthRecordResponseDTO
            {
                HealthRecordId = 1,
                PatientId = 1,
                DoctorId = 1,
                AppointmentId = 1,
                VisitDate = DateTime.Today,
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                Notes = "Drink water and rest"
            }
        };

        public Task<List<HealthRecordResponseDTO>> GetHealthRecordsAsync()
        {
            return Task.FromResult(_records.ToList());
        }

        public Task<bool> DeleteHealthRecordAsync(int id)
        {
            var record = _records.FirstOrDefault(r => r.HealthRecordId == id);

            if (record == null)
                return Task.FromResult(false);

            _records.Remove(record);
            return Task.FromResult(true);
        }
    }
}