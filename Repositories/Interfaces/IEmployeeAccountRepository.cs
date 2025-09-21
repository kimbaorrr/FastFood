using FastFood.DB.Entities;

namespace FastFood.Repositories.Interfaces
{
    public interface IEmployeeAccountRepository
    {
        Task<List<EmployeeAccount>> GetEmployeeAccounts();
        Task<EmployeeAccount> GetEmployeeAccountByUserName(string userName);
        Task UpdateNewPassword(int employeeId, string newPassword, bool isTempPassword);
        Task<EmployeeAccount> GetEmployeeAccountById(int employeeId);
        Task AddEmployeeAccount(EmployeeAccount employeeAccount);
        Task UpdateEmployeeAccount(EmployeeAccount employeeAccount);
        Task DeleteEmployeeAccount(EmployeeAccount employeeAccount);

    }
}
