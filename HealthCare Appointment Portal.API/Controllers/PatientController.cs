using HealthCare_Appointment_Portal.DTOs.PatientDtos;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Services.Interfaces;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace HealthCare_Appointment_Portal.Controllers
{
    [RoutePrefix("api/patients")]
    public class PatientController : ApiController
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAllPatients(string searchTerm = null)
        {
            var patients = await _patientService.GetAllPatientsAsync(searchTerm);
            return Ok(patients);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetPatientById")]
        public async Task<IHttpActionResult> GetPatientById(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

        [HttpGet]
        [Route("email")]
        public async Task<IHttpActionResult> GetPatientByEmail(string email)
        {
            var patient = await _patientService.GetPatientByEmailAsync(email);

            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> AddPatient([FromBody] CreatePatientDto patientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                int patientId = await _patientService.AddPatientAsync(patientDto);
                var createdPatient = await _patientService.GetPatientByIdAsync(patientId);

                return CreatedAtRoute("GetPatientById", new { id = patientId }, createdPatient);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> UpdatePatient(int id, [FromBody] UpdatePatientDto patientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _patientService.UpdatePatientAsync(id, patientDto);
                var updatedPatient = await _patientService.GetPatientByIdAsync(id);

                return Ok(updatedPatient);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> DeletePatient(int id)
        {
            try
            {
                await _patientService.DeletePatientAsync(id);
                return Ok(new { Message = "Patient deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}