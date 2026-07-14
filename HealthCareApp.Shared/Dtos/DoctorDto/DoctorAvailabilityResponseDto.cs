using HealthCareApp.Shared.Dtos.Doctors;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareApp.Shared.Dtos.DoctorDto
{
    public class DoctorAvailabilityResponseDto

{

public int DoctorId { get; set; }

 

public string Date { get; set; } = string.Empty;

 

public bool IsDoctorOnLeave { get; set; }

 

public string Message { get; set; } = string.Empty;

 

public List<SlotAvailabilityDto> Slots { get; set; } = new();

}

}
