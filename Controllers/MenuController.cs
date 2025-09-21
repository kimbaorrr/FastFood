using FastFood.DB;
using FastFood.Models.ViewModels;
using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Areas.Controllers
{
    [Route("menu")]
    public class MenuController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IProductRepository _productRepository;
        private readonly IProductReviewService _productReviewService;
        public MenuController(IProductService productService, IProductRepository productRepository, IProductReviewService productReviewService)
        {
            _productService = productService;
            _productRepository = productRepository;
            _productReviewService = productReviewService;   
        }
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Thực đơn";
            return View();
        }
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> Detail(int productId)
        {
            var product = await this._productRepository.GetProductById(productId);

            ViewBag.Title = "Thông tin món ăn";
            ViewBag.Product = product;
            ViewBag.ReturnUrl = GetAbsoluteUri();

            return View();

        }

        [HttpPost("detail/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Detail(int productId, [FromForm] CustomProductReviewViewModel customProductReviewViewModel)
        {
            (bool success, string message) = await this._productReviewService.AddCustomerProductReview(customProductReviewViewModel);
            return View();
        }


    }
}