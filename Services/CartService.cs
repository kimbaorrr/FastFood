using FastFood.Models.ViewModels;
using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace FastFood.Services
{
    public class CartService : CommonService, ICartService
    {
        private readonly IProductRepository _productRepository;
        public CartService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public Dictionary<int, CustomerCartViewModel> CustomerCartViewModel { get; set; } = new Dictionary<int, CustomerCartViewModel>();

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
    }
}
