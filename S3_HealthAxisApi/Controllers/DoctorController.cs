using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using S3_HealthAxis.Shared.DTOs.Doctor;
using S3_HealthAxisApi.Services.Interface;

namespace S3_HealthAxisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;

        public DoctorsController(
            IDoctorService doctorService,
            IAppointmentService appointmentService)
        {
            _doctorService = doctorService;
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? sortBy,
            [FromQuery] int? specialisation)
        {
            var doctors =
                await _doctorService.GetAllAsync(
                    sortBy,
                    specialisation);

            return Ok(doctors);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(
            int id)
        {
            var doctor =
                await _doctorService.GetByIdAsync(id);

            if (doctor == null)
            {
                return NotFound(
                    $"Doctor with Id {id} not found.");
            }

            return Ok(doctor);
        }

        [HttpGet("specialisation/{specialisation:int}")]
        public async Task<IActionResult>
            GetBySpecialisation(
                int specialisation)
        {
            var doctors =
                await _doctorService
                    .GetActiveBySpecialisationAsync(
                        specialisation);

            return Ok(doctors);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(
            [FromBody] CreateDoctorDto dto)
        {
            var doctor =
                await _doctorService
                    .CreateDoctorWithAccountAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = doctor.DoctorId },
                doctor);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateDoctorDto dto)
        {
            await _doctorService.UpdateAsync(
                id,
                dto);

            return NoContent();
        }

        [HttpGet("{id:int}/availability")]
        public async Task<IActionResult>
            GetAvailability(
                int id,
                [FromQuery] DateOnly date)
        {
            var slots =
                await _doctorService
                    .GetAvailabilityAsync(
                        id,
                        date);

            return Ok(slots);
        }

        [HttpPut("{id:int}/activate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Activate(
            int id)
        {
            await _doctorService.ActivateAsync(id);

            return NoContent();
        }

        [HttpGet("{id:int}/patients")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetDoctorPatients(int id)
        {
            try
            {
                if (User.IsInRole("Doctor"))
                {
                    var doctorIdFromToken = GetReferenceIdFromToken();

                    if (!doctorIdFromToken.HasValue ||
                        doctorIdFromToken.Value != id)
                    {
                        return Forbid();
                    }
                }

                var patients =
                    await _appointmentService.GetDoctorPatientsAsync(id);

                return Ok(patients);
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
            await _doctorService.DeactivateAsync(id);

            return NoContent();
        }
        private int? GetReferenceIdFromToken()
        {
            var referenceIdValue =
                User.FindFirst("ReferenceId")?.Value;

            if (int.TryParse(referenceIdValue, out var referenceId))
            {
                return referenceId;
            }

            return null;
        }

    }

}
