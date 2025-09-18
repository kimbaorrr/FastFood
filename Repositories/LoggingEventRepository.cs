using FastFood.DB;
using FastFood.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FastFood.Repositories;
public class LoggingEventRepository : CommonRepository, ILoggingEventRepository
{
    public LoggingEventRepository(FastFoodEntities fastFoodEntities) : base(fastFoodEntities) { }

    public async Task<List<LoggingEvent>> GetLoggingEvents()
    {
        return await this._fastFoodEntities.LoggingEvents.ToListAsync();
    }

    public async Task<List<LoggingEvent>> GetLoggingEventsByUserType(bool userType)
    {
        return await this._fastFoodEntities.LoggingEvents.Where(x=>x.UserType == userType).ToListAsync();
    }

    public async Task NewLoggingEvent(bool userType, string userId, string userName, string eventDetail)
    {

        LoggingEvent le = new()
        {
            UserId = Convert.ToInt32(userId),
            UserType = userType,
            UserName = userName,
            AccessedTime = DateTime.Now,
            EventDetail = eventDetail
        };
        await this._fastFoodEntities.LoggingEvents.AddAsync(le);
        await this._fastFoodEntities.SaveChangesAsync();
    }
}
