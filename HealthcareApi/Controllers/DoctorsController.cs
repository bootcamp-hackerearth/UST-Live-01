using HealthcareApi.Dtos;
using HealthcareApi.Enums;
using HealthcareApi.Exceptions;
using HealthcareApi.Services;
using System.Net;
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
            try
            {
                var doctor = _service.GetDoctorById(id);

                return Ok(doctor);
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
        [Route("specialisation/{specialisation}")]
        public IHttpActionResult SearchBySpecialisation(Specialisation specialisation)
        {
            try
            {
                var doctors = _service.SearchDoctorsBySpecialisation(specialisation);

                return Ok(doctors);
            }
            catch (HealthcareAppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult AddDoctor([FromBody] CreateDoctorDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _service.AddDoctor(dto);

                return CreatedAtRoute(
                    "DefaultApi",
                    new { controller = "doctors", id = result.DoctorId },
                    result);
            }
            catch (HealthcareAppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateDoctor(int id, [FromBody] UpdateDoctorDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _service.UpdateDoctor(id, dto);

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
        public IHttpActionResult DeactivateDoctor(int id)
        {
            try
            {
                var result = _service.DeactivateDoctor(id);

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
        [Route("{id:int}/reactivate")]
        public IHttpActionResult ReactivateDoctor(int id)
        {
            try
            {
                var result = _service.ReactivateDoctor(id);

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