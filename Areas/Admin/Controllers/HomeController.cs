using FastFood.DB;
using FastFood.Models;
using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using X.PagedList.Extensions;

namespace FastFood.Areas.Admin.Controllers
{
    [Route("admin")]
    public class HomeController : BaseController
    {
        private readonly IOrderService _orderService;

        public HomeController(IOrderRepository orderRepository, IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var orders = await this._orderService.GetOrdersPagedList(page, size);
            ViewBag.Orders = orders;
            ViewBag.CurrentPage = orders.PageNumber;
            ViewBag.TotalPages = orders.PageCount;
            return View();
        }

        




    }
}