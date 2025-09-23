using FastFood.Models.ViewModels;
using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Controllers
{
    /// <summary>
    /// Controller xử lý các chức năng trang chủ cho khách hàng.
    /// </summary>
    public class HomeController : BaseCustomerController
    {
        private readonly IProductService _productService;
        private readonly IProductRepository _productRepository;
        private readonly IProductReviewRepository _productReviewRepository;
        private readonly IArticleRepository _articleRepository;

        /// <summary>
        /// Khởi tạo controller trang chủ với các dịch vụ cần thiết.
        /// </summary>
        /// <param name="productService">Dịch vụ sản phẩm.</param>
        /// <param name="productRepository">Kho lưu trữ sản phẩm.</param>
        /// <param name="productReviewRepository">Kho lưu trữ đánh giá sản phẩm.</param>
        /// <param name="articleRepository">Kho lưu trữ bài viết.</param>
        public HomeController(IProductService productService, IProductRepository productRepository, IProductReviewRepository productReviewRepository, IArticleRepository articleRepository)
        {
            _productService = productService;
            _productRepository = productRepository;
            _productReviewRepository = productReviewRepository;
            _articleRepository = articleRepository;
        }

        /// <summary>
        /// Hiển thị trang chủ với các thông tin sản phẩm, bài viết và đánh giá.
        /// </summary>
        /// <returns>Trang chủ với dữ liệu tổng hợp cho khách hàng.</returns>
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Trang chủ";
            var bestSellingProduct = await this._productRepository.GetBestSellingProduct();
            var top2DiscountProducts = await this._productRepository.GetProductsByOrderDiscount(2);
            var productReviews = await this._productReviewRepository.GetProductReviews();
            var top10HotSales = await this._productRepository.GetProductsByTopSale(10);
            var top3Articles = await this._articleRepository.GetArticlesByApproved(true);

            CustomerCustomHomeViewModel customerCustomHomeViewModel = new()
            {
                BestSellingProduct = bestSellingProduct,
                Top2DiscountProducts = top2DiscountProducts,
                Top10HotSales = top10HotSales,
                Top3Articles = top3Articles,
                ProductReviews = productReviews
            };

            return View(customerCustomHomeViewModel);
        }
    }
}