using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Areas.Admin.Controllers
{
    [Route("admin/charts")]
    [Authorize(AuthenticationSchemes = "EmployeeScheme")]
    public class ChartController : BaseEmployeeController
    {
        private readonly IOrderService _orderService;
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;
        public ChartController(IOrderService orderService, IProductRepository productRepository, IProductService productService)
        {
            _orderService = orderService;
            _productRepository = productRepository;
            _productService = productService;
        }

        [HttpGet("revenue-by-time-range")]
        public async Task<IActionResult> GetRevenueByTimeRange([FromQuery] string timeRange)
        {
            var orderRevenues = await this._orderService.GetRevenueByTimeRange(timeRange);
            return CreateJsonResult(true, string.Empty, orderRevenues);
        }

        [HttpGet("orders-this-week")]
        public async Task<IActionResult> GetOrdersInThisWeek()
        {
            var daysAgo = DateTime.Now.AddDays(-7);
            var ordersThisWeek = await this._orderService.GetOrdersByDate(daysAgo, DateTime.Now);
            return CreateJsonResult(true, string.Empty, ordersThisWeek);
        }

        [HttpGet("top-selling-products")]
        public async Task<IActionResult> GetTopSellingProducts([FromQuery] int take)
        {
            var products = await this._productService.GetCustomProductsByTopSale(take);
            return CreateJsonResult(true, string.Empty, products);
        }
    }
}
