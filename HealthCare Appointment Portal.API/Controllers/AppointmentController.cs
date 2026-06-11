using HealthCare_Appointment_Portal.DTOs.AppointmentDtos;
using HealthCare_Appointment_Portal.Interfaces;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace HealthCare_Appointment_Portal.Controllers
{
    [RoutePrefix("api/appointments")]
    public class AppointmentController
        : ApiController
    {
        private readonly IAppointmentService
            _appointmentService;

        public AppointmentController(
            IAppointmentService appointmentService)
        {
            _appointmentService =
                appointmentService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult>
            GetAllAppointments()
        {
            var appointments =
                await _appointmentService
                    .GetAllAppointmentsAsync();

            return Ok(appointments);
        }

        [HttpGet]
        [Route("{id:int}",
            Name = "GetAppointmentById")]
        public async Task<IHttpActionResult>
            GetAppointmentById(
                int id)
        {
            var appointment =
                await _appointmentService
                    .GetAppointmentByIdAsync(
                        id);

            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointment);
        }


        [HttpGet]
        [Route("patient/{patientId:int}")]
        public async Task<IHttpActionResult>
            GetAppointmentsByPatient(
                int patientId)
        {
            var appointments =
                await _appointmentService
                    .GetAppointmentsByPatientAsync(
                        patientId);

            return Ok(appointments);
        }

        [HttpGet]
        [Route("patient/{patientId:int}/next")]
        public async Task<IHttpActionResult>
            GetNextAppointment(
                int patientId)
        {
            var appointment =
                await _appointmentService
                    .GetNextAppointmentByPatientAsync(
                        patientId);

            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointment);
        }

        [HttpGet]
        [Route("doctor/{doctorId:int}")]
        public async Task<IHttpActionResult>
            GetAppointmentsByDoctor(
                int doctorId)
        {
            var appointments =
                await _appointmentService
                    .GetAppointmentsByDoctorAsync(
                        doctorId);

            return Ok(appointments);
        }

        [HttpGet]
        [Route("doctor/{doctorId:int}/today")]
        public async Task<IHttpActionResult>
            GetTodaySchedule(
                int doctorId)
        {
            var appointments =
                await _appointmentService
                    .GetTodayScheduleAsync(
                        doctorId);

            return Ok(appointments);
        }

        [HttpGet]
        [Route("doctor/{doctorId:int}/weekly")]
        public async Task<IHttpActionResult>
            GetWeeklySchedule(
                int doctorId)
        {
            var appointments =
                await _appointmentService
                    .GetWeeklyScheduleAsync(
                        doctorId);

            return Ok(appointments);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult>
            AddAppointment(
                [FromBody]
                CreateAppointmentDto
                appointmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ModelState);
            }

            try
            {
                int appointmentId =
                    await _appointmentService
                        .AddAppointmentAsync(
                            appointmentDto);

                return CreatedAtRoute(
                    "GetAppointmentById",
                    new
                    {
                        id = appointmentId
                    },
                    new
                    {
                        AppointmentId =
                            appointmentId
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult>
            UpdateAppointment(
                int id,
                [FromBody]
                UpdateAppointmentDto
                appointmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ModelState);
            }

            try
            {
                await _appointmentService
                    .UpdateAppointmentAsync(
                        id,
                        appointmentDto);

                return Ok(
                    "Appointment updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}/confirm")]
        public async Task<IHttpActionResult>
            ConfirmAppointment(
                int id)
        {
            try
            {
                await _appointmentService
                    .ConfirmAppointmentAsync(
                        id);

                return Ok(
                    "Appointment confirmed successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}/cancel")]
        public async Task<IHttpActionResult>
            CancelAppointment(
                int id,
                [FromBody]
                string reason)
        {
            try
            {
                await _appointmentService
                    .CancelAppointmentAsync(
                        id,
                        reason);

                return Ok(
                    "Appointment cancelled successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}/complete")]
        public async Task<IHttpActionResult>
            CompleteAppointment(
                int id)
        {
            try
            {
                await _appointmentService
                    .CompleteAppointmentAsync(
                        id);

                return Ok(
                    "Appointment completed successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult>
            DeleteAppointment(
                int id)
        {
            try
            {
                await _appointmentService
                    .DeleteAppointmentAsync(
                        id);

                return Ok(
                    "Appointment deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }
    }
}