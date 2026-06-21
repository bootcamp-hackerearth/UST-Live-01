using HealthApp.API.Enums;
using HealthApp.API.Models.DTOs;
using HealthApp.API.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.API.Controllers;

[ApiController]
[Route("api/doctors")]
[Authorize]
public class DoctorsController(IDoctorService doctorService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<DoctorDto>>> Get([FromQuery] SpecialisationType? specialisation)
        => Ok(
            specialisation.HasValue
                ? await doctorService.GetDoctorsBySpecialisationAsync(specialisation.Value)
                : await doctorService.GetAllDoctorsAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DoctorDto>> GetById(int id)
        => Ok(await doctorService.GetDoctorByIdAsync(id));

    [HttpGet("{id:int}/availability")]
    public async Task<ActionResult<DoctorAvailabilityDto>> Availability(int id, [FromQuery] DateTime date)
        => Ok(await doctorService.GetDoctorAvailabilityAsync(
            id,
            date == default ? DateTime.Today : date));
}