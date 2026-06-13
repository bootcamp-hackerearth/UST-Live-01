using AutoMapper;
using HealthCare.Shared;
using HealthCare.Shared.DTOs.HealthRecord;
using HealthCareApi.Services.Interfaces;
using HealthCareWebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace HealthCareApi.Controllers
{
    [RoutePrefix("api/healthrecords")]
    public class HealthRecordController : ApiController
    {
        private readonly IHealthRecordService _healthRecordService;
        private readonly IMapper _mapper;

        public HealthRecordController(
            IHealthRecordService healthRecordService,
            IMapper mapper)
        {
            _healthRecordService = healthRecordService;
            _mapper = mapper;
        }

        // 1. ADD HEALTH RECORD
        // POST: api/healthrecords
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Add(HealthRecord record)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _healthRecordService
                    .AddHealthRecordAsync(record);

                return Ok(created);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        // 2. GET PATIENT HEALTH HISTORY (VIEW + Pagination)
        // GET: api/healthrecords/patient/5?pageNumber=1&pageSize=5
        [HttpGet]
        [Route("patient/{patientId:int}")]
        public async Task<IHttpActionResult> GetPatientHistory(
            int patientId,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var result = await _healthRecordService
                .GetPatientHealthHistoryAsync(patientId, pageNumber, pageSize);

            //  map ONLY Items
            var dtos = _mapper.Map<IEnumerable<HealthRecordDto>>(result.Items);

            //  return paged result
            return Ok(new PagedResult<HealthRecordDto>
            {
                Items = dtos.ToList(),
                TotalCount = result.TotalCount,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            });
        }

        //  3. GET HEALTH RECORD BY ID
        // GET: api/healthrecords/10
        [HttpGet]
        [Route("appointment/{id:int}")]
        public async Task<IHttpActionResult> GetByAppointmentId(int id)
        {
            try
            {
                var record = await _healthRecordService.GetByAppointmentIdAsync(id);

                if (record == null)
                    return NotFound();

                var dto = _mapper.Map<HealthRecordDto>(record);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("record/{id:int}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            try
            {
                var record = await _healthRecordService.GetByIdAsync(id);

                if (record == null)
                    return NotFound();

                var dto = _mapper.Map<HealthRecordDto>(record);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}