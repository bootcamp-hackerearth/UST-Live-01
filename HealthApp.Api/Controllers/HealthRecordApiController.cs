using HealthApp.Api.Dto;
using HealthApp.Api.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthRecordApiController : ControllerBase
    {

        private readonly IHealthRecordService _service;

        public HealthRecordApiController(IHealthRecordService service)
        {
            _service = service;
        }

        // GET ALL
        [HttpGet]
        [Route("")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllRecordsAsync();
            return Ok(data);
        }

        // FILTER
        [HttpGet]
        [Route("filter")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor,Admin")]
        public async Task<IActionResult> GetByDoctorAndPatient([FromQuery] int doctorId, [FromQuery] int patientId)
        {
            var data = await _service.GetHealthRecordsByDoctorAsync(doctorId, patientId);
            return Ok(data);
        }

        // CREATE
        [HttpPost]
        [Route("")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
        public async Task<IActionResult> Create(HealthRecordDto dto)
        {
                await _service.AddRecordAsync(dto);
                return Ok("Created Successfully");
            
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor,Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetRecordByIdAsync(id);
            return Ok(data);
        }


    }
}
