using FastFood.DB;
using FastFood.DB.Entities;
using FastFood.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Repositories;

public class ProductReviewRepository : CommonRepository, IProductReviewRepository
{
    public ProductReviewRepository(FastFoodEntities fastFoodEntities) : base(fastFoodEntities)
    {
           
    }

    public async Task<List<ProductReview>> GetProductReviews()
    {
        return await this._fastFoodEntities.ProductReviews.ToListAsync();
    }

    public async Task<double> GetAverageStarOfProduct(int productId)
    {
        return await this._fastFoodEntities.ProductReviews
            .Where(x => x.ProductId == productId)
            .AverageAsync(x => x.StarRating) ?? 0; 
    }

    public async Task<ProductReview> GetProductReviewByProductId(int productId)
    {
        return await this._fastFoodEntities.ProductReviews
            .Where(x => x.ProductId == productId)
            .FirstOrDefaultAsync() ?? new ProductReview();
    }

    public async Task<int> GetTotalReviews()
    {
        return await this._fastFoodEntities.ProductReviews.CountAsync();
    }

    public async Task AddProductReview(ProductReview productReview)
    {
        await this._fastFoodEntities.AddAsync(productReview);
        await this._fastFoodEntities.SaveChangesAsync();
    }
}
