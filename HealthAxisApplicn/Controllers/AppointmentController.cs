using HealthAxisApplicn.Dto.Appointments;
using HealthAxisApplicn.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthAxisApplicn.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    [Authorize]
    public class AppointmentController(IAppointmentService service) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var result = await service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await service.GetByIdAsync(id);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDto entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // ✅ GET PatientId FROM JWT
                var patientId = int.Parse(User.FindFirst("PatientId")!.Value);

                // ✅ CALL SERVICE
                var result = await service.CreateAsync(entity, patientId);

                return CreatedAtAction("GetById", new { id = result.AppointmentId }, result);
            }
            catch (Exception ex)
            {
                // ✅ RETURN CLEAN ERROR MESSAGE
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("my")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMyAppointments()
        {
            var patientId = int.Parse(User.FindFirst("PatientId")!.Value);

            var result = await service.GetAppointmentsByPatientIdAsync(patientId);

            return Ok(result);
        }

        [HttpGet("doctor/my")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorAppointments()
        {
            var doctorId = int.Parse(User.FindFirst("DoctorId")!.Value);

            var result = await service.GetAppointmentsByDoctorIdAsync(doctorId);

            return Ok(result);
        }


        [HttpPut("{id:int}")]
        [Authorize(Roles = "Doctor,Patient")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAppointmentStatusDto entity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // ✅ get user role
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            var result = await service.UpdateAsync(id, entity, role!);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("patient/{patientId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var result = await service.GetAppointmentsByPatientIdAsync(patientId);
            return result.Count == 0 ? NotFound() : Ok(result);
        }

        [HttpGet("doctor/{doctorId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByDoctor(int doctorId)
        {
            var result = await service.GetAppointmentsByDoctorIdAsync(doctorId);
            return result.Count == 0 ? NotFound() : Ok(result);
        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await service.DeleteAppointmentAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();

        }

        [HttpGet("doctor/today")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetTodayAppointments()
        {
            var doctorId = int.Parse(User.FindFirst("DoctorId")!.Value);

            var result = await service.GetTodayAppointmentsAsync(doctorId);

            return Ok(result);
        }

    }
}
