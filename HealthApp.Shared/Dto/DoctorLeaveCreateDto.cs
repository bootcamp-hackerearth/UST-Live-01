using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HealthApp.Shared.Dto
{
    public class DoctorLeaveCreateDto
    {
        [Required]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Reason is required")]
        [StringLength(500, MinimumLength = 3,
            ErrorMessage = "Reason must be between 3 and 500 characters")]
        public string Reason { get; set; } = string.Empty;
    }

}
