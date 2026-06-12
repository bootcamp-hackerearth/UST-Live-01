using HealthCareApi.Models;
using System.Collections.Generic;

namespace HealthCareApi.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }          // "Patient", "Doctor", "Admin"

        public virtual Doctor Doctor { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }

    }
}