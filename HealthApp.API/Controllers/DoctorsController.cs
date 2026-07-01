using HealthApp.API.Service.Interface;
using HealthApp.Shared.Constants;
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.API.Controllers;

[ApiController]
[Route("api/doctors")]
[Authorize]
public class DoctorsController(IDoctorService doctorService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = Roles.Patient + "," + Roles.Admin)]
    public async Task<ActionResult<List<DoctorDto>>> Get(
        [FromQuery] SpecialisationType? specialisation)
        => Ok(
            specialisation.HasValue
                ? await doctorService.GetDoctorsBySpecialisationAsync(specialisation.Value)
                : await doctorService.GetAllDoctorsAsync());

    [HttpGet("{id:int}")]
    [Authorize(Roles = Roles.Patient + "," + Roles.Admin + "," + Roles.Doctor)]
    public async Task<ActionResult<DoctorDto>> GetById(int id)
        => Ok(await doctorService.GetDoctorByIdAsync(id));

    [HttpGet("me")]
    [Authorize(Roles = Roles.Doctor)]
    public async Task<ActionResult<DoctorDto>> GetMe()
        => Ok(await doctorService.GetLoggedInDoctorProfileAsync());

    [HttpGet("{id:int}/availability")]
    [Authorize(Roles = Roles.Patient + "," + Roles.Admin + "," + Roles.Doctor)]
    public async Task<ActionResult<DoctorAvailabilityDto>> Availability(
        int id,
        [FromQuery] DateTime date)
        => Ok(await doctorService.GetDoctorAvailabilityAsync(
            id,
            date == default ? DateTime.Today : date));
}