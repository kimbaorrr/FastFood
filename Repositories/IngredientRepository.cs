using FastFood.DB;
using FastFood.DB.Entities;
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

    public async Task<Ingredient> GetIngredientById(int ingredientId)
    {
        return await this._fastFoodEntities.Ingredients
            .Where(x => x.IngredientId == ingredientId)
            .FirstOrDefaultAsync() 
            ?? new Ingredient();
    }

    public async Task AddIngredient(Ingredient ingredient)
    {
        await this._fastFoodEntities.AddAsync(ingredient);
        await this._fastFoodEntities.SaveChangesAsync();
    }

    public async Task UpdateIngredient(Ingredient ingredient)
    {
        this._fastFoodEntities.Update(ingredient);
        await this._fastFoodEntities.SaveChangesAsync();
    }

    public async Task DeleteIngredient(Ingredient ingredient)
    {
        this._fastFoodEntities.RemoveRange(ingredient);
        await this._fastFoodEntities.SaveChangesAsync();
    }
}
