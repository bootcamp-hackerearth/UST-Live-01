using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthAppWebAPI.Controllers
{
    using HealthAppWebAPI.Models.Dtos;
    using HealthAppWebAPI.Services.Interfaces;
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;

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
        public async Task<IHttpActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            try
            {
                var record = await _service.GetByIdAsync(id);
                return Ok(record);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Add(CreateHealthRecordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.AddAsync(dto);

            return Ok("Health record added successfully.");
        }

        [HttpGet]
        [Route("healthrecords/patient/{patientId:int}")]
        public async Task<IHttpActionResult> GetByPatient(int patientId)
        {
            var result = await _service.GetByPatientIdAsync(patientId);

            return Ok(result);
        }

        
    }

}