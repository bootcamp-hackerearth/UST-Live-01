using SharedClasses.Dtos;
using HealthcareApi.Services;
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
            var appointment = _service.GetAppointmentById(id);

            return Ok(appointment);
        }

        [HttpGet]
        [Route("patient/{patientId:int}")]
        public IHttpActionResult GetByPatient(int patientId)
        {
            var appointments = _service.GetAppointmentsByPatient(patientId);

            return Ok(appointments);
        }

        [HttpGet]
        [Route("doctor/{doctorId:int}")]
        public IHttpActionResult GetByDoctor(int doctorId)
        {
            var appointments = _service.GetAppointmentsByDoctor(doctorId);

            return Ok(appointments);
        }

        [HttpGet]
        [Route("patient/{patientId:int}/upcoming")]
        public IHttpActionResult GetUpcomingByPatient(int patientId)
        {
            var appointments = _service.GetUpcomingAppointmentsByPatient(patientId);

            return Ok(appointments);
        }

        [HttpGet]
        [Route("doctor/{doctorId:int}/upcoming")]
        public IHttpActionResult GetUpcomingByDoctor(int doctorId)
        {
            var appointments = _service.GetUpcomingAppointmentsByDoctor(doctorId);

            return Ok(appointments);
        }

        [HttpGet]
        [Route("patient/{patientId:int}/cancelled")]
        public IHttpActionResult GetCancelledByPatient(int patientId)
        {
            var appointments = _service.GetCancelledAppointmentsByPatient(patientId);

            return Ok(appointments);
        }

        [HttpGet]
        [Route("doctor/{doctorId:int}/cancelled")]
        public IHttpActionResult GetCancelledByDoctor(int doctorId)
        {
            var appointments = _service.GetCancelledAppointmentsByDoctor(doctorId);

            return Ok(appointments);
        }

        [HttpPost]
        [Route("book")]
        public IHttpActionResult BookAppointment([FromBody] BookAppointmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _service.BookAppointment(dto);

            return CreatedAtRoute(
                "DefaultApi",
                new { controller = "appointments", id = result.AppointmentId },
                result);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateAppointment(
            int id,
            [FromBody] UpdateAppointmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _service.UpdateAppointment(id, dto);

            return Ok(result);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteAppointment(int id)
        {
            var result = _service.DeleteAppointment(id);

            return Ok(result);
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

            var result = _service.ConfirmAppointment(id, dto);

            return Ok(result);
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

            var result = _service.CancelAppointmentByPatient(id, dto);

            return Ok(result);
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

            var result = _service.CancelAppointmentByDoctor(id, dto);

            return Ok(result);
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

            var result = _service.CompleteAppointment(id, dto);

            return Ok(result);
        }
    }
}