using HealthApp.Shared.Enums;

namespace HealthApp.Shared.Dtos
{
    public class AppointmentFilterDto
    {
        public int? DoctorId { get; set; }

        public int? PatientId { get; set; }

        public AppointmentStatus? Status { get; set; }

        public DateOnly? Date { get; set; }

        public DateOnly? FromDate { get; set; }

        public DateOnly? ToDate { get; set; }

        public bool OnlyUpcoming { get; set; } = false;
    }
}