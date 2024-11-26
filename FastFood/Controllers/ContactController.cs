using System.Web.Mvc;

namespace FastFood.Controllers
{
    [RoutePrefix("lien-he")]
    public class ContactController : SessionController
    {
        [HttpGet]
        [Route("")]
        public ActionResult Index()
        {
            ViewBag.Title = "Liên hệ FastFood";
            return View();
        }
    }
}