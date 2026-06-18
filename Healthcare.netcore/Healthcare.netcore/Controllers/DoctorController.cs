using HealthAxis.API.DTOs;
using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/doctors")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _service;

        public DoctorController(IDoctorService service)
        {
            _service = service;
        }

        // ✅ GET all doctors
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var doctors = await _service.GetAllAsync(CancellationToken.None);
            return Ok(doctors);
        }

        // ✅ GET doctor by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var doctor = await _service.GetByIdAsync(id, CancellationToken.None);
            return Ok(doctor);
        }

        // ✅ ✅ FIXED — Get ALL ACTIVE (available) doctors
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableDoctors()
        {
            var doctors = await _service.GetAvailableDoctorsAsync();
            return Ok(doctors);
        }

        // ✅ NEW — Search by name
        [HttpGet("search")]
        public async Task<IActionResult> Search(string name)
        {
            var result = await _service.SearchByNameAsync(name);
            return Ok(result);
        }

        // ✅ NEW — Filter by specialization
        [HttpGet("by-specialisation")]
        public async Task<IActionResult> BySpecialisation(string specialization)
        {
            var result = await _service.GetBySpecialisationAsync(specialization);
            return Ok(result);
        }

        // ✅ ADD doctor (Admin use)
        [HttpPost]
        public async Task<IActionResult> Add(CreateDoctorDto dto)
        {
            var result = await _service.AddAsync(dto);
            return Ok(result);
        }

        // ✅ UPDATE doctor (Admin use)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateDoctorDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return Ok(result);
        }
    }
}