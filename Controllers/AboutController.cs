using Microsoft.AspNetCore.Mvc;

namespace FastFood.Areas.Controllers
{
    [Route("about")]
    public class AboutController : BaseController
    {
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Về chúng tôi";
            return View();
        }
    }
}