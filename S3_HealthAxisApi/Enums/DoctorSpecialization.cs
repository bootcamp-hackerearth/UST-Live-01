using System.ComponentModel.DataAnnotations;
namespace S3_HealthAxisApi.Enums
{
    public enum DoctorSpecialisation
    {
        [Display(Name = "General Practitioner")]
        GeneralPractitioner = 1,

        [Display(Name = "Cardiologist")]
        Cardiologist = 2,

        [Display(Name = "Dermatologist")]
        Dermatologist = 3,

        [Display(Name = "Neurologist")]
        Neurologist = 4,

        [Display(Name = "Pediatrician")]
        Pediatrician = 5,

        [Display(Name = "Psychiatrist")]
        Psychiatrist = 6,

        [Display(Name = "Orthopedic Surgeon")]
        OrthopedicSurgeon = 7,

        [Display(Name = "Gynecologist")]
        Gynecologist = 8,

        [Display(Name = "Oncologist")]
        Oncologist = 9,

        [Display(Name = "Endocrinologist")]
        Endocrinologist = 10
    }
}