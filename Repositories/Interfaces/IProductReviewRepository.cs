using FastFood.DB.Entities;

namespace FastFood.Repositories.Interfaces
{
    public interface IProductReviewRepository
    {
        Task<List<ProductReview>> GetProductReviews();
        Task<double> GetAverageStarOfProduct(int productId);
        Task<List<ProductReview>> GetProductReviewsByProductId(int productId);
        Task<int> GetTotalReviews();
        Task AddProductReview(ProductReview productReview);
    }
}
