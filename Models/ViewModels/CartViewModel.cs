using FastFood.DB;
using FastFood.Repositories.Interfaces;

namespace FastFood.Models.ViewModels
{
    public class CustomerCartViewModel
    {
        public int ProductId { get; set; } = -1;

        /// <summary>
        /// Tên sản phẩm.
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Số lượng sản phẩm.
        /// </summary>
        public int Quantity { get; set; } = 1;

        /// <summary>
        /// Giá bán của sản phẩm.
        /// </summary>
        public int OriginalPrice { get; set; } = 0;
        public int? Discount { get; set; } = 0;

        /// <summary>
        /// Tổng tiền của sản phẩm dựa trên số lượng và giá bán.
        /// </summary>
        public int? FinalPrice => OriginalPrice * Quantity * (Discount/100);

        /// <summary>
        /// Hình ảnh của sản phẩm.
        /// </summary>
        public string ProductImage { get; set; } = string.Empty;
    }
}
