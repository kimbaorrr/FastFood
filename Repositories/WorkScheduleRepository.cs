using FastFood.DB;
using FastFood.DB.Entities;
using FastFood.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FastFood.Repositories;

public class WorkScheduleRepository : CommonRepository, IWorkScheduleRepository
{
    public WorkScheduleRepository(FastFoodEntities fastFoodEntities) : base(fastFoodEntities) { }

    public async Task<List<WorkSchedule>> GetWorkSchedules()
    {
        return await this._fastFoodEntities.WorkSchedules.ToListAsync();
    }

}
