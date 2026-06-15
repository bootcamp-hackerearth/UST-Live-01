namespace HealthAxis.API.Dtos.DoctorDtos
{
    public class DoctorDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Specialisation { get; set; } = string.Empty;

        public decimal ConsultationFee { get; set; }

        public bool IsAvailable { get; set; }
    }
}