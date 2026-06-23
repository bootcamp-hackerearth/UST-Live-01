using HealthApp.API.Service.Interface;
using HealthApp.Shared.Constants;
using HealthApp.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.API.Controllers;

[ApiController]
[Route("api/health-records")]
[Authorize]
public class HealthRecordsController(
    IHealthRecordService service,
    IPatientService patientService) : ControllerBase
{
    [HttpGet("{patientId:int}")]
    [Authorize(Roles = Roles.Patient + "," + Roles.Doctor)]
    public async Task<ActionResult<List<HealthRecordDto>>> ByPatient(int patientId)
    {
        await patientService.EnsurePatientAccessAsync(patientId);

        return Ok(await service.GetHealthRecordsByPatientIdAsync(patientId));
    }

    [HttpGet("record/{id:int}")]
    [Authorize(Roles = Roles.Patient + "," + Roles.Doctor)]
    public async Task<ActionResult<HealthRecordDto>> ById(int id)
    {
        var healthRecord = await service.GetHealthRecordByIdAsync(id);

        await patientService.EnsurePatientAccessAsync(healthRecord.PatientId);

        return Ok(healthRecord);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Doctor)]
    public async Task<ActionResult<HealthRecordDto>> Post(AddHealthRecordDto dto)
        => Ok(await service.AddHealthRecordAsync(dto));
}