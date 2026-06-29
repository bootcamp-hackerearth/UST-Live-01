using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Api.Models
{

    public class User : IdentityUser
    {
        public Patient? Patient { get; set; }

    }
}
