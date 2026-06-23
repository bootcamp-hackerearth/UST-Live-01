namespace Healthcare.Shared.DTOs.Patient
{
    public class PatientListDto
    {
        public int PatientId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public bool HasInsurance { get; set; }
    }
}
