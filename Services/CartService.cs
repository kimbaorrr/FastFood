using FastFood.Models;
using FastFood.Models.ViewModels;
using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace FastFood.Services
{
    public class CartService : CommonService, ICartService
    {
        private readonly IProductRepository _productRepository;
        private readonly IPromoRepository _promoRepository;

        public CartService(IProductRepository productRepository, IPromoRepository promoRepository)
        {
            _productRepository = productRepository;
            _promoRepository = promoRepository;
        }

        public CartService(Dictionary<int, CustomerCartViewModel> customerCartViewModel)
        {
            this.CustomerCartViewModel = customerCartViewModel;
        }

        public Dictionary<int, CustomerCartViewModel> CustomerCartViewModel { get; set; }
            = new Dictionary<int, CustomerCartViewModel>();

        /// <summary>
        /// Kiểm tra xem giỏ hàng có rỗng hay không.
        /// </summary>
        /// <returns>Trả về true nếu giỏ hàng rỗng, ngược lại là false.</returns>
        public bool IsCartEmpty() => !this.CustomerCartViewModel.Any();

        /// <summary>
        /// Thêm sản phẩm vào giỏ hàng.
        /// </summary>
        /// <param name="maSanPham">Mã sản phẩm.</param>
        /// <param name="soLuong">Số lượng sản phẩm cần thêm.</param>
        public async Task AddItem(int productId, int quantity)
        {
            if (this.CustomerCartViewModel.TryGetValue(productId, out CustomerCartViewModel? customerCartViewModel))
            {
                customerCartViewModel.Quantity += quantity;
            }
            else
            {
                var product = await this._productRepository.GetProductById(productId);
                if (product == null) return;

                this.CustomerCartViewModel[productId] = new()
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Discount = product.Discount,
                    OriginalPrice = product.OriginalPrice,
                    ProductImage = string.Empty,
                    ProductName = product.ProductName
                };
            }
        }

        /// <summary>
        /// Xóa sản phẩm khỏi giỏ hàng.
        /// </summary>
        /// <param name="maSanPham">Mã sản phẩm cần xóa.</param>
        public void RemoveItem(int productId)
        {
            this.CustomerCartViewModel.Remove(productId);
        }

        /// <summary>
        /// Giảm số lượng của một sản phẩm trong giỏ hàng.
        /// </summary>
        /// <param name="maSanPham">Mã sản phẩm cần giảm số lượng.</param>
        public void DecreaseQuantity(int productId)
        {
            if (!this.CustomerCartViewModel.TryGetValue(productId, out CustomerCartViewModel? customerCartViewModel)) return;

            customerCartViewModel.Quantity--;

            if (customerCartViewModel.Quantity <= 0)
                this.RemoveItem(productId);
        }

        /// <summary>
        /// Tính tổng tiền của tất cả sản phẩm trong giỏ hàng.
        /// </summary>
        /// <returns>Tổng tiền của giỏ hàng.</returns>
        public int TotalPay() => this.CustomerCartViewModel.Values.Sum(x => x.FinalPrice) ?? 0;

        public async Task<(bool, string, string)> GetSummaryCheckout(string promoCode)
        {
            var promo = await this._promoRepository.GetPromoByPromoCode(promoCode);

            if (promo != null)
            {
                bool isExpired = promo.EndTime.HasValue && DateTime.Now > promo.EndTime.Value;
                bool isUsable = promo.Usage > 0;

                if (isExpired || !isUsable)
                {
                    return (false, "Mã khuyến mãi đã hết hạn hoặc không còn lượt sử dụng !", string.Empty);
                }

                PaymentSummaryViewModel paymentSummaryViewModel = new()
                {
                    PromoCode = promoCode,
                    PromoId = promo.PromoId,
                    PromoAmount = promo.DiscountAmount,
                    ShippingFee = 20000,
                    TotalProductPrice = this.TotalPay(),
                    
                };

                return (true, string.Empty, JsonConvert.SerializeObject(paymentSummaryViewModel));
            }

            return (false, "Mã khuyến mãi không hợp lệ !", string.Empty); 
            
        }
    }
}
