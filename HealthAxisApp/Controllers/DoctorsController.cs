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
    [RoutePrefix("api/doctors")]
    public class DoctorsController : ApiController
    {
        private readonly IDoctorService _service;

        public DoctorsController(IDoctorService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll(
            string specialisation = null,
            string searchText = null,
            bool activeOnly = false)
        {
            var doctors = _service.GetAll(specialisation, searchText, activeOnly);

            return Ok(doctors);
        }


        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var doctor = _service.GetById(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(DoctorDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string errorMessage;
            int doctorId;

            bool result = _service.Create(
                dto,
                out errorMessage,
                out doctorId);

            if (!result)
            {
                return BadRequest(errorMessage);
            }

            return Ok(new
            {
                Message = "Doctor created successfully.",
                DoctorId = doctorId
            });
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Update(int id, DoctorDto dto)
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

            return Ok("Doctor updated.");
        }

        [HttpPut]
        [Route("{id:int}/toggle-status")]
        public IHttpActionResult ToggleStatus(int id)
        {
            bool result = _service.ToggleStatus(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok("Doctor status updated.");
        }
    }
}
