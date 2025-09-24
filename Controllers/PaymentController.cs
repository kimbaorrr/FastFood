using System.Collections.Specialized;
using System.Net;
using System.Web;
using FastFood.DB;
using FastFood.Models;
using FastFood.Models.ViewModels;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace FastFood.Controllers
{
    /// <summary>
    /// Controller xử lý các chức năng liên quan đến thanh toán cho khách hàng.
    /// </summary>
    [Route("payment")]
    public class PaymentController : BaseCustomerController
    {
        private readonly IPaymentService _paymentService;
        private readonly ICartService _cartService;

        /// <summary>
        /// Khởi tạo controller thanh toán với các dịch vụ cần thiết.
        /// </summary>
        /// <param name="paymentService">Dịch vụ thanh toán.</param>
        /// <param name="cartService">Dịch vụ giỏ hàng.</param>
        public PaymentController(IPaymentService paymentService, ICartService cartService)
        {
            _paymentService = paymentService;
            _cartService = cartService;
        }

        /// <summary>
        /// Hiển thị trang thanh toán cho khách hàng.
        /// </summary>
        /// <returns>Trang View thanh toán với thông tin cần thiết.</returns>
        [HttpGet("checkout")]
        public async Task<IActionResult> Checkout()
        {
            var addPaymentViewModel = await this._paymentService.AddPaymentViewModel(
                this.GetPaymentSummaryFromSession(),
                this._customerId
            );
            ViewBag.Title = "Thanh toán";
            return View(addPaymentViewModel);
        }

        /// <summary>
        /// Xử lý khi khách hàng gửi thông tin thanh toán.
        /// </summary>
        /// <param name="addPaymentViewModel">Model thông tin thanh toán.</param>
        /// <returns>Chuyển hướng đến URL thanh toán.</returns>
        [HttpPost("")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index(AddPaymentViewModel addPaymentViewModel)
        {
            string paymentUrl = await this._paymentService.AddPaymentViewPostModel(
                addPaymentViewModel,
                this._cartService.CustomerCartViewModel,
                this._customerId,
                HttpContext.Connection.RemoteIpAddress?.ToString() ?? IPAddress.Loopback.ToString(),
                Url.Action("Result", "Payment") ?? "about:blank"
            );
            return Redirect(paymentUrl);
        }

        /// <summary>
        /// Hiển thị kết quả thanh toán sau khi khách hàng hoàn tất giao dịch.
        /// </summary>
        /// <returns>Trang View kết quả thanh toán.</returns>
        [HttpGet("result")]
        public async Task<IActionResult> Result()
        {
            ViewBag.Title = "Kết quả thanh toán";

            (bool success, string message, PaymentResultViewModel paymentResultViewModel) =
                await this._paymentService.GetPaymentResultViewModel(Request.Query);

            return View(paymentResultViewModel);
        }
    }
}