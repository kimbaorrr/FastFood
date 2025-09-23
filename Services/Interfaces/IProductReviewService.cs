using FastFood.DB.Entities;
using FastFood.Models.ViewModels;

namespace FastFood.Services.Interfaces
{
    /// <summary>
    /// Interface cho các dịch vụ đánh giá sản phẩm.
    /// </summary>
    public interface IProductReviewService
    {
        /// <summary>
        /// Lấy thông tin khách hàng có đánh giá sản phẩm cao nhất.
        /// </summary>
        /// <returns>Đánh giá sản phẩm của khách hàng.</returns>
        Task<ProductReview> GetCustomersHasTopProductStar();

        /// <summary>
        /// Thêm đánh giá sản phẩm từ khách hàng.
        /// </summary>
        /// <param name="customProductReviewViewModel">Model đánh giá sản phẩm.</param>
        /// <returns>Kết quả thêm đánh giá và thông báo.</returns>
        Task<(bool, string)> AddCustomerProductReview(CustomProductReviewViewModel customProductReviewViewModel);
    }
}
