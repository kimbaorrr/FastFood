using FastFood.DB;
using FastFood.DB.Entities;
using FastFood.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FastFood.Repositories;

public class PromoRepository : CommonRepository, IPromoRepository
{
    public PromoRepository(FastFoodEntities fastFoodEntities) : base(fastFoodEntities) { }

    public async Task<Promo> GetPromoByPromoCode(string promoCode)
    {
        return await this._fastFoodEntities.Promos
            .FirstOrDefaultAsync(x => x.PromoCode.Equals(promoCode))
            ?? new Promo();
    }
}
