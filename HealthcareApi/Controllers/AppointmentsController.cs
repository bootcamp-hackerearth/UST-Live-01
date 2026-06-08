using HealthcareApi.Dtos;
using HealthcareApi.Exceptions;
using HealthcareApi.Services;
using System.Net;
using System.Web.Http;

namespace HealthcareApi.Controllers
{
    [RoutePrefix("api/appointments")]
    public class AppointmentsController : ApiController
    {
        private readonly IAppointmentService _service;

        public AppointmentsController(IAppointmentService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            var appointments = _service.GetAllAppointments();

            return Ok(appointments);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var appointment = _service.GetAppointmentById(id);

                return Ok(appointment);
            }
            catch (EntityNotFoundException ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message);
            }
            catch (HealthcareAppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("patient/{patientId:int}")]
        public IHttpActionResult GetByPatient(int patientId)
        {
            try
            {
                var appointments = _service.GetAppointmentsByPatient(patientId);

                return Ok(appointments);
            }
            catch (EntityNotFoundException ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message);
            }
            catch (HealthcareAppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("doctor/{doctorId:int}")]
        public IHttpActionResult GetByDoctor(int doctorId)
        {
            try
            {
                var appointments = _service.GetAppointmentsByDoctor(doctorId);

                return Ok(appointments);
            }
            catch (EntityNotFoundException ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message);
            }
            catch (HealthcareAppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("patient/{patientId:int}/upcoming")]
        public IHttpActionResult GetUpcomingByPatient(int patientId)
        {
            try
            {
                var appointments = _service.GetUpcomingAppointmentsByPatient(patientId);

                return Ok(appointments);
            }
            catch (EntityNotFoundException ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message);
            }
            catch (HealthcareAppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("doctor/{doctorId:int}/upcoming")]
        public IHttpActionResult GetUpcomingByDoctor(int doctorId)
        {
            try
            {
                var appointments = _service.GetUpcomingAppointmentsByDoctor(doctorId);

                return Ok(appointments);
            }
            catch (EntityNotFoundException ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message);
            }
            catch (HealthcareAppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("patient/{patientId:int}/cancelled")]
        public IHttpActionResult GetCancelledByPatient(int patientId)
        {
            try
            {
                var appointments = _service.GetCancelledAppointmentsByPatient(patientId);

                return Ok(appointments);
            }
            catch (EntityNotFoundException ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message);
            }
            catch (HealthcareAppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("book")]
        public IHttpActionResult BookAppointment([FromBody] BookAppointmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _service.BookAppointment(dto);

                return CreatedAtRoute(
                    "DefaultApi",
                    new { controller = "appointments", id = result.AppointmentId },
                    result);
            }
            catch (HealthcareAppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateAppointment(int id, [FromBody] UpdateAppointmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _service.UpdateAppointment(id, dto);

                return Ok(result);
            }
            catch (EntityNotFoundException ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message);
            }
            catch (HealthcareAppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteAppointment(int id)
        {
            try
            {
                var result = _service.DeleteAppointment(id);

                return Ok(result);
            }
            catch (EntityNotFoundException ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message);
            }
            catch (HealthcareAppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("{id:int}/confirm")]
        public IHttpActionResult ConfirmAppointment(
            int id,
            [FromBody] ConfirmAppointmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _service.ConfirmAppointment(id, dto);

                return Ok(result);
            }
            catch (EntityNotFoundException ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message);
            }
            catch (HealthcareAppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("{id:int}/cancel-by-patient")]
        public IHttpActionResult CancelByPatient(
            int id,
            [FromBody] CancelByPatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _service.CancelAppointmentByPatient(id, dto);

                return Ok(result);
            }
            catch (EntityNotFoundException ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message);
            }
            catch (HealthcareAppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("{id:int}/cancel-by-doctor")]
        public IHttpActionResult CancelByDoctor(
            int id,
            [FromBody] CancelByDoctorDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _service.CancelAppointmentByDoctor(id, dto);

                return Ok(result);
            }
            catch (EntityNotFoundException ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message);
            }
            catch (HealthcareAppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("{id:int}/complete")]
        public IHttpActionResult CompleteAppointment(
            int id,
            [FromBody] CompleteAppointmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _service.CompleteAppointment(id, dto);

                return Ok(result);
            }
            catch (EntityNotFoundException ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message);
            }
            catch (HealthcareAppException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}