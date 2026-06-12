using HealthApp.API.Service.Interface;
using HealthApp.Shared.DTOs;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace HealthApp.API.Controllers
{
    [RoutePrefix("api/healthrecords")]
    public class HealthRecordApiController : ApiController
    {
        private readonly IHealthRecordService _service;

        public HealthRecordApiController(IHealthRecordService service)
        {
            _service = service;
        }

        // ✅ GET ALL
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            var data = await _service.GetAllRecords();
            return Ok(data);
        }

        // ✅ GET BY PATIENT
        [HttpGet]
        [Route("patient/{id}")]
        public async Task<IHttpActionResult> GetByPatient(int id)
        {
            var data = await _service.GetPatientRecords(id);
            return Ok(data);
        }

        // ✅ FILTER
        [HttpGet]
        [Route("filter")]
        public async Task<IHttpActionResult> GetByDoctorAndPatient(int doctorId, int patientId)
        {
            var data = await _service.GetHealthRecordsByDoctor(doctorId, patientId);
            return Ok(data);
        }

        // ✅ CREATE
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Create(HealthRecordDto dto)
        {
            try
            {
                await _service.AddRecord(dto);
                return Ok("Created Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}