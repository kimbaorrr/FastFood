using FastFood.DB;
using FastFood.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FastFood.Repositories;

public class StoreInfoRepository : CommonRepository, IStoreInfoRepository
{
    public StoreInfoRepository(FastFoodEntities fastFoodEntities) : base(fastFoodEntities) { }

    public async Task<StoreInfo> GetStoreInfo()
    {
        return await this._fastFoodEntities.StoreInfos.SingleOrDefaultAsync();
    }
}
    
