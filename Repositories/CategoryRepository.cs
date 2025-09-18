using FastFood.DB;
using FastFood.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FastFood.Repositories;

public class CategoryRepository : CommonRepository, ICategoryRepository
{
    public CategoryRepository(FastFoodEntities fastFoodEntities) : base (fastFoodEntities) { }

    public async Task<List<Category>> GetCategories()
    {
        return await this._fastFoodEntities.Categories.ToListAsync();
    }

    public async Task<int> CountProductInCategory(int categoryId)
    {
        return await this._fastFoodEntities.Categories.Where(x=>x.CategoryId == categoryId).CountAsync();
    }

    public async Task<List<Category>> TakeOrderCategoryByProductQuantity(int take)
    {
        return await this._fastFoodEntities.Categories
            .Include(x=>x.Products)
            .OrderByDescending(x=>x.Products)
            .Take(take)
            .ToListAsync();
    }

    public async Task<string> GetCategoryName(int categoryId)
    {
        return await this._fastFoodEntities.Categories
            .Where(x => x.CategoryId == categoryId)
            .Select(x => x.CategoryName)
            .FirstOrDefaultAsync() 
            ?? string.Empty;
    }
}
