namespace HealthApp.ConsoleApp.Models
{
    // Represents a health record for a patient's visit to a doctor
    public class HealthRecord
    {
        public int RecordId { get; set; }
        public required Patient Patient { get; set; }
        public required Doctor Doctor { get; set; }
        public DateTime VisitDate { get; set; }
        public required string Diagnosis { get; set; }
        public required string Prescription { get; set; }
        public required string DoctorNotes { get; set; }
        public int AppointmentId { get; set; }

        // Method to get a summary of the health record
        public string GetSummary()
        {
            return $"Record Id: {RecordId} | Patient: {Patient.Name} | Doctor: {Doctor.Name} | Date: {VisitDate.ToShortDateString()} | Diagnosis: {Diagnosis} | Prescription: {Prescription} | Notes: {DoctorNotes}";
        }
    }
}