namespace HealthApp.Api.Dtos
{
    public class HealthRecordCreateDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int? AppointmentId { get; set; }
        public DateTime VisitDate { get; set; }
        public string Diagnosis { get; set; }
        public string Prescription { get; set; }
        public string? Notes { get; set; }
    }
}
