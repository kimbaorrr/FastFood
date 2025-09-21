using FastFood.DB;
using FastFood.DB.Entities;
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

    public string GetFullName(int? employeeId)
    {
        return this._fastFoodEntities.Employees
            .Where(x => x.EmployeeId == employeeId)
            .Select(x => x.LastName + " " + x.FirstName)
            .FirstOrDefault() ?? string.Empty;
    }

    public async Task<Employee> GetEmployeeById(int? employeeId)
    {
        return await this._fastFoodEntities.Employees
            .FirstOrDefaultAsync(x => x.EmployeeId == employeeId) 
            ?? new Employee();
    }

    public async Task AddEmployee(Employee employee)
    {
        await this._fastFoodEntities.AddAsync(employee);
        await this._fastFoodEntities.SaveChangesAsync();
    }

    public async Task UpdateEmployee(Employee employee)
    {
        this._fastFoodEntities.Update(employee);
        await this._fastFoodEntities.SaveChangesAsync();
    }

    public async Task DeleteEmployee(Employee employee)
    {
        this._fastFoodEntities.Remove(employee);
        await this._fastFoodEntities.SaveChangesAsync();
    }

    public async Task<List<Employee>> GetEmployeesWithAccountStatus(bool isHasAccount)
    {
        return await this._fastFoodEntities.Employees
            .Where(x => isHasAccount ? x.EmployeeAccount != null : x.EmployeeAccount == null)
            .ToListAsync();
    }
}
