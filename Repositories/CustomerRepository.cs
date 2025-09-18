using FastFood.DB;
using FastFood.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FastFood.Repositories;

public class CustomerRepository : CommonRepository, ICustomerRepository
{
    public CustomerRepository(FastFoodEntities fastFoodEntities) : base(fastFoodEntities) { }
    public async Task<int> CountNewCustomers(DateTime fromTime, DateTime toTime)
    {
        return await this._fastFoodEntities.Customers
            .Where(x => x.CreatedAt >= fromTime && x.CreatedAt < toTime).CountAsync();
    }

    public async Task<List<Customer>> GetCustomers()
    {
        return await this._fastFoodEntities.Customers.ToListAsync();
    }

    public string GetFullName(int customerId)
    {
        return this._fastFoodEntities.Customers
            .Where(x=>x.CustomerId == customerId)
            .Select(x => x.LastName + " " + x.FirstName).FirstOrDefault() ?? string.Empty;
    }

    public async Task<List<Customer>> GetCustomersByOrderStatus(int orderStatus)
    {
        return await this._fastFoodEntities.Customers
            .Include(x=>x.Orders)
            .Where(x=>x.Orders.Any(x=>x.OrderStatus == 7))
            .ToListAsync();
    }




}
