using HealthCare_Appointment_Portal.DTOs.HealthRecordDtos;
using HealthCare_Appointment_Portal.Interfaces;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace HealthCare_Appointment_Portal.Controllers
{
    [RoutePrefix("api/health-records")]
    public class HealthRecordController
        : ApiController
    {
        private readonly IHealthRecordService
            _healthRecordService;

        public HealthRecordController(
            IHealthRecordService
                healthRecordService)
        {
            _healthRecordService =
                healthRecordService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult>
            GetAllHealthRecords()
        {
            var records =
                await _healthRecordService
                    .GetAllHealthRecordsAsync();

            return Ok(records);
        }

        [HttpGet]
        [Route("{id:int}",
            Name = "GetHealthRecordById")]
        public async Task<IHttpActionResult>
            GetHealthRecordById(
                int id)
        {
            var record =
                await _healthRecordService
                    .GetHealthRecordByIdAsync(
                        id);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(record);
        }

        [HttpGet]
        [Route("patient/{patientId:int}")]
        public async Task<IHttpActionResult>
            GetRecordsByPatient(
                int patientId)
        {
            var records =
                await _healthRecordService
                    .GetRecordsByPatientAsync(
                        patientId);

            return Ok(records);
        }

        [HttpGet]
        [Route("doctor/{doctorId:int}")]
        public async Task<IHttpActionResult>
            GetRecordsByDoctor(
                int doctorId)
        {
            var records =
                await _healthRecordService
                    .GetRecordsByDoctorAsync(
                        doctorId);

            return Ok(records);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult>
            AddHealthRecord(
                [FromBody]
                CreateHealthRecordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ModelState);
            }

            try
            {
                int recordId =
                    await _healthRecordService
                        .AddHealthRecordAsync(
                            dto);

                return CreatedAtRoute(
                    "GetHealthRecordById",
                    new
                    {
                        id = recordId
                    },
                    new
                    {
                        RecordId = recordId
                    });
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
            UpdateHealthRecord(
                int id,
                [FromBody]
                UpdateHealthRecordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ModelState);
            }

            try
            {
                await _healthRecordService
                    .UpdateHealthRecordAsync(
                        id,
                        dto);

                return Ok(
                    "Health record updated successfully.");
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
            DeleteHealthRecord(
                int id)
        {
            try
            {
                await _healthRecordService
                    .DeleteHealthRecordAsync(
                        id);

                return Ok(
                    "Health record deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }
    }
}