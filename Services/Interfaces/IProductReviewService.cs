using FastFood.DB.Entities;
using FastFood.Models.ViewModels;

namespace FastFood.Services.Interfaces
{
    public interface IProductReviewService
    {
        Task<ProductReview> GetCustomersHasTopProductStar();
        Task<(bool, string)> AddCustomerProductReview(CustomProductReviewViewModel customProductReviewViewModel);
    }
}
