using HealthCare_Appointment_Portal.Utilities;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthCare_Appointment_Portal_MVC.Controllers
{
    public class DashboardSelectionController : Controller
    {
        private readonly IPatientApiService _patientService;
        private readonly IDoctorApiService _doctorService;

        public DashboardSelectionController(
            IPatientApiService patientService,
            IDoctorApiService doctorService)
        {
            _patientService = patientService;
            _doctorService = doctorService;
        }

        public ActionResult Index()
        {
            return View();
        }

        #region Patient

        [HttpGet]
        public ActionResult Patient()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Patient(
            int patientId)
        {
            try
            {
                await _patientService
                    .GetPatientByIdAsync(
                        patientId);

                Session["Role"] =
                    "Patient";

                Session[
                    Constants.ReferenceIdKey] =
                    patientId;

                return RedirectToAction(
                    "Dashboard",
                    "Patient");
            }
            catch
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid Patient ID.");

                return View();
            }
        }

        #endregion

        #region Doctor

        [HttpGet]
        public ActionResult Doctor()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Doctor(
            int doctorId)
        {
            try
            {
                await _doctorService
                    .GetDoctorByIdAsync(
                        doctorId);

                Session["Role"] =
                    "Doctor";

                Session[
                    Constants.ReferenceIdKey] =
                    doctorId;

                return RedirectToAction(
                    "Dashboard",
                    "Doctor");
            }
            catch
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid Doctor ID.");

                return View();
            }
        }

        #endregion

        #region Admin

        public ActionResult Admin()
        {
            Session["Role"] =
                "Admin";

            return RedirectToAction(
                "Dashboard",
                "Admin");
        }

        #endregion

        #region Logout

        public ActionResult Logout()
        {
            Session.Clear();

            Session.Abandon();

            return RedirectToAction(
                "Index");
        }

        #endregion
    }
}