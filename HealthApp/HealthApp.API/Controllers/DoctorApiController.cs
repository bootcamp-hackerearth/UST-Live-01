using HealthApp.API.Service.Interface;
using HealthApp.Shared.Constant;
using HealthApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace HealthApp.API.Controllers
{
    [RoutePrefix("api/doctors")]
    public class DoctorApiController : ApiController
    {
        private readonly IDoctorService _service;

        public DoctorApiController(IDoctorService service)
        {
            _service = service;
        }

        // ✅ GET all doctors
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<DoctorDto>> GetAll()
        {
            return await _service.GetAllDoctors();
        }

        // ✅ GET doctor by ID
        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            try
            {
                var doctor = await _service.GetDoctorById(id);
                return Ok(doctor);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ CREATE doctor
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Create(DoctorDto dto)
        {
            try
            {
                await _service.AddDoctor(dto);
                return Ok("Doctor added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ SEARCH by specialisation
        [HttpGet]
        [Route("specialisation/{type}")]
        public async Task<IHttpActionResult> SearchBySpecialisation(SpecialisationType type)
        {
            var result = await _service.SearchBySpecialisation(type);
            return Ok(result);
        }

        // ✅ TOGGLE active/inactive
        [HttpPut]
        [Route("{id}/toggle")]
        public async Task<IHttpActionResult> ToggleStatus(int id)
        {
            try
            {
                await _service.ChangeDoctorStatus(id);
                return Ok("Doctor status updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IHttpActionResult> Update(int id, DoctorDto dto)
        {
            try
            {
                await _service.UpdateDoctor(id, dto);
                return Ok("Doctor updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}