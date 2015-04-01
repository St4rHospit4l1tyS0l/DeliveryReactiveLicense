using System.Web.Mvc;

namespace DeliveryReactiveLicensing.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}