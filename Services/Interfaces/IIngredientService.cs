using FastFood.Models.ViewModels;

namespace FastFood.Services.Interfaces
{
    public interface IIngredientService
    {
        Task<List<OriginalIngredientViewModel>> GetOriginalIngredientViewModels();
        Task<List<CustomIngredientViewModel>> GetCustomIngredientViewModels(int productID);
    }
}