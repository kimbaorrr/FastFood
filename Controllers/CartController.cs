using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FastFood.Controllers
{
    /// <summary>
    /// Controller xử lý các chức năng liên quan đến giỏ hàng của khách hàng.
    /// </summary>
    [Route("cart")]
    [AllowAnonymous]
    public class CartController : BaseCustomerController
    {
        private readonly ICartService _cartService;

        /// <summary>
        /// Khởi tạo controller giỏ hàng với dịch vụ giỏ hàng.
        /// </summary>
        /// <param name="cartService">Dịch vụ giỏ hàng.</param>
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        /// <summary>
        /// Hiển thị trang giỏ hàng của khách hàng.
        /// </summary>
        /// <returns>Trang View giỏ hàng.</returns>
        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.Title = "Giỏ hàng của tôi";
            return View();
        }

        /// <summary>
        /// Lấy danh sách sản phẩm trong giỏ hàng.
        /// </summary>
        /// <returns>Kết quả JSON chứa thông tin giỏ hàng.</returns>
        [HttpGet("get-items")]
        public IActionResult GetItems()
        {
            if (this._cartService.IsCartEmpty())
                return CreateJsonResult(false, "Giỏ hàng rỗng !" );

            return CreateJsonResult(true, string.Empty, this._cartService.CustomerCartViewModel);
        }

        /// <summary>
        /// Thêm sản phẩm vào giỏ hàng.
        /// </summary>
        /// <param name="productId">ID sản phẩm cần thêm.</param>
        /// <param name="quantity">Số lượng sản phẩm (mặc định là 1).</param>
        /// <returns>Kết quả thêm sản phẩm dưới dạng JSON.</returns>
        [HttpPost("add-item")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddItem([FromForm] int productId, [FromForm] int quantity = 1)
        {
            await this._cartService.AddItem(productId, quantity);
            return CreateJsonResult(true, "Thêm sản phẩm thành công !");
        }

        /// <summary>
        /// Xóa sản phẩm khỏi giỏ hàng.
        /// </summary>
        /// <param name="productId">ID sản phẩm cần xóa.</param>
        /// <returns>Kết quả xóa sản phẩm dưới dạng JSON.</returns>
        [HttpPost("remove-item")]
        [AutoValidateAntiforgeryToken]
        public IActionResult RemoveItem([FromForm] int productId)
        {
            this._cartService.RemoveItem(productId);
            return CreateJsonResult(true, $"Đã xóa sản phẩm {productId} khỏi giỏ !");
        }

        /// <summary>
        /// Giảm số lượng sản phẩm trong giỏ hàng.
        /// </summary>
        /// <param name="productId">ID sản phẩm cần giảm số lượng.</param>
        /// <returns>Kết quả giảm số lượng dưới dạng JSON.</returns>
        [HttpPost("decrease-quantity")]
        [AutoValidateAntiforgeryToken]
        public IActionResult DecreaseQuantity([FromForm] int productId)
        {
            this._cartService.DecreaseQuantity(productId);
            return CreateJsonResult(true, "Giảm số lượng thành công !");
        }

        /// <summary>
        /// Kiểm tra giỏ hàng có rỗng hay không.
        /// </summary>
        /// <returns>Kết quả kiểm tra dưới dạng JSON.</returns>
        [HttpPost("check-is-empty")]
        [AutoValidateAntiforgeryToken]
        public IActionResult IsCartEmpty()
        {
            if (this._cartService.IsCartEmpty())
                return CreateJsonResult(true, "Giỏ hàng hiện đang rỗng !");

            return CreateJsonResult(false, "Giỏ hàng hiện đã có ít nhất 1 sản phẩm !" );
        }

        /// <summary>
        /// Lấy thông tin tổng kết thanh toán của giỏ hàng.
        /// </summary>
        /// <param name="promoCode">Mã khuyến mãi (nếu có).</param>
        /// <returns>Kết quả tổng kết thanh toán dưới dạng JSON.</returns>
        [HttpPost("summary-checkout")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> GetSummaryCheckout([FromForm] string promoCode)
        {
            (bool success, string message, string summaryCheckout) = await _cartService.GetSummaryCheckout(promoCode);
            this.AddPaymentSummaryToSession(summaryCheckout);
            return CreateJsonResult(true, string.Empty, summaryCheckout);
        }
    }
}
