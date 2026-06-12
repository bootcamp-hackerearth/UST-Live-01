using HealthApp.API.Data;
using HealthApp.API.Service.Interface;
using HealthApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;

namespace HealthApp.API.Controllers
{
    [RoutePrefix("api/appointments")]
    public class AppointmentApiController : ApiController
    {
        private readonly IAppointmentService _service;
        private readonly HealthAppEntities _db;

        public AppointmentApiController(IAppointmentService service)
        {
            _service = service;
            _db = new HealthAppEntities();
        }

        // ✅ GET ALL
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<AppointmentDto>> Get()
        {
            return await _service.GetAllAppointments();
        }

        // ✅ GET BY ID
        [HttpGet]
        [Route("{id}")]
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
        [Route("")]
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
        [Route("{id}/cancel")]
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
        [Route("{id}/confirm")]
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
        [Route("{id}/complete")]
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

        // ✅ ✅ ✅ GET PATIENT NAME
        [HttpGet]
        [Route("patient/{id}")]
        public async Task<IHttpActionResult> GetPatient(int id)
        {
            try
            {
                var patient = await _db.Patients
                    .Where(p => p.PatientId == id)
                    .Select(p => new
                    {
                        doctorId = 0,      // not used, but keeps JSON consistent
                        fullName = p.FullName
                    })
                    .FirstOrDefaultAsync();

                if (patient == null)
                    return BadRequest("Invalid Patient Id");

                return Ok(patient);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ ✅ ✅ GET DOCTORS BY SPECIALIZATION
        [HttpGet]
        [Route("doctors")]
        public async Task<IHttpActionResult> GetDoctors(string specialization)
        {
            try
            {
                var doctors = await _db.Doctors
                    .Where(d => d.Specialisation == specialization && d.IsActive)
                    .Select(d => new
                    {
                        doctorId = d.DoctorId,
                        fullName = d.FullName
                    })
                    .ToListAsync();

                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ ✅ ✅ GET AVAILABLE SLOTS
        [HttpGet]
        [Route("availability")]
        public async Task<IHttpActionResult> GetAvailability(int doctorId, DateTime date)
        {
            try
            {
                var slots = await _service.CheckDoctorAvailability(doctorId, date);
                return Ok(slots);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
