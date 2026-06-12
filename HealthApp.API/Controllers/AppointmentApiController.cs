using HealthApp.API.Service.Interface;
using HealthApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace HealthApp.API.Controllers
{
    public class AppointmentApiController : ApiController
    {
        private readonly IAppointmentService _service;

        public AppointmentApiController(IAppointmentService service)
        {
            _service = service;
        }

        // ✅ GET ALL
        [HttpGet]
        [Route("api/appointments")]
        public async Task<IEnumerable<AppointmentDto>> Get()
        {
            return await _service.GetAllAppointments();
        }

        // ✅ GET BY ID
        [HttpGet]
        [Route("api/appointments/{id}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            try
            {
                var data = await _service.GetAppointmentById(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ CREATE
        [HttpPost]
        [Route("api/appointments")]
        public async Task<IHttpActionResult> Post(AppointmentDto dto)
        {
            try
            {
                await _service.Add(dto);
                return Ok("Appointment created successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ CANCEL
        [HttpPut]
        [Route("api/appointments/{id}/cancel")]
        public async Task<IHttpActionResult> Cancel(int id, string reason)
        {
            try
            {
                await _service.CancelAppointment(id, reason);
                return Ok("Appointment cancelled");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ CONFIRM
        [HttpPut]
        [Route("api/appointments/{id}/confirm")]
        public async Task<IHttpActionResult> Confirm(int id)
        {
            try
            {
                await _service.ConfirmAppointment(id);
                return Ok("Appointment confirmed");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ COMPLETE
        [HttpPut]
        [Route("api/appointments/{id}/complete")]
        public async Task<IHttpActionResult> Complete(int id)
        {
            try
            {
                await _service.CompleteAppointment(id);
                return Ok("Appointment completed");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}