using AutoMapper;
using HealthCare.Shared;
using HealthCare.Shared.DTOs.Patient;
using HealthCareApi.Services.Interfaces;
using HealthCareWebApi;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace HealthCareApi.Controllers
{
    [RoutePrefix("api/patients")]
    public class PatientController : ApiController
    {
        private readonly IPatientService _patientService;
        private readonly IMapper _mapper;

        public PatientController(IPatientService patientService, IMapper mapper)
        {
            _patientService = patientService;
            _mapper = mapper;
        }

        //  GET: api/patients?pageNumber=1&pageSize=10
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetPatients(
            string searchTerm = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var result = await _patientService.GetPaginatedPatientAsync(
                searchTerm,
                pageNumber,
                pageSize);

            //  map ONLY Items
            var dtos = _mapper.Map<IEnumerable<PatientDto>>(result.Items);

            //  wrap again
            return Ok(new PagedResult<PatientDto>
            {
                Items = dtos.ToList(),
                TotalCount = result.TotalCount,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            });
        }

        // GET: api/patients/5
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);

            if (patient == null)
                return NotFound();

            var dto = _mapper.Map<PatientDto>(patient);

            return Ok(dto);
        }

        // POST: api/patients
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Add(Patient patient)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdPatient = await _patientService.AddPatientAsync(patient);

            var dto = _mapper.Map<PatientDto>(createdPatient);

            return Created($"api/patients/{dto.PatientId}", dto);
        }

        // PUT: api/patients/5
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Update(int id, Patient patient)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedPatient = await _patientService.UpdatePatientAsync(patient);

            if (updatedPatient == null)
                return NotFound();

            var dto = _mapper.Map<PatientDto>(updatedPatient);

            return Ok(dto);
        }

        // DELETE (Soft delete): api/patients/5
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var result = await _patientService.DeletePatientAsync(id);

            if (!result)
                return NotFound();

            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }
    }
}