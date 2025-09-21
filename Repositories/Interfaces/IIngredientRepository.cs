using FastFood.DB.Entities;

namespace FastFood.Repositories.Interfaces
{
    public interface IIngredientRepository
    {
        Task<List<Ingredient>> GetIngredients();
        Task<Ingredient> GetIngredientById(int ingredientId);
        Task AddIngredient(Ingredient ingredient);
        Task UpdateIngredient(Ingredient ingredient);
        Task DeleteIngredient(Ingredient ingredient);
    }
}
