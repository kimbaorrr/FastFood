using FastFood.DB;
using FastFood.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FastFood.Repositories;

public class IngredientRepository : CommonRepository, IIngredientRepository
{
    public IngredientRepository(FastFoodEntities fastFoodEntities) : base(fastFoodEntities) { }

    public async Task<List<Ingredient>> GetIngredients()
    {
        return await this._fastFoodEntities.Ingredients.ToListAsync();
    }
}
