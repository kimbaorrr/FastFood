using System.Web.Mvc;

namespace FastFood.Controllers
{
    [RoutePrefix("ve-chung-toi")]
    public class AboutController : SessionController
    {
        [Route("")]
        public ActionResult Index()
        {
            ViewBag.Title = "Về chúng tôi";
            return View();
        }
    }
}