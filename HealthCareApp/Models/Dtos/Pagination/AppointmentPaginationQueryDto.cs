using HealthCareApp.Enums;

namespace HealthCareApp.Dtos
{
    public class AppointmentPaginationQueryDto : PaginationQueryDto
    {
        public string? SearchTerm { get; set; }

        public int? PatientId { get; set; }

        public int? DoctorId { get; set; }

        public AppointmentStatus? Status { get; set; }

        public DateTime? ScheduledDate { get; set; }

        public bool? UpcomingOnly { get; set; }


    }
}