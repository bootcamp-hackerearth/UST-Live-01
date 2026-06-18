using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Doctor")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors =await _doctorService.GetAllAsync();

            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);

            return Ok(doctor);
        }

        [HttpGet("{id}/availability")]
        public async Task<IActionResult> GetDoctorAvailability(int id)
        {
            var doctor =await _doctorService.GetAvailabilityAsync(id);

            return Ok(new
            {
                doctor.DoctorId,
                doctor.FullName,
                doctor.IsActive,
                Message = doctor.IsActive? "Doctor is available" : "Doctor is not available"
            });
        }
    }
}