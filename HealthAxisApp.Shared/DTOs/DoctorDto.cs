using HealthAxisApp.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthAxisApp.Shared.DTOs
{
    public class DoctorDto
    {
        public int? DoctorId { get; set; }
        [Required, StringLength(50)] public string FullName { get; set; }
        [Required] public SpecialisationEnum Specialisation { get; set; }
        [Range(1, 64)] public int YearsOfExperience { get; set; }
        [Range(typeof(decimal), "0", "999999", ErrorMessage = "Consultation fee cannot be negative.")]
        public decimal ConsultationFee { get; set; }
        public bool IsActive { get; set; }
        public int UpcomingAppointmentCount { get; set; }
    }
}
