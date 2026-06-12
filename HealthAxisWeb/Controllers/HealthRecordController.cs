using HealthAxis.Shared.Dtos;
using HealthAxis.Web.Services;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthAxis.Web.Controllers
{
    public class HealthRecordController : Controller
    {
        private readonly IHealthRecordApiService _service;

        public HealthRecordController(IHealthRecordApiService service)
        {
            _service = service;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Create(CreateHealthRecordDto dto)
        {
            var result = await _service.Create(dto);

            return Json(new
            {
                Success = result.Success,
                Message = result.Message
            });
        }
        [HttpGet]
        public async Task<JsonResult> GetByPatient(int id)
        {
            var records = await _service.GetByPatient(id);
            return Json(records, JsonRequestBehavior.AllowGet);
        }
    }
}