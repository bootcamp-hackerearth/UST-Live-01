using HealthApp.API.Constants;
using HealthApp.API.Models.DTOs;
using HealthApp.API.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.API.Controllers;

[ApiController]
[Route("api/health-records")]
[Authorize]
public class HealthRecordsController(IHealthRecordService service) : ControllerBase
{
    [HttpGet("{patientId:int}")]
    [Authorize(Roles = Roles.Patient + "," + Roles.Doctor + "," + Roles.Admin)]
    public async Task<ActionResult<List<HealthRecordDto>>> ByPatient(int patientId)
        => Ok(await service.GetHealthRecordsByPatientIdAsync(patientId));

    [HttpGet("record/{id:int}")]
    [Authorize(Roles = Roles.Patient + "," + Roles.Doctor + "," + Roles.Admin)]
    public async Task<ActionResult<HealthRecordDto>> ById(int id)
        => Ok(await service.GetHealthRecordByIdAsync(id));

    [HttpPost]
    [Authorize(Roles = Roles.Doctor + "," + Roles.Admin)]
    public async Task<ActionResult<HealthRecordDto>> Post(AddHealthRecordDto dto)
        => Ok(await service.AddHealthRecordAsync(dto));
}