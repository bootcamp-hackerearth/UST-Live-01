using SharedClasses.Dtos;
using HealthcareApi.Services;
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
            var record = _service.GetRecordById(id);

            return Ok(record);
        }

        [HttpGet]
        [Route("patient/{patientId:int}")]
        public IHttpActionResult GetByPatient(int patientId)
        {
            var records = _service.GetRecordsByPatient(patientId);

            return Ok(records);
        }

        [HttpGet]
        [Route("doctor/{doctorId:int}")]
        public IHttpActionResult GetByDoctor(int doctorId)
        {
            var records = _service.GetRecordsByDoctor(doctorId);

            return Ok(records);
        }

        [HttpGet]
        [Route("appointment/{appointmentId:int}")]
        public IHttpActionResult GetByAppointment(int appointmentId)
        {
            var records = _service.GetRecordsByAppointment(appointmentId);

            return Ok(records);
        }
        
        [HttpGet]
        [Route("search")]
        public IHttpActionResult Search([FromUri] string query = "")
        {
            var records = _service.SearchHealthRecords(query);

            return Ok(records);
        }
       
        [HttpGet]
        [Route("patient/{patientId:int}/search")]
        public IHttpActionResult SearchByPatient(
            int patientId,
            [FromUri] string query = "")
        {
            var records =
                _service.SearchHealthRecordsByPatient(patientId, query);

            return Ok(records);
        }
        
        [HttpGet]
        [Route("doctor/{doctorId:int}/search")]
        public IHttpActionResult SearchByDoctor(
            int doctorId,
            [FromUri] string query = "")
        {
            var records =
                _service.SearchHealthRecordsByDoctor(doctorId, query);

            return Ok(records);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult AddHealthRecord([FromBody] AddHealthRecordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _service.AddRecord(dto);

            return CreatedAtRoute(
                "DefaultApi",
                new { controller = "healthrecords", id = result.HealthRecordId },
                result);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateHealthRecord(
            int id,
            [FromBody] UpdateHealthRecordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _service.UpdateRecord(id, dto);

            return Ok(result);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteHealthRecord(int id)
        {
            var result = _service.DeleteRecord(id);

            return Ok(result);
        }
    }
}