using HealthApp.API.Service.Interface;
using HealthApp.Shared.Constants;
using HealthApp.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.API.Controllers;

[ApiController]
[Route("api/patients")]
[Authorize]
public class PatientsController(
    IPatientService patientService,
    IHealthRecordService healthRecordService) : ControllerBase
{
    [HttpGet("{id:int}")]
    [Authorize(Roles = Roles.Patient + "," + Roles.Doctor + "," + Roles.Admin)]
    public async Task<ActionResult<PatientDto>> Get(int id)
        => Ok(await patientService.GetPatientByIdAsync(id));

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Patient + "," + Roles.Admin)]
    public async Task<ActionResult<PatientDto>> Put(int id, UpdatePatientDto dto)
        => Ok(await patientService.UpdatePatientAsync(id, dto));

    [HttpGet("{id:int}/health-records")]
    [Authorize(Roles = Roles.Patient + "," + Roles.Doctor)]
    public async Task<ActionResult<List<HealthRecordDto>>> Records(int id)
    {
        await patientService.EnsurePatientAccessAsync(id);

        return Ok(await healthRecordService.GetHealthRecordsByPatientIdAsync(id));
    }
}