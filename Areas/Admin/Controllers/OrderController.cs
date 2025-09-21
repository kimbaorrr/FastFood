using FastFood.DB;
using FastFood.Models;
using FastFood.Models.ViewModels;
using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace FastFood.Areas.Admin.Controllers
{
    [Route("admin/orders")]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderService orderService, IOrderRepository orderRepository)
        {
            _orderService = orderService;
            _orderRepository = orderRepository;
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var orders = await this._orderService.GetOrdersPagedList(page, size);
            var orderCards = await this._orderService.GetOrdersStatusCard();

            ViewBag.Orders = orders;
            ViewBag.OrderCards = orderCards;
            ViewBag.CurrentPage = orders.PageNumber;
            ViewBag.TotalPages = orders.PageCount;
            ViewBag.Title = "Tất cả đơn hàng";
            return View();
        }

        [HttpGet("get/by-status/{id}")]
        public async Task<IActionResult> GetOrdersByStatus([FromQuery] int orderStatusId, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var ordersByStatuses = await this._orderService.GetOrdersByOrderStatusPagedList(orderStatusId, page, size);
            ViewBag.Orders = ordersByStatuses;
            ViewBag.OrderStatusId = orderStatusId;
            return View();
        }

        [HttpGet("detail")]
        public async Task<IActionResult> Detail([FromQuery] int orderId)
        {
            var orderDetail = await this._orderService.GetOrderDetailViewModel(orderId);
            ViewBag.OrderDetail = orderDetail;
            return View();
        }

        [HttpPost("update/status")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> UpdateOrderStatus([FromForm] int orderId, [FromForm] int orderStatus)
        {
            (bool success, string message) = await this._orderService.UpdateOrderStatus(orderId, orderStatus);
            return CreateJsonResult(success, message);
        }

        [HttpPost("update/shipping-info")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> UpdateShippingInfo([FromForm] NewShippingInfo newShippingInfo)
        {
            (bool success, string message) = await this._orderService.AddShippingInfo(newShippingInfo);
            return CreateJsonResult(success, message);
        }

        [HttpPost("check/shipping-info")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> IsAddShippingInfo([FromForm] int orderId)
        {
            (bool success, string message) = await this._orderService.IsAddShippingInfo(orderId);
            return CreateJsonResult(success, message);
        }
    }
}

