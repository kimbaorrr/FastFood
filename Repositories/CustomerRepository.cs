using FastFood.DB;
using FastFood.DB.Entities;
using FastFood.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FastFood.Repositories;

public class CustomerRepository : CommonRepository, ICustomerRepository
{
    public CustomerRepository(FastFoodEntities fastFoodEntities) : base(fastFoodEntities) { }

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

    public async Task<Customer> GetCustomerById(int customerId)
    {
        return await this._fastFoodEntities.Customers
            .FirstOrDefaultAsync(x => x.CustomerId == customerId)
            ?? new Customer();
    }

    




}
