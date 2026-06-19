using HealthApp.Api.Dto;
using HealthApp.Api.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.Api.Controllers
{
    [Route("api/doctors")]
    [ApiController]
    public class DoctorApiController : ControllerBase
    {
        private readonly IDoctorService _service;

        public DoctorApiController(IDoctorService service)
        {
            _service = service;
        }

        // GET all active doctors
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllActive()
        {
            var data = await _service.GetAllActiveDoctorAsync();
            return Ok(data);
        }

        // GET all doctors
        [HttpGet("all")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllDoctorsAsync();
            return Ok(data);
        }

        // GET doctor by ID
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor,Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var doctor = await _service.GetDoctorByIdAsync(id);
            return Ok(doctor);
        }

        // CREATE doctor
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] DoctorDto dto)
        {
            var result = await _service.AddDoctorAsync(dto);
            return Ok(result);
        }

        // SEARCH by specialisation
        [HttpGet("specialisation/{type}")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchBySpecialisation(string type)
        {
            var result = await _service.SearchBySpecialisationAsync(type);
            return Ok(result);
        }

        // UPDATE doctor
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] DoctorDto dto)
        {
            var result = await _service.UpdateDoctorByIdAsync(id, dto);
            return Ok(result);
        }
    }
}