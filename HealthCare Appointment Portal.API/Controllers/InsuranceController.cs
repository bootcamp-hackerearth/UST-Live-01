using HealthCare_Appointment_Portal.DTOs.InsuranceDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Interfaces;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace HealthCare_Appointment_Portal.Controllers
{
    [RoutePrefix("api/insurances")]
    public class InsuranceController
        : ApiController
    {
        private readonly IInsuranceService
            _insuranceService;

        public InsuranceController(
            IInsuranceService insuranceService)
        {
            _insuranceService =
                insuranceService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult>
            GetAllInsurances()
        {
            var insurances =
                await _insuranceService
                    .GetAllInsurancesAsync();

            return Ok(insurances);
        }

        [HttpGet]
        [Route("{id:int}",
            Name = "GetInsuranceById")]
        public async Task<IHttpActionResult>
            GetInsuranceById(
                int id)
        {
            var insurance =
                await _insuranceService
                    .GetInsuranceByIdAsync(
                        id);

            if (insurance == null)
            {
                return NotFound();
            }

            return Ok(insurance);
        }

        [HttpGet]
        [Route("patient/{patientId:int}")]
        public async Task<IHttpActionResult>
            GetInsurancesByPatient(
                int patientId)
        {
            var insurances =
                await _insuranceService
                    .GetInsurancesByPatientAsync(
                        patientId);

            return Ok(insurances);
        }

        [HttpGet]
        [Route("status/{status}")]
        public async Task<IHttpActionResult>
            GetInsurancesByStatus(
                InsuranceStatus status)
        {
            var insurances =
                await _insuranceService
                    .GetInsurancesByStatusAsync(
                        status);

            return Ok(insurances);
        }

        [HttpGet]
        [Route("active")]
        public async Task<IHttpActionResult>
            GetActiveInsurances()
        {
            var insurances =
                await _insuranceService
                    .GetActiveInsurancesAsync();

            return Ok(insurances);
        }

        [HttpGet]
        [Route("expired")]
        public async Task<IHttpActionResult>
            GetExpiredInsurances()
        {
            var insurances =
                await _insuranceService
                    .GetExpiredInsurancesAsync();

            return Ok(insurances);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult>
            AddInsurance(
                [FromBody]
                CreateInsuranceDto insuranceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ModelState);
            }

            try
            {
                int insuranceId =
                    await _insuranceService
                        .AddInsuranceAsync(
                            insuranceDto);

                return CreatedAtRoute(
                    "GetInsuranceById",
                    new
                    {
                        id = insuranceId
                    },
                    new
                    {
                        InsuranceId =
                            insuranceId
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
            UpdateInsurance(
                int id,
                [FromBody]
                UpdateInsuranceDto insuranceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    ModelState);
            }

            try
            {
                await _insuranceService
                    .UpdateInsuranceAsync(
                        id,
                        insuranceDto);

                return Ok(
                    "Insurance updated successfully.");
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
            DeleteInsurance(
                int id)
        {
            try
            {
                await _insuranceService
                    .DeleteInsuranceAsync(
                        id);

                return Ok(
                    "Insurance deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }
    }
}