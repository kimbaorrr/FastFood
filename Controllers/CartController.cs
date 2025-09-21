using FastFood.DB;
using FastFood.Models;
using FastFood.Models.ViewModels;
using FastFood.Services;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FastFood.Areas.Controllers
{
    [Route("cart")]
    public class CartController : BaseController
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
            cartService.CustomerCartViewModel = this._customerCartViewModel;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Giỏ hàng của tôi";
            return View();
        }

        [HttpGet("get-items")]
        public async Task<IActionResult> GetCartItems()
        {
            if (this._cartService.IsCartEmpty())
            {
                return CreateJsonResult(false, "Giỏ hàng rỗng !");
            }
            return CreateJsonResult(true, string.Empty, JsonConvert.SerializeObject(this._cartService.CustomerCartViewModel));
        }

        [HttpPost("add-item")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddItem([FromForm] int productId, [FromForm] int quantity = 1)
        {
            await this._cartService.AddItem(productId, quantity);
            return CreateJsonResult(true, "Thêm sản phẩm thành công !");
        }

        [HttpPost("remove-item")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> RemoveItem([FromForm] int productId)
        {
            this._cartService.RemoveItem(productId);
            return CreateJsonResult(true, $"Đã xóa sản phẩm {productId} khỏi giỏ !");
        }

        [HttpPost("decrease-quantity")]
        public async Task<IActionResult> DecreaseQuantity([FromForm] int productId)
        {
            this._cartService.DecreaseQuantity(productId);
            return CreateJsonResult(true, "Giảm số lượng thành công !");

        }

        [HttpPost("check-is-empty")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsCartEmpty()
        {
            if (this._cartService.IsCartEmpty())
            {
                return CreateJsonResult(true, "Giỏ hàng hiện đang rỗng !");
            }

            return CreateJsonResult(false, "Giỏ hàng hiện đã có ít nhất 1 sản phẩm !");
        }

        [HttpPost("summary-checkout")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> GetSummaryCheckout([FromForm] string promoCode)
        {
            (bool success, string message, string summaryCheckoutSerialized) = await this._cartService.GetSummaryCheckout(promoCode);
            return CreateJsonResult(success, message, summaryCheckoutSerialized);
        }
    }
}
