using FastFood.DB;
using FastFood.DB.Entities;
using FastFood.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FastFood.Repositories;

public class PermissionRepository : CommonRepository, IPermissionRepository
{
    public PermissionRepository(FastFoodEntities fastFoodEntities) : base(fastFoodEntities)
    {

    }

    public async Task<List<Permission>> GetPermissions()
    {
        return await this._fastFoodEntities.Permissions.ToListAsync();
    }

    public async Task<List<string>> GetDescriptionByIds(int[] permissionIds)
    {
        return await this._fastFoodEntities.Permissions
            .Where(x=> permissionIds.Contains(x.PermissionId))
            .Select(x=>x.Description)
            .ToListAsync();
    }

}
