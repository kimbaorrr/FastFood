using FastFood.DB;
using FastFood.DB.Entities;
using FastFood.Models;
using FastFood.Models.ViewModels;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FastFood.Controllers
{
    /// <summary>
    /// Controller xử lý các chức năng tài khoản khách hàng.
    /// </summary>
    [Route("account")]
    public class AccountController : BaseCustomerController
    {
        private readonly ICustomerService _customerService;

        /// <summary>
        /// Khởi tạo controller tài khoản với dịch vụ khách hàng.
        /// </summary>
        /// <param name="customerService">Dịch vụ khách hàng.</param>
        public AccountController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Đăng nhập khách hàng.
        /// </summary>
        /// <param name="customerLoginViewModel">Thông tin đăng nhập khách hàng.</param>
        /// <returns>Kết quả đăng nhập dưới dạng JSON.</returns>
        [HttpPost("login")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(CustomerLoginViewModel customerLoginViewModel)
        {
            if (!ModelState.IsValid)
                return CreateJsonResult(false, "Dữ liệu không hợp lệ");

            (bool success, string message, CustomerClaimInfoViewModel? customerClaimInfoViewModel)
                = await this._customerService.LoginChecker(customerLoginViewModel);

            if (success)
            {
                await this.AddCustomerToClaim(
                    customerClaimInfoViewModel!.CustomerId.ToString(),
                    customerClaimInfoViewModel,
                    rememberMe: true
                );

                return CreateJsonResult(true, "Đăng nhập thành công !");
            }

            return CreateJsonResult(false, message);
        }

        /// <summary>
        /// Đăng xuất khách hàng.
        /// </summary>
        /// <returns>Chuyển hướng về trang chủ.</returns>
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await this.ClearClaims();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Đăng ký tài khoản khách hàng mới.
        /// </summary>
        /// <param name="customerRegisterViewModel">Thông tin đăng ký khách hàng.</param>
        /// <returns>Kết quả đăng ký dưới dạng JSON.</returns>
        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(CustomerRegisterViewModel customerRegisterViewModel)
        {
            (bool success, string message) = await this._customerService.Register(customerRegisterViewModel);
            return CreateJsonResult(success, message);
        }

    }
}