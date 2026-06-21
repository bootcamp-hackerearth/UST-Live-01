using HealthCare.Api.DTOs.Doctor;
using HealthCare.Api.DTOs.Patient;
using HealthCare.Api.Services.Implementations;
using HealthCare.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Api.Controllers
{
    [Route("api/doctors")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _service;

        public DoctorController(IDoctorService service)
        {
            _service = service;
        }

        [HttpPut("my-profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
        public async Task<IActionResult> GetMyProfile()
        {
            var doctorId = GetDoctorIdFromClaims();
            var result = await _service.GetByIdAsync(doctorId);
            return Ok(result);
        }


        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
   
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDoctorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.UpdateAsync(id, dto);

            return Ok(new { message = "Doctor profile updated successfully" });
        }


        [HttpPost("leaves")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> AddDoctorLeaves([FromBody] List<CreateLeaveDto> leaves)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var doctorId = GetDoctorIdFromClaims();
            var result = await _service.CreateLeave(doctorId, leaves);
            return Ok(new { message = "Leaves added successfully" });
        }

        //Get doctors
        [HttpGet("available")]
        [AllowAnonymous]
        public async Task<IActionResult> AvailableDoctors([FromQuery] string specialisation,[FromQuery] DateOnly date)
        {
            if (string.IsNullOrWhiteSpace(specialisation))
                return BadRequest("Specialisation is required");

            var result = await _service.AvailableDoctors(specialisation, date);

            return Ok(result);
        }

        private int GetDoctorIdFromClaims()
        {
            var claim = User.FindFirst("DoctorId")
                ?? throw new InvalidOperationException("DoctorId claim not found in token.");

            return int.Parse(claim.Value);
        }



    }
}
