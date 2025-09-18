using FastFood.DB;
using FastFood.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FastFood.Repositories;

public class ProductRepository : CommonRepository, IProductRepository
{
    public ProductRepository(FastFoodEntities fastFoodEntities) : base(fastFoodEntities) { }
    public async Task<List<Product>> GetProducts()
    {
        return await this._fastFoodEntities.Products.ToListAsync();
    }
    public async Task<Product> GetProductById(int productId)
    {
        return await this._fastFoodEntities.Products
            .Where(x => x.ProductId == productId)
            .FirstOrDefaultAsync() 
            ?? new Product();
    }

    public async Task<List<Product>> GetProductsByApproveStatus(bool isApproved)
    {
        return await this._fastFoodEntities.Products
            .Where(x => x.IsApprove == isApproved).ToListAsync();
    }

    public async Task<List<Product>> GetProductsByCategory(bool isApproved, int categoryId, int take)
    {
        var products = await this.GetProductsByApproveStatus(isApproved);
        return products
            .Where(x => x.CategoryId == categoryId)
            .Take(take)
            .ToList();
    }

    public async Task<List<Product>> GetRandomProducts(bool isApproved, int excludedProductId, int take)
    {
        var products = await this.GetProductsByApproveStatus(isApproved);
        return products
            .Where(x=>x.ProductId != excludedProductId)
            .OrderBy(x=>Guid.NewGuid())
            .Take(take)
            .ToList();
    }

    public async Task<List<Product>> GetProductsByOrderDiscount()
    {
        var products = await this.GetProductsByApproveStatus(true);
        return products
            .OrderByDescending(x => x.Discount)
            .ToList();
    }

    public async Task<List<Product>> GetProductsByTopSale(int take)
    {
        var products = await this.GetProductsByApproveStatus(true);
        return products
            .OrderByDescending(x=>x.FinalPrice)
            .Take(take)
            .ToList();
    }


    
}
