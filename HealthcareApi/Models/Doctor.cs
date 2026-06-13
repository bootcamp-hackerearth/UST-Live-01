using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SharedClasses.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthcareApi.Models
{
    public class Doctor
    {
        public Doctor()
        {
            OffDays = new List<DateTime>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Doctor ID")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Doctor full name is required.")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Specialisation is required.")]
        public Specialisation Specialisation { get; set; }

        [Required(ErrorMessage = "Practice start date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Practice Start Date")]
        public DateTime PracticeStartDate { get; set; }

        [NotMapped]
        [Display(Name = "Years of Experience")]
        public int YearsOfExperience
        {
            get
            {
                DateTime today = DateTime.Today;

                if (PracticeStartDate == DateTime.MinValue)
                {
                    return 0;
                }

                int years = today.Year - PracticeStartDate.Year;

                if (PracticeStartDate.Date > today.AddYears(-years))
                {
                    years--;
                }

                return years < 0 ? 0 : years;
            }
        }

        [Required(ErrorMessage = "Consultation fee is required.")]
        [Range(0, 100000, ErrorMessage = "Consultation fee cannot be negative.")]
        [Display(Name = "Consultation Fee")]
        public decimal ConsultationFee { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }
        [NotMapped]
        public List<DateTime> OffDays { get; set; }

        public bool IsAvailable(DateTime date)
        {
            if (!IsActive)
            {
                return false;
            }

            if (OffDays == null)
            {
                return true;
            }

            return !OffDays.Any(offDay => offDay.Date == date.Date);
        }

        public Doctor GetDoctorSummary()
        {
            return this;
        }
    }
}