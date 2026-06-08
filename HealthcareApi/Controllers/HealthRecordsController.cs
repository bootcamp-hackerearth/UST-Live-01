using HealthcareApi.Dtos;
using HealthcareApi.Exceptions;
using HealthcareApi.Services;
using System.Net;
using System.Web.Http;

namespace HealthcareApi.Controllers
{
    [RoutePrefix("api/healthrecords")]
    public class HealthRecordsController : ApiController
    {
        private readonly IHealthRecordService _service;

        public HealthRecordsController(IHealthRecordService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            var records = _service.GetAllRecords();

            return Ok(records);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var record = _service.GetRecordById(id);

                return Ok(record);
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
                var records = _service.GetRecordsByPatient(patientId);

                return Ok(records);
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
                var records = _service.GetRecordsByDoctor(doctorId);

                return Ok(records);
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
        [Route("appointment/{appointmentId:int}")]
        public IHttpActionResult GetByAppointment(int appointmentId)
        {
            try
            {
                var records = _service.GetRecordsByAppointment(appointmentId);

                return Ok(records);
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
        [Route("")]
        public IHttpActionResult AddHealthRecord([FromBody] AddHealthRecordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _service.AddRecord(dto);

                return CreatedAtRoute(
                    "DefaultApi",
                    new { controller = "healthrecords", id = result.HealthRecordId },
                    result);
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

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateHealthRecord(int id, [FromBody] UpdateHealthRecordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _service.UpdateRecord(id, dto);

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
        public IHttpActionResult DeleteHealthRecord(int id)
        {
            try
            {
                var result = _service.DeleteRecord(id);

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