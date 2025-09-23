using FastFood.DB;
using FastFood.DB.Entities;
using FastFood.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.CodeAnalysis;
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
            .FirstOrDefaultAsync(x => x.ProductId == productId) 
            ?? new Product();
    }

    public async Task<Product> GetProductByName(string productName)
    {
        return await this._fastFoodEntities.Products
            .FirstOrDefaultAsync(x => x.ProductName.Equals(productName))
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

    public async Task<List<Product>> GetProductsByOrderDiscount(int take)
    {
        var products = await this.GetProductsByApproveStatus(true);
        return products
            .OrderByDescending(x => x.Discount)
            .Take(take)
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

    public async Task<Product> GetBestSellingProduct()
    {
        return await this._fastFoodEntities.Products
        .OrderByDescending(p => p.OrderDetails.Count)
        .FirstOrDefaultAsync() ?? new Product();
    }

    public async Task AddProduct(Product product)
    {
        await this._fastFoodEntities.AddAsync(product);
        await this._fastFoodEntities.SaveChangesAsync();
    }

    public async Task UpdateProduct(Product product)
    {
        this._fastFoodEntities.UpdateRange(product);
        await this._fastFoodEntities.SaveChangesAsync();
    }


    public async Task DeleteProduct(Product product)
    {
        this._fastFoodEntities.Remove(product);
        await this._fastFoodEntities.SaveChangesAsync();
    }

    public async Task<bool> IsProductHasOrder(int productId)
    {
        return await this._fastFoodEntities.OrderDetails
            .Where(x => x.ProductId == productId)
            .AnyAsync();
    }


}
