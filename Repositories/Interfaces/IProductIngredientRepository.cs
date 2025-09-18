using FastFood.DB;

namespace FastFood.Repositories.Interfaces
{
    public interface IProductIngredientRepository
    {
        Task<List<ProductIngredient>> GetProductIngredients();
        Task<List<ProductIngredient>> GetProductIngredientsWithDetails();
    }
}
