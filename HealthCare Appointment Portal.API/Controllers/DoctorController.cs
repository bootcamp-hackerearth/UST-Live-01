using HealthCare_Appointment_Portal.DTOs.DoctorDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Interfaces;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace HealthCare_Appointment_Portal.Controllers
{
    [RoutePrefix("api/doctors")]
    public class DoctorController
        : ApiController
    {
        private readonly IDoctorService
            _doctorService;

        public DoctorController(
            IDoctorService doctorService)
        {
            _doctorService =
                doctorService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult>
            GetAllDoctors()
        {
            var doctors =
                await _doctorService
                    .GetAllDoctorsAsync();

            return Ok(doctors);
        }

        [HttpGet]
        [Route("{id:int}",
            Name = "GetDoctorById")]
        public async Task<IHttpActionResult>
            GetDoctorById(
                int id)
        {
            var doctor =
                await _doctorService
                    .GetDoctorByIdAsync(
                        id);

            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);
        }

        [HttpGet]
        [Route("specialisation/{specialisation}")]
        public async Task<IHttpActionResult>
            GetDoctorsBySpecialisation(
                Specialisation specialisation)
        {
            var doctors =
                await _doctorService
                    .GetDoctorsBySpecialisationAsync(
                        specialisation);

            return Ok(doctors);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult>
            AddDoctor(
                [FromBody]
                CreateDoctorDto doctorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ModelState);
            }

            try
            {
                int doctorId =
                    await _doctorService
                        .AddDoctorAsync(
                            doctorDto);

                var createdDoctor =
                    await _doctorService
                        .GetDoctorByIdAsync(
                            doctorId);

                return CreatedAtRoute(
                    "GetDoctorById",
                    new
                    {
                        id = doctorId
                    },
                    createdDoctor);
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
            UpdateDoctor(
                int id,
                [FromBody]
                UpdateDoctorDto doctorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ModelState);
            }

            try
            {
                await _doctorService
                    .UpdateDoctorAsync(
                        id,
                        doctorDto);

                var updatedDoctor =
                    await _doctorService
                        .GetDoctorByIdAsync(
                            id);

                return Ok(
                    updatedDoctor);
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
            DeleteDoctor(
                int id)
        {
            try
            {
                await _doctorService
                    .DeleteDoctorAsync(
                        id);

                return Ok(
                    new
                    {
                        Message =
                            "Doctor deleted successfully."
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }
    }
}