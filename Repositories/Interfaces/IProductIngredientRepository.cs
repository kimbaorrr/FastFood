using FastFood.DB.Entities;

namespace FastFood.Repositories.Interfaces
{
    public interface IProductIngredientRepository
    {
        Task<List<ProductIngredient>> GetProductIngredients();
        Task<List<ProductIngredient>> GetProductIngredientsWithDetails();
        Task AddProductIngredient(ProductIngredient productIngredient);
        Task UpdateProductIngredient(ProductIngredient productIngredient);
        Task DeleteProductIngredient(ProductIngredient productIngredient);
        Task<ProductIngredient> GetProductIngredientByProductId(int productId);
    }
}
