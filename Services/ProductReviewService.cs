using FastFood.DB.Entities;
using FastFood.Models.ViewModels;
using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;

namespace FastFood.Services
{
    public class ProductReviewService : CommonService, IProductReviewService
    {
        private readonly IProductReviewRepository _productReviewRepository;
        public ProductReviewService(IProductReviewRepository productReviewRepository)
        {
            _productReviewRepository = productReviewRepository;
        }

        public async Task<ProductReview> GetCustomersHasTopProductStar()
        {
            var productReviews = await this._productReviewRepository.GetProductReviews();
            return productReviews
                .GroupBy(x => x.CustomerId)
                .Select(g => g.OrderByDescending(x => x.StarRating).FirstOrDefault()!)
                .OrderByDescending(x => x.StarRating)
                .FirstOrDefault() ?? new ProductReview();
        }

        public async Task<(bool, string)> AddCustomerProductReview(CustomProductReviewViewModel customProductReviewViewModel)
        {
            ProductReview productReview = new()
            {
                CreatedAt = DateTime.Now,
                CustomerId = customProductReviewViewModel.CustomerId,
                ReviewContent = customProductReviewViewModel.Content,
                StarRating = customProductReviewViewModel.StarRating,
                ProductId = customProductReviewViewModel.ProductId
            };
            await this._productReviewRepository.AddProductReview(productReview);
            return (true, $"Thêm đánh giá thành công cho sản phẩm {customProductReviewViewModel.ProductId} !");
        }


    }
}
