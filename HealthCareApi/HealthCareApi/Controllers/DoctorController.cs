//using HealthCareApi.Models;
using AutoMapper;
using HealthCare.Shared.DTOs.Doctor;
using HealthCareApi.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HealthCare.Shared;

namespace HealthCareApi.Controllers
{
    [RoutePrefix("api/doctors")]
    public class DoctorController : ApiController
    {
        private readonly IDoctorService _doctorService;
        private readonly IMapper _mapper;

        public DoctorController(IDoctorService doctorService, IMapper mapper)
        {
            _doctorService = doctorService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetDoctors(
            [FromUri] string specialization = null,
            [FromUri] string searchTerm = null,
            [FromUri] bool orderByDescending = false,
            [FromUri] int pageNumber = 1,
            [FromUri] int pageSize = 10)
        {
            var result = await _doctorService.GetFilteredDoctorsAsync(
                specialization,
                searchTerm,
                orderByDescending,
                pageNumber,
                pageSize);

            //  map ONLY Items
            var doctorDtos = _mapper.Map<IEnumerable<DoctorDto>>(result.Items);

            //  wrap in paged result
            return Ok(new PagedResult<DoctorDto>
            {
                Items = doctorDtos.ToList(),
                TotalCount = result.TotalCount,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            });
        }



        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null) return NotFound();

            var doctorDto = _mapper.Map<DoctorDto>(doctor);

            return Ok(doctorDto);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Add(Doctor doctor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdDoctor = await _doctorService.AddDoctorAsync(doctor);

            var dto = _mapper.Map<DoctorDto>(createdDoctor);

            // ✅ Correct REST response
            return Created($"api/doctors/{dto.DoctorId}", dto);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Update(int id, Doctor doctor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != doctor.DoctorId)
                return BadRequest("Doctor ID mismatch");

            var updatedDoctor = await _doctorService.UpdateDoctorAsync(doctor);

            if (updatedDoctor == null)
                return NotFound();

            var dto = _mapper.Map<DoctorDto>(updatedDoctor);

            return Ok(dto);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var result = await _doctorService.DeleteDoctorAsync(id);

            if (!result)
                return NotFound();

            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }
    }
}