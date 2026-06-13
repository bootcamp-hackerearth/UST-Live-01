using System.Web.Mvc;

namespace HealthcareWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Healthcare appointment and patient care management platform.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact the healthcare management support team.";

            return View();
        }
    }
}