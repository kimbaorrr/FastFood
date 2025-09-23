using FastFood.DB;
using FastFood.Models.ViewModels;
using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Controllers
{
    /// <summary>
    /// Controller xử lý các chức năng liên quan đến thực đơn cho khách hàng.
    /// </summary>
    [Route("menu")]
    public class MenuController : BaseCustomerController
    {
        private readonly IProductService _productService;
        private readonly IProductRepository _productRepository;
        private readonly IProductReviewService _productReviewService;
        private readonly ICategoryService _categoryService;

        /// <summary>
        /// Khởi tạo controller thực đơn với các dịch vụ cần thiết.
        /// </summary>
        /// <param name="productService">Dịch vụ sản phẩm.</param>
        /// <param name="productRepository">Kho lưu trữ sản phẩm.</param>
        /// <param name="productReviewService">Dịch vụ đánh giá sản phẩm.</param>
        /// <param name="categoryService">Dịch vụ danh mục sản phẩm.</param>
        public MenuController(IProductService productService, IProductRepository productRepository, IProductReviewService productReviewService, ICategoryService categoryService)
        {
            _productService = productService;
            _productRepository = productRepository;
            _productReviewService = productReviewService;   
            _categoryService = categoryService;
        }

        /// <summary>
        /// Hiển thị trang thực đơn với danh sách danh mục sản phẩm.
        /// </summary>
        /// <returns>Trang thực đơn.</returns>
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Thực đơn";
            var customCategoryViewModel = await this._categoryService.GetCustomCategoryViewModel();
            return View(customCategoryViewModel);
        }

        /// <summary>
        /// Hiển thị chi tiết món ăn.
        /// </summary>
        /// <param name="productId">ID sản phẩm cần xem chi tiết.</param>
        /// <returns>Trang chi tiết món ăn.</returns>
        [HttpGet("detail/{productId}")]
        public async Task<IActionResult> Detail(int productId)
        {
            var customProductDetailViewModel = await this._productService.GetCustomProductDetailViewModel(productId);

            ViewBag.Title = "Thông tin món ăn";
            ViewBag.ReturnUrl = GetAbsoluteUri();

            return View(customProductDetailViewModel);
        }

        /// <summary>
        /// Thêm đánh giá sản phẩm từ khách hàng.
        /// </summary>
        /// <param name="productId">ID sản phẩm được đánh giá.</param>
        /// <param name="customProductReviewViewModel">Model đánh giá sản phẩm.</param>
        /// <returns>Trang chi tiết món ăn sau khi thêm đánh giá.</returns>
        [HttpPost("detail/{productId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Detail(int productId, [FromForm] CustomProductReviewViewModel customProductReviewViewModel)
        {
            (bool success, string message) = await this._productReviewService.AddCustomerProductReview(customProductReviewViewModel);
            return View();
        }
    }
}