using HealthCare.Api.DTOs.Patient;
using HealthCare.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class AdminPatientController : ControllerBase
{
    private readonly IPatientService _patientService;


    public AdminPatientController(IPatientService patientService)
    {
        _patientService = patientService;

    }

    // Patient admin endpoints

    [HttpGet("/patients/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetPatientById(int id)
    {
        var result = await _patientService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("/patients")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllPatient([FromQuery] PatientFilter filter)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _patientService.GetAllAsync(filter);
        return Ok(result);
    }

    [HttpPut("/patients/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdatePatient(int id, [FromBody] UpdatePatientDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _patientService.UpdateAsync(id, dto);
        return Ok();
    }

    [HttpPatch("/patients/{id}/status")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdatePatientStatus(int id, [FromBody] bool isActive)
    {
        await _patientService.UpdateStatusAsync(id, isActive);
        return Ok();
    }

    [HttpDelete("/patients/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePatient(int id)
    {
        await _patientService.DeleteAsync(id);
        return Ok();
    }
}
