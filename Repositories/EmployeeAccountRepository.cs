using FastFood.DB;
using FastFood.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FastFood.Repositories;

public class EmployeeAccountRepository : CommonRepository, IEmployeeAccountRepository
{
    public EmployeeAccountRepository(FastFoodEntities fastFoodEntities) : base(fastFoodEntities) { }

    public async Task<List<EmployeeAccount>> GetEmployeeAccounts()
    {
        return await this._fastFoodEntities.EmployeeAccounts.ToListAsync();
    }

    public async Task<EmployeeAccount> GetEmployeeAccountByUserName(string userName)
    {
        return await this._fastFoodEntities.EmployeeAccounts
            .Where(x => x.UserName.Equals(userName))
            .FirstOrDefaultAsync() ?? new EmployeeAccount();
    }

    public async Task<EmployeeAccount> GetEmployeeAccountById(int employeeId)
    {
        return await this._fastFoodEntities.EmployeeAccounts
            .Where(x => x.EmployeeId == employeeId)
            .FirstOrDefaultAsync() ?? new EmployeeAccount();
    }

    public async Task AddEmployeeAccount(EmployeeAccount employeeAccount)
    {
        await this._fastFoodEntities.EmployeeAccounts.AddAsync(employeeAccount);
        await this._fastFoodEntities.SaveChangesAsync();
    }

    public async Task UpdateEmployeeAccount(EmployeeAccount employeeAccount)
    {
        this._fastFoodEntities.EmployeeAccounts.Update(employeeAccount);
        await this._fastFoodEntities.SaveChangesAsync();
    }

    public async Task DeleteEmployeeAccount(EmployeeAccount employeeAccount)
    {
        this._fastFoodEntities.EmployeeAccounts.Remove(employeeAccount);
        await this._fastFoodEntities.SaveChangesAsync();
    }

    public async Task UpdateNewPassword(int employeeId, string newPassword, bool isTempPassword)
    {
        var employeeAccount = await this.GetEmployeeAccountById(employeeId);
        employeeAccount.Password = newPassword;
        employeeAccount.TemporaryPassword = isTempPassword;
        this._fastFoodEntities.EmployeeAccounts.Update(employeeAccount);
        await this._fastFoodEntities.SaveChangesAsync();
    }
}
