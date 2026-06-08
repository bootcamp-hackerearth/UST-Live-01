using HealthcareApi.Dtos;
using HealthcareApi.Exceptions;
using HealthcareApi.Services;
using System.Net;
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
            try
            {
                var patient = _service.GetPatientById(id);

                return Ok(patient);
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
        public IHttpActionResult AddPatient([FromBody] CreatePatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _service.RegisterPatient(dto);

                return CreatedAtRoute(
                    "DefaultApi",
                    new { controller = "patients", id = result.PatientId },
                    result);
            }
            catch (HealthcareAppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdatePatient(int id, [FromBody] UpdatePatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _service.UpdatePatient(id, dto);

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
        public IHttpActionResult DeletePatient(int id)
        {
            try
            {
                var result = _service.DeletePatient(id);

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