using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace HealthCareApp.Shared.Dtos.DoctorLeaves
{
    public sealed class CreateDoctorLeaveDto
{

[Required]
public DateTime StartDate { get; set; }

 

[Required]
public DateTime EndDate { get; set; }

 
[Required]
public string Reason { get; set; } = string.Empty;
}
}
