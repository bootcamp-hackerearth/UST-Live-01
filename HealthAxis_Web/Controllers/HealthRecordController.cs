using AutoMapper;
using HealthAxis.Api.Database;
using HealthAxis.Api.Models;
using HealthAxis.Api.Services;
using HealthAxis.Shared.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HealthAxis.Api.Controllers
{
    [RoutePrefix("api/healthrecord")]
    public class HealthRecordController : ApiController
    {
        private readonly IHealthRecordService _service;

        public HealthRecordController(IHealthRecordService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(CreateHealthRecordDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid request");

            var result = _service.Create(dto);

            if (!result.Success)
                return Content(System.Net.HttpStatusCode.BadRequest, result);

            return Ok(result);
        }
        [HttpGet]
        [Route("patient/{id}")]
        public IHttpActionResult GetByPatient(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid patient id");

            var result = _service.GetByPatient(id);

            return Ok(result);
        }

    }
}