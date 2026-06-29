using HealthAxisApplicn.Dto.Doctors;
using HealthAxisApplicn.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxisApplicn.Controllers
{
    [Route("api/doctors")]
    [ApiController]
    [Authorize]
    public class DoctorController(IDoctorService service) : ControllerBase
    {
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]    
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

        [HttpPut("toggle/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var result = await service.ToggleActiveAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateDoctorDto entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
    }

            var result = await service.CreateAsync(entity);

            return Ok(result); 
}


        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDoctorDto entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await service.UpdateAsync(id, entity);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveDoctors()
        {
            return Ok(await service.GetActiveDoctorsAsync());
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> SearchByName(string name)
        {
            var result = await service.SearchByNameAsync(name);
            return result.Count == 0 ? NotFound() : Ok(result);
        }

        [HttpGet("specialisation/{specialisation}")]
        public async Task<IActionResult> SearchBySpecialisation(string specialisation)
        {
            var result = await service.SearchBySpecialisationAsync(specialisation);
            return result.Count == 0 ? NotFound() : Ok(result);
        }


        [HttpGet("search")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminOrPatient")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            var doctors = await service.SearchAsync(query);
            return Ok(doctors);
        }

    }
}
