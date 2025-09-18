using FastFood.DB;

namespace FastFood.Repositories.Interfaces
{
    public interface IProductReviewRepository
    {
        Task<List<ProductReview>> GetProductReviews();
        Task<double> GetAverageStarOfProduct(int productId);
        Task<ProductReview> GetProductReviewByProductId(int productId);
        Task<int> GetTotalReviews();
    }
}
