using FastFood.DB;

namespace FastFood.Services.Interfaces
{
    public interface IProductReviewService
    {
        Task<ProductReview> GetCustomersHasTopProductStar();
    }
}
