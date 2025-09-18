using FastFood.DB;
using FastFood.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FastFood.Repositories;

public class ProductIngredientRepository : CommonRepository, IProductIngredientRepository
{
    public ProductIngredientRepository(FastFoodEntities fastFoodEntities) : base(fastFoodEntities) { }

    public async Task<List<ProductIngredient>> GetProductIngredients()
    {
        return await this._fastFoodEntities.ProductIngredients.ToListAsync();
    }
    public async Task<List<ProductIngredient>> GetProductIngredientsWithDetails()
    {
        return await this._fastFoodEntities.ProductIngredients.Include(x=>x.Ingredient).ToListAsync();
    }
}
