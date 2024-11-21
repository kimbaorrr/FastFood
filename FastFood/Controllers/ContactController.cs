using System.Web.Mvc;

namespace FastFood.Controllers
{
    public class ContactController : SessionController
    {
        // GET: Contact
        public ActionResult Index()
        {
            ViewBag.Title = "Liên hệ FastFood";
            return View();
        }
    }
}