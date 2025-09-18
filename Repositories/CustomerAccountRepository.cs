using FastFood.DB;
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
}
