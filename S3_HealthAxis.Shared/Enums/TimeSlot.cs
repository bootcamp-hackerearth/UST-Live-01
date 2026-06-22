using System.ComponentModel.DataAnnotations;
namespace S3_HealthAxis.Shared.Enums
{
    public enum AppointmentTimeSlot
    {
        [Display(Name = "10:00 AM - 10:30 AM")]
        TenAM = 1,

        [Display(Name = "10:30 AM - 11:00 AM")]
        TenThirtyAM = 2,

        [Display(Name = "11:00 AM - 11:30 AM")]
        ElevenAM = 3,

        [Display(Name = "11:30 AM - 12:00 PM")]
        ElevenThirtyAM = 4,

        [Display(Name = "12:00 PM - 12:30 PM")]
        TwelvePM = 5,

        [Display(Name = "12:30 PM - 01:00 PM")]
        TwelveThirtyPM = 6,

        [Display(Name = "01:00 PM - 01:30 PM")]
        OnePM = 7,

        [Display(Name = "01:30 PM - 02:00 PM")]
        OneThirtyPM = 8,

        [Display(Name = "02:00 PM - 02:30 PM")]
        TwoPM = 9,

        [Display(Name = "02:30 PM - 03:00 PM")]
        TwoThirtyPM = 10,

        [Display(Name = "03:00 PM - 03:30 PM")]
        ThreePM = 11,

        [Display(Name = "03:30 PM - 04:00 PM")]
        ThreeThirtyPM = 12
    }
}