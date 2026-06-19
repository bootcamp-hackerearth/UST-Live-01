using HealthApp.Api.Dto;
using HealthApp.Api.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientApiController : ControllerBase
    {

        private readonly IPatientService _service;

        public PatientApiController(IPatientService service)
        {
            _service = service;
        }

        // GET all patients
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var data= await _service.GetAllPatientsAsync();
            return Ok(data);
              
            
        }

        // GET patient by ID
        [HttpGet]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles ="Doctor,Admin"  )]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _service.GetPatientByIdAsync(id);
            return Ok(patient);
            
        }

        // CREATE new patient
        [HttpPost]
        [Route("")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> Create([FromBody] PatientDto dto)
        {
            await _service.AddPatientAsync(dto);
            return Ok("Patient registered successfully");
            
            
        }

        //  UPDATE patient
        [HttpPut]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> update(int id, [FromBody] PatientDto dto)
        {
            await _service.UpdatePatientByIdAsync(id, dto);
            return Ok("Patient updated successfully");
            
        }
    }

}