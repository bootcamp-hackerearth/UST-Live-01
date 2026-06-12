namespace Healthaxis2.Shared.DTOs
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }

        public string DoctorName { get; set; }

        public string Specialisation { get; set; }

        public int Experience { get; set; }

        public int Fees { get; set; }

        public bool IsActive { get; set; }
    }
}