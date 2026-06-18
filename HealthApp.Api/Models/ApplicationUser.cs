using Microsoft.AspNetCore.Identity;

namespace HealthApp.Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? PatientId { get; set; }

        public int? DoctorId { get; set; }

        public Patient? Patient { get; set; }

        public Doctor? Doctor { get; set; }
    }

}
