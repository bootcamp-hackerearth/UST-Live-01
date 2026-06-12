using AutoMapper;
using Healthaxis2.Shared.DTOs;
using System.Collections.Generic;
using System.Web.Http;
using Healthaxis2.Models;
using Healthaxis2.Services.Interfaces;

namespace Healthaxis2.Controllers
{
    [RoutePrefix("api/appointments")]
    public class AppointmentsController : ApiController
    {
        private readonly IAppointmentService _service;

        // ✅ Service injected (DI)
        public AppointmentsController(IAppointmentService service)
        {
            _service = service;
        }

        // ✅ GET ALL
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            var data = _service.GetAll();

            var result = Mapper.Map<List<AppointmentDto>>(data);

            return Ok(result);
        }

        // ✅ GET BY ID
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var data = _service.GetById(id);

            if (data == null)
                return NotFound();

            var result = Mapper.Map<AppointmentDto>(data);

            return Ok(result);
        }

        // ✅ CREATE (WITH SLOT VALIDATION INSIDE SERVICE 🔥)
        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(AppointmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var model = Mapper.Map<Appointment>(dto);

            var response = _service.Create(model);

            if (response != "Success")
                return BadRequest(response);

            return Ok(new
            {
                message = "Appointment booked successfully"
            });
        }

        // ✅ UPDATE STATUS
        [HttpPut]
        [Route("{id}/status")]
        public IHttpActionResult UpdateStatus(int id, string status, string reason = null)
        {
            _service.UpdateStatus(id, status, reason);

            return Ok(new
            {
                message = "Status updated successfully"
            });
        }

        // ✅ DELETE
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            _service.Delete(id);

            return Ok("Appointment deleted successfully");
        }
    }
}