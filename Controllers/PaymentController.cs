using System.Collections.Specialized;
using System.Net;
using System.Web;
using FastFood.DB;
using FastFood.Models;
using FastFood.Models.ViewModels;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace FastFood.Areas.Controllers
{
    [Route("payment")]
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var addPaymentViewModel = await this._paymentService.AddPaymentViewModel(
                this.GetPaymentSummaryFromClaim(),
                this._customerId
            );
            ViewBag.Title = "Thanh toán";
            return View(addPaymentViewModel);
        }

        [HttpPost("")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(AddPaymentViewModel addPaymentViewModel)
        {
            string paymentUrl = await this._paymentService.AddPaymentViewPostModel(
                addPaymentViewModel,
                this._customerCartViewModel,
                this._customerId,
                HttpContext.Connection.RemoteIpAddress?.ToString() ?? IPAddress.Loopback.ToString()
            );
            return Redirect(paymentUrl);
        }

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