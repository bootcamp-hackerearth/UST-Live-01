using HealthAxis.Api.Services;
using HealthAxis.Shared.Dtos;
using System.Runtime.Remoting.Messaging;
using System.Web.Http;

namespace HealthAxis.Api.Controllers
{
    [RoutePrefix("api/patient")]
    public class PatientController : ApiController
    {
        private readonly IPatientService _service;

        public PatientController(IPatientService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(_service.GetAllPatients());
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var patient = _service.GetById(id);

            if (patient == null)
                return NotFound();

            return Ok(patient);
        }

        [HttpPost]
        [Route("create")]
        public IHttpActionResult Create(CreatePatientDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _service.Create(dto);

         return Ok(result);
        }


        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Update(int id, PatientDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = _service.Update(id, dto);

            if (updated == null)
                return NotFound();

            return Ok(new ApiResponseDto
                {
                Success = true,
                Message = "Patient updated successfully"
        });
        }


        [HttpPut]
        [Route("{id}/deactivate")]
        public IHttpActionResult Deactivate(int id)
        {
            var success = _service.Deactivate(id);

            if (!success)
                return NotFound();

            return Ok("Patient deactivated successfully");
        }
    }
}