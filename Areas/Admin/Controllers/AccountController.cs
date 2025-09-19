using FastFood.DB;
using FastFood.Models.ViewModels;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FastFood.Areas.Admin.Controllers
{
    [Route("admin/auth")]
    public class AccountController : BaseController
    {
        private readonly IEmployeeService _employeeService;
        public AccountController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            ViewBag.Title = "Đăng nhập hệ thống";
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(EmployeeLoginViewModel employeeLoginViewModel)
        {
            if (ModelState.IsValid)
            {
                (bool isValid, Employee? employee) = await this._employeeService.LoginChecker(employeeLoginViewModel);

                if (isValid)
                {
                    await this.AddEmployeeToClaim(
                        employee!.EmployeeId.ToString(),
                        employee,
                        employeeLoginViewModel.RememberMe
                    );
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                ViewBag.AlertMessage = "Thông tin đăng nhập không hợp lệ !";
            }
            return View();

        }

        [HttpPost("logout")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Logout()
        {
            await this.ClearClaims();
            return RedirectToAction("Login", "Account", new { area = "Admin" });
        }

        [HttpPost("change-password")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ChangePassword(EmployeeChangePasswordViewModel employeeChangePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var employee = this.GetEmployeeFromClaim();
                employeeChangePasswordViewModel.EmployeeId = employee!.EmployeeId;
                (bool success, string message) = await this._employeeService.ChangePassword(employeeChangePasswordViewModel);
                return CreateJsonResult(success, message);
            }

            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return CreateJsonResult(false, string.Join(" | ", errors));
        }

        [HttpPost("forgot-password")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ForgotPassword(EmployeeForgotPasswordViewModel employeeForgotPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                (bool success, string message) = await this._employeeService.ForgotPassword(employeeForgotPasswordViewModel);

                return CreateJsonResult(success, message);
            }
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return CreateJsonResult(false, string.Join(" | ", errors));

        }
    }
}
