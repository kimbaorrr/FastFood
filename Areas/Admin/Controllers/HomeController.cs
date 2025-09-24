using FastFood.DB;
using FastFood.Models;
using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using X.PagedList;
using X.PagedList.Extensions;

namespace FastFood.Areas.Admin.Controllers
{
    [Route("admin")]
    [Authorize(AuthenticationSchemes = "EmployeeScheme")]
    public class HomeController : BaseEmployeeController
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IInventoryInService _inventoryInService;
        private readonly ICustomerRepository _customerRepository;

        public HomeController(IOrderService orderService, ICustomerService customerService, IInventoryInService inventoryInService, ICustomerRepository customerRepository)
        {
            _orderService = orderService;
            _customerService = customerService;
            _inventoryInService = inventoryInService;
            _customerRepository = customerRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var orders = await this._orderService.GetOrdersPagedList(page, size);
            var totalOrdersToday = await this._orderService.CountOrderByDate(DateTime.Now, DateTime.Now.AddDays(-1));
            var totalRevenueToday = await this._orderService.GetRevenueByDateTime(DateTime.Now, DateTime.Now.AddDays(-1));
            var totalNewCustomersToday = await this._customerService.CountNewCustomers(DateTime.Now, DateTime.Now.AddDays(-1));
            var totalInInventoryToday = await this._inventoryInService.CountInventoryInByDateTime(DateTime.Now, DateTime.Now.AddDays(-1));

            var percentageChangeOrders = await this._orderService.CompareOrderCountByPercentage(DateTime.Now, DateTime.Now.AddDays(-1));
            var percentageChangeRevenue = await this._orderService.CompareRevenueByPercentage(DateTime.Now, DateTime.Now.AddDays(-1));
            var percentageChangeNewCustomers = await this._customerService.CompareCustomersByPercentage(DateTime.Now, DateTime.Now.AddDays(-1));
            var percentageChangeInInventory = 0;


            ViewBag.Orders = orders;
            ViewBag.TotalOrdersToday = totalOrdersToday;
            ViewBag.TotalRevenueToday = totalRevenueToday;
            ViewBag.TotalNewCustomersToday = totalNewCustomersToday;
            ViewBag.TotalInInventoryToday = totalInInventoryToday;

            ViewBag.PercentageChangeOrders = percentageChangeOrders;
            ViewBag.PercentageChangeRevenue = percentageChangeRevenue;
            ViewBag.PercentageChangeNewCustomers = percentageChangeNewCustomers;
            ViewBag.PercentageChangeInInventory = percentageChangeInInventory;

            ViewBag.CurrentPage = orders.PageNumber;
            ViewBag.TotalPages = orders.PageCount;

            return View();
        }
    }
}