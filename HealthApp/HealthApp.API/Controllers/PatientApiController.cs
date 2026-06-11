using HealthApp.API.Data;
using HealthApp.API.Repository.Impl;
using HealthApp.API.Service.Impl;
using HealthApp.API.Service.Interface;
using HealthApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace HealthApp.API.Controllers
{
    [RoutePrefix("api/patients")]
    public class PatientApiController : ApiController
    {

        private readonly IPatientService _service;

        public PatientApiController(IPatientService service)
        {
            _service = service;
        }

        // ✅ GET all patients
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<PatientDto>> Get()
        {
            return await _service.GetAll();
        }

        // ✅ GET patient by ID
        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            try
            {
                var patient = await _service.GetPatientById(id);
                return Ok(patient);
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.NotFound, ex.Message);
            }
        }

        // ✅ CREATE new patient
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(PatientDto dto)
        {
            try
            {
                await _service.RegisterPatient(dto);
                return Ok("Patient registered successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ UPDATE patient
        [HttpPut]
        [Route("{id}")]
        public async Task<IHttpActionResult> Put(int id, PatientDto dto)
        {
            try
            {
                await _service.UpdatePatientById(id, dto);
                return Ok("Patient updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}