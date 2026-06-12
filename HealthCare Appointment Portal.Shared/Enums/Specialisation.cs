using System.ComponentModel.DataAnnotations;

namespace HealthCare_Appointment_Portal.Enums
{

    public enum Specialisation
    {

        Cardiology = 1,
        Neurology,
        Dermatology,
        Orthopedics,
        Pediatrics,
        Gynecology,
        Oncology,
        Psychiatry,
        Ophthalmology,
        ENT,
        Pulmonology,
        Gastroenterology,
        Nephrology,
        Urology,
        Endocrinology,
        Radiology,
        Anesthesiology,
        [Display(Name = "General Surgery")]
        GeneralSurgery,
        [Display(Name = "Emergency Surgery")]
        EmergencySurgery,
        Cardiologist
    }
}
