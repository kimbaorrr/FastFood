using Microsoft.AspNetCore.Mvc;

namespace FastFood.Areas.Controllers
{
    [Route("ve-chung-toi")]
    public class AboutController : SessionController
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.Title = "Về chúng tôi";
            return View();
        }
    }
}