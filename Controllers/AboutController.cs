using Microsoft.AspNetCore.Mvc;

namespace FastFood.Controllers
{
    /// <summary>
    /// Controller xử lý các yêu cầu liên quan đến trang "Về chúng tôi".
    /// </summary>
    [Route("about")]
    public class AboutController : BaseCustomerController
    {
        /// <summary>
        /// Hiển thị trang "Về chúng tôi".
        /// </summary>
        /// <returns>Kết quả trả về trang View.</returns>
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Về chúng tôi";
            return View();
        }
    }
}