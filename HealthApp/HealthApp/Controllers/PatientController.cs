using HealthApp.Service.Interface;
using HealthApp.Shared.DTOs;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthApp.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientApiService _service;

        public PatientController(IPatientApiService service)
        {
            _service = service;
        }

        public async Task<ActionResult> PatientIndex()
        {
            var patients = await _service.GetAll();
            return View(patients);
        }

        public async Task<ActionResult> GetById(int id)
        {
            var patient = await _service.GetById(id);
            return View(patient);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(PatientDto dto)
        {
            await _service.Create(dto);
            return RedirectToAction("PatientIndex");
        }
        public async Task<ActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
                return RedirectToAction("PatientIndex");

            var patients = await _service.GetAll();

            if (int.TryParse(query, out int id))
            {
                var result = patients.Find(p => p.PatientId == id);
                return View("GetById", result);
            }

            var filtered = patients
                .FindAll(p => p.FullName.ToLower().Contains(query.ToLower()));

            return View("PatientIndex", filtered);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var patient = await _service.GetById(id);
            return View(patient);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, PatientDto dto)
        {
            await _service.Update(id, dto);
            return RedirectToAction("PatientIndex");
        }
    }
}