using FastFood.DB.Entities;
using FastFood.Models.ViewModels;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FastFood.Areas.Admin.Controllers
{
    [Route("admin/auth")]
    public class AccountController : BaseEmployeeController
    {
        private readonly IEmployeeService _employeeService;
        public AccountController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            ViewBag.Title = "Đăng nhập hệ thống";
            return View();
        }

        [HttpPost("login")]
        [AutoValidateAntiforgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(EmployeeLoginViewModel employeeLoginViewModel)
        {
            if (ModelState.IsValid)
            {
                (bool success, string message, EmployeeClaimInfoViewModel? employeeClaimInfo) = await this._employeeService.LoginChecker(employeeLoginViewModel);

                if (success)
                {
                    await this.AddEmployeeToClaim(
                        employeeClaimInfo!.EmployeeId.ToString(),
                        employeeClaimInfo,
                        employeeLoginViewModel.RememberMe
                    );
                }
                return CreateJsonResult(success, message);
            }
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return CreateJsonResult(false, string.Join(" | ", errors));

        }

        [HttpGet("logout")]
        [Authorize(AuthenticationSchemes = "EmployeeScheme")]
        public async Task<IActionResult> Logout()
        {
            await this.ClearClaims();
            return RedirectToAction("Login", "Account", new { area = "Admin" });
        }

        [HttpPost("change-password")]
        [AutoValidateAntiforgeryToken]
        [Authorize(AuthenticationSchemes = "EmployeeScheme")]
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
        [AllowAnonymous]
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
