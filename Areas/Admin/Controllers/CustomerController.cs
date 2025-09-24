using FastFood.DB.Entities;
using FastFood.Models;
using FastFood.Models.ViewModels;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace FastFood.Areas.Admin.Controllers
{
    [Route("admin/customers")]
    [Authorize(AuthenticationSchemes = "EmployeeScheme")]
    public class CustomerController : BaseEmployeeController
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var customers = await this._customerService.GetCustomersPagedList(page, size);
            ViewBag.Customers = customers;
            ViewBag.CurrentPage = customers.PageNumber;
            ViewBag.TotalPages = customers.PageCount;
            return View();
        }

        [HttpGet("potential/get")]
        public async Task<IActionResult> GetPotential([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var potentialCustomers = await this._customerService.GetPotentialCustomersPagedList(page, size);
            ViewBag.PotentialCustomers = potentialCustomers;
            ViewBag.CurrentPage = potentialCustomers.PageNumber;
            ViewBag.TotalPages = potentialCustomers.PageCount;
            return View();
        }

        [HttpGet("detail")]
        public async Task<IActionResult> Detail([FromQuery] int customerId, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            (CustomerDetailViewModel customerDetailViewModel, IPagedList<Order> orders) = await this._customerService.GetCustomerDetailWithOrdersPagedList(customerId, page, size);
            ViewBag.CustomerDetail = customerDetailViewModel;
            ViewBag.ReturnURL = GetAbsoluteUri();
            ViewBag.Orders = orders;
            return View();
        }
    }
}