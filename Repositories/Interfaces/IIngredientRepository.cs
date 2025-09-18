using FastFood.DB;

namespace FastFood.Repositories.Interfaces
{
    public interface IIngredientRepository
    {
        Task<List<Ingredient>> GetIngredients();
    }
}
