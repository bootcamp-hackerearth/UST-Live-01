using SharedClasses.Dtos;
using HealthcareApi.Services;
using System.Web.Http;

namespace HealthcareApi.Controllers
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
        public IHttpActionResult GetAll()
        {
            var patients = _service.GetAllPatients();

            return Ok(patients);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var patient = _service.GetPatientById(id);

            return Ok(patient);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult AddPatient([FromBody] CreatePatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _service.RegisterPatient(dto);

            return CreatedAtRoute(
                "DefaultApi",
                new { controller = "patients", id = result.PatientId },
                result);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdatePatient(
            int id,
            [FromBody] UpdatePatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _service.UpdatePatient(id, dto);

            return Ok(result);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeletePatient(int id)
        {
            var result = _service.DeletePatient(id);

            return Ok(result);
        }
    }
}
