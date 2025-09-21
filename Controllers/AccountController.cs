using FastFood.DB;
using FastFood.DB.Entities;
using FastFood.Models;
using FastFood.Models.ViewModels;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FastFood.Areas.Controllers
{
    [Route("account")]
    public class AccountController : BaseController
    {
        private readonly ICustomerService _customerService;

        public AccountController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("login")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(CustomerLoginViewModel customerLoginViewModel)
        {
            if (ModelState.IsValid)
            {
                (bool isValid, Customer? customer) = await this._customerService.LoginChecker(customerLoginViewModel);

                if (isValid)
                {
                    await this.AddCustomerToClaim(
                        customer!.CustomerId.ToString(),
                        customer,
                        true
                    );
                    return CreateJsonResult(true, "Đăng nhập thành công !");
                }
                
            }
            return CreateJsonResult(false, "Thông tin đăng nhập không hợp lệ !");
        }

        [HttpPost("logout")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Logout()
        {
            await this.ClearClaims();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(CustomerRegisterViewModel customerRegisterViewModel)
        {
            (bool success, string message) = await this._customerService.Register(customerRegisterViewModel);
            return CreateJsonResult(success, message);
        }

    }
}