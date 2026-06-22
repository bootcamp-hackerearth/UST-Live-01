using HealthApp.Api.Exceptions;
using HealthApp.Api.Extensions;
using HealthApp.Api.Services.Interfaces;
using HealthApp.Shared.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.Api.Controllers
{
    [ApiController]
    [Route("api/patients/profile")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
    public class PatientProfileController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientProfileController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyProfile()
        {
            var patientId = User.GetPatientId();

            if (patientId == null)
            {
                throw new ForbiddenAccessException(
                    "Patient profile is not linked to this user.");
            }

            var patient = await _patientService.GetPatientByIdAsync(
                patientId.Value);

            return Ok(patient);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMyProfile(
            [FromBody] PatientCreateDto dto)
        {
            var patientId = User.GetPatientId();

            if (patientId == null)
            {
                throw new ForbiddenAccessException(
                    "Patient profile is not linked to this user.");
            }

            await _patientService.UpdatePatientAsync(
                patientId.Value,
                dto);

            return Ok(new
            {
                message = "Patient profile updated successfully."
            });
        }
    }
}