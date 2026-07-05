namespace Healthcare.Shared.DTOs.Patient
{
    public class PatientListDto
    {
        public int PatientId { get; set; }
        public string FullName { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; } 
        public string? Gender { get; set; } 
        public bool HasInsurance { get; set; }
        public bool IsActive { get; set; }  

    }
}
