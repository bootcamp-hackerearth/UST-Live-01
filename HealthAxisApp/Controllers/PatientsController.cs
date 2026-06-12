using HealthAxisApp.Services;
using HealthAxisApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HealthAxisApp.Controllers
{
    [RoutePrefix("api/patients")]
    public class PatientsController : ApiController
    {
        private readonly IPatientService _service;

        public PatientsController(IPatientService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll(string insuranceStatus = null, string searchText = null)
        {
            var patients = _service.GetAll(insuranceStatus, searchText);

            return Ok(patients);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var patient = _service.GetById(id);

            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(PatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string errorMessage;
            int patientId;

            bool result = _service.Create(
                dto,
                out errorMessage,
                out patientId);

            if (!result)
            {
                return BadRequest(errorMessage);
            }

            return Ok(new
            {
                Message = "Patient created successfully.",
                PatientId = patientId
            });
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Update(int id, PatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string errorMessage;

            bool result = _service.Update(id, dto, out errorMessage);

            if (!result)
            {
                return BadRequest(errorMessage);
            }

            return Ok("Patient updated.");
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            bool result = _service.Delete(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok("Patient deleted.");
        }
    }
}
