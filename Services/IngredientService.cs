using FastFood.Models;
using FastFood.Models.ViewModels;
using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;

namespace FastFood.Services
{
    public class IngredientService : CommonService, IIngredientService
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IProductIngredientRepository _productIngredientRepository;
        public IngredientService(IIngredientRepository ingredientRepository, IProductIngredientRepository productIngredientRepository)
        {
            _ingredientRepository = ingredientRepository;
            _productIngredientRepository = productIngredientRepository;
        }

        public async Task<List<OriginalIngredientViewModel>> GetOriginalIngredientViewModels()
        {
            var ingredients = await this._ingredientRepository.GetIngredients();
            return ingredients.Select(x => new OriginalIngredientViewModel
            {
                IngredientId = x.IngredientId,
                IngredientName = x.IngredientName,
                Description = x.Description ?? string.Empty
            }).ToList();
        }

        public async Task<List<CustomIngredientViewModel>> GetCustomIngredientViewModels(int productID)
        {
            var productIngredientsWithDetails = await this._productIngredientRepository.GetProductIngredientsWithDetails();
            return productIngredientsWithDetails
                .Where(x => x.ProductId == productID)
                .Select(x => new CustomIngredientViewModel()
                {
                    IngredientId = x.IngredientId,
                    IngredientName = x.Ingredient.IngredientName,
                    Description = x.Ingredient.Description ?? string.Empty,
                    QuantityNeeded = x.QuantityNeeded,
                    Unit = x.Unit
                }).ToList();
        }


    }
}
