using FastFood.DB;
using FastFood.DB.Entities;
using FastFood.Repositories.Interfaces;
using System;
using System.Collections.Generic;

namespace FastFood.Repositories;

public class FeedbackRepository : CommonRepository, IFeedbackRepository
{
    public FeedbackRepository(FastFoodEntities fastFoodEntities) : base(fastFoodEntities) { }
    public async Task AddFeedback(Feedback feedback)
    {
        await this._fastFoodEntities.AddAsync(feedback);
        await this._fastFoodEntities.SaveChangesAsync();
    }
}
