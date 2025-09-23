using FastFood.DB.Entities;
using FastFood.Models;
using FastFood.Models.ViewModels;
using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace FastFood.Services
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductRepository _productRepository;
        private readonly IPromoRepository _promoRepository;

        public CartService(
            IHttpContextAccessor httpContextAccessor,
            IProductRepository productRepository,
            IPromoRepository promoRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _productRepository = productRepository;
            _promoRepository = promoRepository;
        }

        private ISession Session => _httpContextAccessor.HttpContext!.Session;

        private Dictionary<int, CustomerCartViewModel> GetCart()
        {
            var json = Session.GetString("CartInfo");
            if (string.IsNullOrEmpty(json))
                return new();

            return JsonConvert.DeserializeObject<Dictionary<int, CustomerCartViewModel>>(json)!;
        }

        private void SaveCart(Dictionary<int, CustomerCartViewModel> cart)
        {
            var json = JsonConvert.SerializeObject(cart);
            Session.SetString("CartInfo", json);
        }

        public Dictionary<int, CustomerCartViewModel> CustomerCartViewModel
        {
            get => this.GetCart();
            set => this.SaveCart(value);
        }

        public bool IsCartEmpty() => !this.GetCart().Any();

        public async Task AddItem(int productId, int quantity)
        {
            var cart = this.GetCart();
            var product = await _productRepository.GetProductById(productId);

            if (cart.TryGetValue(productId, out var item))
            {
                item.Quantity += quantity;
                item.FinalPrice = CalculateFinalPrice(product.OriginalPrice, product.Discount, item.Quantity);
            }
            else
            {
                cart[productId] = new CustomerCartViewModel
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Discount = product.Discount,
                    OriginalPrice = product.OriginalPrice,
                    ProductImage = product.Image?.Split(",").FirstOrDefault() ?? string.Empty,
                    ProductName = product.ProductName,
                    FinalPrice = CalculateFinalPrice(product.OriginalPrice, product.Discount, quantity)
                };
            }

            this.SaveCart(cart);
        }

        public void RemoveItem(int productId)
        {
            var cart = this.GetCart();
            cart.Remove(productId);
            this.SaveCart(cart);
        }

        public void DecreaseQuantity(int productId)
        {
            var cart = GetCart();

            if (!cart.TryGetValue(productId, out var item)) return;

            item.Quantity--;
            item.FinalPrice = CalculateFinalPrice(item.OriginalPrice, item.Discount, item.Quantity);

            if (item.Quantity <= 0)
                cart.Remove(productId);

            SaveCart(cart);
        }

        private int CalculateTotalProductPrice()
        {
            var cart = this.GetCart();
            return cart.Values.Sum(x => (int)((x.OriginalPrice * (100 - x.Discount) / 100.0) * x.Quantity));
        }

        public async Task<(bool, string, string)> GetSummaryCheckout(string promoCode)
        {
            var cart = this.GetCart();
            
            if (string.IsNullOrEmpty(promoCode))
            {
                PaymentSummaryViewModel paymentSummaryViewModel = new()
                {
                    PromoCode = string.Empty,
                    PromoId = null,
                    PromoAmount = 0,
                    ShippingFee = 20000,
                    TotalProductPrice = this.CalculateTotalProductPrice(),
                    TotalPay = CalculateTotalPay()
                };

                return (true, string.Empty, JsonConvert.SerializeObject(paymentSummaryViewModel));
            }

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
                    TotalProductPrice = this.CalculateTotalProductPrice(),
                    TotalPay = CalculateTotalPay(promo.DiscountAmount)
                };

                return (true, string.Empty, JsonConvert.SerializeObject(paymentSummaryViewModel));
            }

            return (false, "Mã khuyến mãi không hợp lệ !", string.Empty);
        }

        private int CalculateTotalPay(int discountAmount = 0)
        {
            int totalProductPrice = this.CalculateTotalProductPrice();
            int shippingFee = 20000;
            return shippingFee + totalProductPrice - discountAmount;
        }

        private static int CalculateFinalPrice(int originalPrice, int? discount, int quantity)
        {
            var discountValue = discount ?? 0;
            return (int)((originalPrice * (100 - discountValue) / 100.0) * quantity);
        }
    }
}
