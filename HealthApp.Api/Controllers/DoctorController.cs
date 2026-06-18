using HealthApp.Api.Dtos;
using HealthApp.Api.Enums;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.Api.Controllers
{
    [ApiController]
    [Route("api/doctors")]
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
        public async Task<IActionResult> GetDoctors(
            [FromQuery] string? search,
            [FromQuery] SpecialisationType? specialisation,
            [FromQuery] bool? isActive)
        {
            var doctors = await _doctorService.SearchDoctorsAsync(
                search,
                specialisation,
                isActive);

            return Ok(doctors);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);

            return Ok(doctor);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDoctor([FromBody] DoctorCreateDto dto)
        {
            await _doctorService.AddDoctorAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created,
                new { message = "Doctor created successfully." });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDoctor(
            int id,
            [FromBody] DoctorCreateDto dto)
        {
            await _doctorService.UpdateDoctorAsync(id, dto);

            return Ok(new { message = "Doctor updated successfully." });
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> ChangeDoctorStatus(
            int id,
            [FromQuery] bool isActive)
        {
            await _doctorService.ChangeStatusAsync(id, isActive);

            return Ok(new { message = "Doctor status updated successfully." });
        }

        [HttpGet("specialisation/{specialisation}")]
        public async Task<IActionResult> GetDoctorsBySpecialisation(
            string specialisation)
        {
            if (!Enum.TryParse(
                    specialisation,
                    true,
                    out SpecialisationType specialisationType))
            {
                throw new InvalidRequestException("Invalid specialisation.");
            }

            var doctors = await _doctorService.GetDoctorsBySpecialisationAsync(
                specialisationType);

            return Ok(doctors);
        }

        [HttpGet("{id:int}/availability")]
        public async Task<IActionResult> GetDoctorAvailability(
            int id,
            [FromQuery] DateOnly date)
        {
            var slots = await _appointmentService.GetAvailableSlotsAsync(id, date);

            return Ok(slots);
        }
    }
}