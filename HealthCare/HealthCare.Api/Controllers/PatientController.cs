using HealthCare.Api.DTOs.Appointment;
using HealthCare.Api.DTOs.Patient;
using HealthCare.Api.Models;
using HealthCare.Api.Services.Implementations;
using HealthCare.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Api.Controllers
{
    [Route("api/patients")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _service;

        public PatientController(IPatientService service)
        {
            _service = service;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll(PatientFilter filter)
        {
            var patients = await _service.GetAllAsync(filter);
            return Ok(patients);
        }


        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _service.GetByIdAsync(id);

            if (patient is null)
                return NotFound();

            return Ok(patient);
        }


        [HttpPut]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult> Update(int id,  UpdatePatientDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id == null)
                return Unauthorized();


            await _service.UpdateAsync(id, dto);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }


        //Serach by name 
        [HttpGet("search")]
        public async Task<IActionResult> SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Name is required");

            var result = await _service.SearchByNameAsync(name);

            if (!result.Any())
                return NotFound($"No patients found with name '{name}'");

            return Ok(result);
        }

        //Get Profile
        //[HttpGet("me")]
        //[Authorize(Roles = "Patient")]
        //public async Task<IActionResult> GetMyProfile()
        //{
        //    var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        //    if (userId == null)
        //        return Unauthorized();

        //    var patient = await _service.GetByIdAsync(userId);

        //    if (patient == null)
        //        return NotFound();

        //    return Ok(patient);
        //}

    }
}
