using FastFood.DB;
using FastFood.Models;
using FastFood.Models.ViewModels;
using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics.Contracts;
using X.PagedList;
using X.PagedList.Extensions;

namespace FastFood.Areas.Admin.Controllers
{
    [Route("admin/employee")]
    [Authorize(AuthenticationSchemes = "EmployeeScheme")]
    public class EmployeeController : BaseEmployeeController
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeService _employeeService;
        private readonly IPermissionRepository _permissionRepository;

        public EmployeeController(IEmployeeRepository employeeRepository, IEmployeeService employeeService, IPermissionRepository permissionRepository)
        {
            _employeeRepository = employeeRepository;
            _employeeService = employeeService;
            _permissionRepository = permissionRepository;
        }

        [HttpGet("timesheet")]
        public async Task<IActionResult> TimeSheet()
        {
            return View();
        }

        [HttpGet("permission")]
        public async Task<IActionResult> Permission()
        {
            EmployeeRegisterLoginAccountViewModel employeeRegisterLoginAccountViewModel = new EmployeeRegisterLoginAccountViewModel();
            return View(employeeRegisterLoginAccountViewModel);
        }

        [HttpGet("permission/get")]
        public async Task<IActionResult> GetEmployeePermissions([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var employeesWithPermissions = await this._employeeService.GetEmployeesPermissionsPagedList(page, size);
            ViewBag.Employees = employeesWithPermissions;
            ViewBag.CurrentPage = employeesWithPermissions.PageNumber;
            ViewBag.TotalPages = employeesWithPermissions.PageCount;
            return View();
        }

        [HttpGet()]
        public async Task<IActionResult> GetPermissions()
        {
            var permissions = await this._permissionRepository.GetPermissions();
            var customPermissions = permissions.Select(x => new
            {
                Id = x.PermissionId,
                Name = x.Description
            });
            return CreateJsonResult(true, string.Empty, customPermissions);
        }

        [HttpGet("employee/account/get")]
        public async Task<IActionResult> GetEmployeesByAccountStatus([FromQuery] bool isHasAccount)
        {
            var employeesByAccount = await this._employeeRepository.GetEmployeesWithAccountStatus(isHasAccount);

            if (!isHasAccount)
            {
                var customEmployeesNonAccount = employeesByAccount.Select(x => new
                {
                    EmployeeId = x.EmployeeId!,
                    FullName = x.LastName + " " + x.FirstName
                });
                return CreateJsonResult(true, string.Empty, customEmployeesNonAccount);
            }

            var customEmployeesHasAccount = employeesByAccount.Select(x => new
            {
                EmployeeId = x.EmployeeId!,
                FullName = x.LastName + " " + x.FirstName,
                UserName = x.EmployeeAccount!.UserName!,
                EmployeeCreatedAt = x.CreatedAt,
                AccountCreatedAt = x.EmployeeAccount.CreatedAt,
                Permissions = this._permissionRepository.GetDescriptionByIds(
                    x.EmployeeAccount.Permission!
                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray()
                    ?? new int[] { }
                )
            });
            return CreateJsonResult(true, string.Empty, customEmployeesHasAccount);

        }

        [HttpPost("employee/register-account")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> RegisterLoginAccount([FromForm] EmployeeRegisterLoginAccountViewModel employeeRegisterLoginAccountViewModel, [FromForm] string selectedPermissions)
        {
            (bool success, string message) = await this._employeeService.RegisterLoginAccount(employeeRegisterLoginAccountViewModel, selectedPermissions);
            return CreateJsonResult(success, message);
        }
        [HttpGet("employee/all")]
        public async Task<IActionResult> GetEmployeesHasAccount()
        {
            var employeeAccounts = await this._employeeRepository.GetEmployeesWithAccountStatus(true);
            return CreateJsonResult(true, string.Empty, employeeAccounts);
        }

    }
}