using FastFood.DB;
using FastFood.Models;
using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace FastFood.Areas.Admin.Controllers
{
    [Route("admin/ingredient")]
    public class IngredientController : BaseController
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IIngredientService _ingredientService;

        public IngredientController(IIngredientRepository ingredientRepository, IIngredientService ingredientService)
        {
            _ingredientRepository = ingredientRepository;
            _ingredientService = ingredientService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetIngredients()
        {
            var ingredients = await this._ingredientRepository.GetIngredients();
            var customIngredients = ingredients
                .Select(x => new
                {
                    IngredientId = x.IngredientId,
                    IngredientName = x.IngredientName,
                    Description = x.Description
                });
            return CreateJsonResult(true, string.Empty, customIngredients);
        }

        [HttpGet("by-product/get")]
        public async Task<IActionResult> GetByProductID([FromQuery] int productId)
        {
            var ingredients = await this._ingredientRepository.GetIngredientById(productId);
            return CreateJsonResult(true, string.Empty, ingredients);
        }






    }
}