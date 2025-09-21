using FastFood.DB;
using FastFood.DB.Entities;
using FastFood.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FastFood.Repositories;

public class InventoryInRepository : CommonRepository, IInventoryInRepository
{
    public InventoryInRepository(FastFoodEntities fastFoodEntities) : base(fastFoodEntities) { }

    public async Task<List<InventoryIn>> GetInventoryIns()
    {
        return await this._fastFoodEntities.InventoryIns.ToListAsync();
    }
}
