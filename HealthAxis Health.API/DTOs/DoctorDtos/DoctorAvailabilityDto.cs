namespace HealthAxisHealth.API.DTOs.DoctorDtos
{
    public class DoctorAvailabilityDto
    {
        #region Properties

        public DateTime Date { get; set; }

        public string TimeSlot { get; set; }
            = string.Empty;

        public bool IsAvailable { get; set; }

        #endregion
    }
}
