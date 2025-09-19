using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Areas.Admin.Controllers
{
    [Route("admin/charts")]
    public class ChartController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IProductRepository _productRepository;
        public ChartController(IOrderService orderService, IProductRepository productRepository)
        {
            _orderService = orderService;
            _productRepository = productRepository;
        }

        [HttpGet("orders/revenue/time-range/get/{timeRange}")]
        public async Task<IActionResult> GetRevenueByTimeRange([FromQuery] string timeRange)
        {
            var orderRevenues = await this._orderService.GetRevenueByTimeRange(timeRange);
            return CreateJsonResult(true, string.Empty, orderRevenues);
        }

        [HttpGet("orders/this-week/get")]
        public async Task<IActionResult> GetOrdersThisWeek()
        {
            var daysAgo = DateTime.Now.AddDays(-7);
            var ordersThisWeek = await this._orderService.GetOrdersByDate(daysAgo, DateTime.Now);
            return CreateJsonResult(true, string.Empty, ordersThisWeek);
        }

        [HttpGet("products/top-sale/get")]
        public async Task<IActionResult> GetProductsTopSale([FromQuery] int take)
        {
            var products = await this._productRepository.GetProductsByTopSale(take);
            return CreateJsonResult(true, string.Empty, products);
        }
    }
}
