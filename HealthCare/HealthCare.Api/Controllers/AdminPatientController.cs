using HealthCare.Api.DTOs.Authentication;
using HealthCare.Api.DTOs.Patient;
using HealthCare.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class AdminPatientController : ControllerBase
{
    private readonly IPatientService _patientService;
    private readonly IAuthService _authService;
    public AdminPatientController(IPatientService patientService, IAuthService authService)
    {
        _patientService = patientService;
        _authService = authService;
    }

    // Get patient by id

    [HttpGet("/patients/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<IActionResult> GetPatientById(int id)
    {
        var result = await _patientService.GetByIdAsync(id);
        if (result == null) return NotFound("Patient Not Found");
        return Ok(result);
    }

    //Get All pat
    [HttpGet("/patients")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<IActionResult> GetAllPatient([FromQuery] PatientFilter filter)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _patientService.GetAllAsync(filter);
        return Ok(result);
    }

    //update patient
    [HttpPut("/patients/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<IActionResult> UpdatePatient(int id, [FromBody] UpdatePatientDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _patientService.UpdateAsync(id, dto);
        return Ok();
    }

    //Update status
    [HttpPatch("/patients/{id}/status")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<IActionResult> UpdatePatientStatus(int id, [FromBody] bool isActive)
    {
        await _patientService.UpdateStatusAsync(id, isActive);
        return Ok();
    }

    //delete patient
    [HttpDelete("/patients/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<IActionResult> DeletePatient(int id)
    {
        await _patientService.DeleteAsync(id);
        return Ok();
    }

    //register
    [HttpPost("register/patient")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegisterPatient(PatientRegisterDto dto)
    {
        await _authService.RegisterPatientAsync(dto);
        return Ok(new { message = "Registration successful" });
    }

    //Serach by name 
    [HttpGet("search")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<IActionResult> SearchByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest("Name is required");

        var result = await _patientService.SearchByNameAsync(name);

        if (!result.Any())
            return NotFound($"No patients found with name '{name}'");

        return Ok(result);
    }

}}
