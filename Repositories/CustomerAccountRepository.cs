using FastFood.DB;
using FastFood.DB.Entities;
using FastFood.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FastFood.Repositories;

public class CustomerAccountRepository : CommonRepository, ICustomerAccountRepository
{
    public CustomerAccountRepository(FastFoodEntities fastFoodEntities) : base(fastFoodEntities) { }

    public async Task<List<CustomerAccount>> GetCustomerAccounts()
    {
        return await this._fastFoodEntities.CustomerAccounts.ToListAsync();
    }

    public async Task<CustomerAccount?> GetCustomerAccountByUserName(string userName)
    {
        return await this._fastFoodEntities.CustomerAccounts
            .FirstOrDefaultAsync(x => x.UserName.Equals(userName));
    }

    public async Task AddCustomerAccount(CustomerAccount customerAccount)
    {
        await this._fastFoodEntities.AddAsync(customerAccount);
        await this._fastFoodEntities.SaveChangesAsync();
    }
}
