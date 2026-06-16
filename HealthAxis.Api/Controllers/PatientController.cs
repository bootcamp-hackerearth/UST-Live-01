//using HealthAxisCore_Api.Services.Interfaces;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace HealthAxisCore_Api.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class PatientController(IPatientService service) : ControllerBase
//    {
//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            var result = await service.GetAllAsync();
//            return Ok(result);
//        }
//        [HttpGet]
//        public async Task<IActionResult> GetById([FromRoute]int id)
//        {
//            var result = await service.GetByIdAsync(id);
//            if(result is null)
//            {
//                return NotFound();
//            }
//            else
//            {
//                return Ok(result);
//            }
//        }
        
//    }
//}
