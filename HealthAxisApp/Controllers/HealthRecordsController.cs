using HealthAxisApp.Services;
using HealthAxisApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HealthAxisApp.Controllers
{
    [RoutePrefix("api/health-records")]
    public class HealthRecordsController : ApiController
    {
        private readonly IHealthRecordService _service;

        public HealthRecordsController(IHealthRecordService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{patientId:int}")]
        public IHttpActionResult GetByPatient(int patientId)
        {
            var records = _service.GetByPatient(patientId);

            return Ok(records);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(HealthRecordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string errorMessage;

            bool result = _service.Create(dto, out errorMessage);

            if (!result)
            {
                return BadRequest(errorMessage);
            }

            return Ok("Health record created.");
        }
    }
}
