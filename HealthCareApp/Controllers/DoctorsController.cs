using HealthCareApp.Dtos;
using HealthCareApp.Enums;
using HealthCareApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController(IDoctorService service) : ControllerBase
    {
        // Public endpoint:
        // Doctor submits profile request.
        // No password is created here.
        // VerificationStatus = Pending, IsActive = false.
      
        // Public endpoint:
        // Patients/public users can view active approved doctors.
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllActiveDoctors()
        {
            var result = await service.GetAllActiveDoctorsAsync();

            return Ok(result);
        }

        // Public endpoint:
        // View doctor profile by id.
        [HttpGet("{doctorId:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDoctorById([FromRoute] int doctorId)
        {
            var result = await service.GetDoctorByIdAsync(doctorId);

            return Ok(result);
        }

        // Public endpoint:
        // Filter active doctors by specialisation.
        [HttpGet("specialisation/{specialisation}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetActiveDoctorsBySpecialisation(
            [FromRoute] SpecialisationType specialisation)
        {
            var result = await service.GetActiveDoctorsBySpecialisationAsync(specialisation);

            return Ok(result);
        }

        // Admin-only endpoint:
        // Admin can view all doctors, including pending/rejected/inactive doctors.
        [HttpGet("all")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var result = await service.GetAllDoctorsAsync();

            return Ok(result);
        }

        // Admin-only endpoint:
        // Admin can update doctor profile details.
        // Doctor should not update full profile details.
        [HttpPut("{doctorId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> UpdateDoctor(
            [FromRoute] int doctorId,
            [FromBody] UpdateDoctorDto request)
        {
            var result = await service.UpdateDoctorAsync(doctorId, request);

            return Ok(result);
        }

        // Admin-only endpoint:
        // Admin can delete doctor if your current service supports hard delete.
        // Later we can replace this with deactivate/reactivate.
        [HttpDelete("{doctorId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> DeleteDoctor([FromRoute] int doctorId)
        {
            var result = await service.DeleteDoctorAsync(doctorId);

            return Ok(result);
        }
    }
}