using FastFood.DB;
using FastFood.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FastFood.Repositories;

public class OrderRepository : CommonRepository, IOrderRepository
{
    public OrderRepository(FastFoodEntities fastFoodEntities) : base(fastFoodEntities) { }

    public async Task<List<Order>> GetOrders()
    {
        return await this._fastFoodEntities.Orders.ToListAsync();
    }

    public async Task<List<Order>> GetOrdersWithDetails()
    {
        return await this._fastFoodEntities.Orders.Include(x => x.OrderDetails).ToListAsync();
    }

    public async Task<List<Order>> GetOrdersDelivered()
    {
        return await this._fastFoodEntities.Orders.Where(x => x.OrderStatus == 7).ToListAsync();
    }

    public async Task<int> CountOrders()
    {
        return await this._fastFoodEntities.Orders.CountAsync();
    }

    public async Task<int> CalculateTotalOrdersRevenue()
    {
        return await this._fastFoodEntities.Orders.SumAsync(x => x.TotalPrice);
    }

    public async Task<double> CalculateAverageOrdersRevenue()
    {
        return await this._fastFoodEntities.Orders.AverageAsync(x => x.TotalPrice);
    }







}
