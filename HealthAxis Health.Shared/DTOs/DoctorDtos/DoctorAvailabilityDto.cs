using System.Diagnostics.CodeAnalysis;

namespace HealthAxisHealth.Shared.DTOs.DoctorDtos
{
    [ExcludeFromCodeCoverage]
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
