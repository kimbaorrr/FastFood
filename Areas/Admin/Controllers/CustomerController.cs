using FastFood.DB.Entities;
using FastFood.Models;
using FastFood.Models.ViewModels;
using FastFood.Repositories.Interfaces;
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
        private readonly IOrderRepository _orderRepository;

        public CustomerController(ICustomerService customerService, IOrderRepository orderRepository)
        {
            _customerService = customerService;
            _orderRepository = orderRepository;
        }

        [HttpGet("get")]
        public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var customers = await this._customerService.GetCustomersPagedList(page, size);
            ViewBag.Customers = customers;
            ViewBag.CurrentPage = customers.PageNumber;
            ViewBag.TotalPages = customers.PageCount;
            return View();
        }

        [HttpGet("potential/get")]
        public async Task<IActionResult> Potential([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var potentialCustomers = await this._customerService.GetPotentialCustomersPagedList(page, size);
            var customerOrderCounts = 0;
            var totalOrders = await this._orderRepository.CountOrders();
            var totalRevenueOrders = await this._orderRepository.GetTotalOrdersRevenue();
            var averageRevenueOrders = await this._orderRepository.GetAverageOrdersRevenue();

            ViewBag.PotentialCustomers = potentialCustomers;
            ViewBag.CustomerOrderCounts = customerOrderCounts;
            ViewBag.TotalOrders = totalOrders;
            ViewBag.AverageRevenueOrders = averageRevenueOrders;
            ViewBag.TotalRevenueOrders = totalRevenueOrders;

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