using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using S3_HealthAxisApi.DTOs.Doctor;
using S3_HealthAxisApi.Services.Interface;

namespace S3_HealthAxisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? sortBy,
            [FromQuery] int? specialisation)
        {
            var doctors = await _doctorService
                .GetAllAsync(sortBy, specialisation);

            return Ok(doctors);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);

            if (doctor == null)
            {
                return NotFound(
                    $"Doctor with Id {id} not found.");
            }

            return Ok(doctor);
        }

        [HttpGet("specialisation/{specialisation:int}")]
        public async Task<IActionResult> GetBySpecialisation(
            int specialisation)
        {
            try
            {
                var doctors =
                    await _doctorService
                        .GetActiveBySpecialisationAsync(
                            specialisation);

                return Ok(doctors);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(
            CreateDoctorDto dto)
        {
            try
            {
                var doctor =
                    await _doctorService.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = doctor.DoctorId },
                    doctor);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            UpdateDoctorDto dto)
        {
            try
            {
                await _doctorService.UpdateAsync(id, dto);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}/activate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Activate(
            int id)
        {
            try
            {
                await _doctorService.ActivateAsync(id);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id:int}/deactivate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deactivate(
            int id)
        {
            try
            {
                await _doctorService.DeactivateAsync(id);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}   