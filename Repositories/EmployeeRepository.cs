using FastFood.DB;
using FastFood.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FastFood.Repositories;

public class EmployeeRepository : CommonRepository, IEmployeeRepository
{
    public EmployeeRepository(FastFoodEntities fastFoodEntities) : base(fastFoodEntities) { }

    public async Task<List<Employee>> GetEmployees()
    {
        return await this._fastFoodEntities.Employees.ToListAsync();
    }

    public string GetFullName(int employeeId)
    {
        return this._fastFoodEntities.Employees
            .Where(x => x.EmployeeId == employeeId)
            .Select(x => x.LastName + " " + x.FirstName)
            .FirstOrDefault() ?? string.Empty;
    }
}
