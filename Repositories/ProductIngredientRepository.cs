using FastFood.DB;
using FastFood.DB.Entities;
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

    public async Task AddProductIngredient(ProductIngredient productIngredient)
    {
        await this._fastFoodEntities.AddAsync(productIngredient);
        await this._fastFoodEntities.SaveChangesAsync();
    }

    public async Task UpdateProductIngredient(ProductIngredient productIngredient)
    {
        this._fastFoodEntities.UpdateRange(productIngredient);
        await this._fastFoodEntities.SaveChangesAsync();
    }


    public async Task DeleteProductIngredient(ProductIngredient productIngredient)
    {
        this._fastFoodEntities.RemoveRange(productIngredient);
        await this._fastFoodEntities.SaveChangesAsync();
    }

    public async Task<ProductIngredient> GetProductIngredientByProductId(int productId)
    {
        return await this._fastFoodEntities.ProductIngredients
            .Where(x=>x.ProductId == productId)
            .FirstOrDefaultAsync() ?? new ProductIngredient();
    }
}
