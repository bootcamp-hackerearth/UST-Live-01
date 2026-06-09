using SharedClasses.Dtos;
using SharedClasses.Enums;
using HealthcareApi.Services;
using System.Web.Http;

namespace HealthcareApi.Controllers
{
    [RoutePrefix("api/doctors")]
    public class DoctorsController : ApiController
    {
        private readonly IDoctorService _service;

        public DoctorsController(IDoctorService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            var doctors = _service.GetAllDoctors();

            return Ok(doctors);
        }

        [HttpGet]
        [Route("active")]
        public IHttpActionResult GetAllActive()
        {
            var doctors = _service.GetAllActiveDoctors();

            return Ok(doctors);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var doctor = _service.GetDoctorById(id);

            return Ok(doctor);
        }

        [HttpGet]
        [Route("specialisation/{specialisation}")]
        public IHttpActionResult SearchBySpecialisation(Specialisation specialisation)
        {
            var doctors = _service.SearchDoctorsBySpecialisation(specialisation);

            return Ok(doctors);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult AddDoctor([FromBody] CreateDoctorDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _service.AddDoctor(dto);

            return CreatedAtRoute(
                "DefaultApi",
                new { controller = "doctors", id = result.DoctorId },
                result);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateDoctor(
            int id,
            [FromBody] UpdateDoctorDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _service.UpdateDoctor(id, dto);

            return Ok(result);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeactivateDoctor(int id)
        {
            var result = _service.DeactivateDoctor(id);

            return Ok(result);
        }

        [HttpPost]
        [Route("{id:int}/reactivate")]
        public IHttpActionResult ReactivateDoctor(int id)
        {
            var result = _service.ReactivateDoctor(id);

            return Ok(result);
        }
    }
}