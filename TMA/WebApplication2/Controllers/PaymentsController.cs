using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    public class PaymentsController : Controller
    {
        public ActionResult PaymentsToMe()
        {
            return View();
        }

        public ActionResult PaymentsToOthers()
        {
            return View();
        }

        public ActionResult MyPayments()
        {
            return View();
        }
    }
}