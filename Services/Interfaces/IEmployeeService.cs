using FastFood.DB;
using FastFood.Models.ViewModels;

namespace FastFood.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<bool> CheckPermission(string employeeId, int permissonId);
        Task<bool> CheckRole(string employeeId);
        Task<SortedList<string, string>> CustomListPermissions(string permissionIds);
        Task<(bool, Employee?)> LoginChecker(EmployeeLoginViewModel employeeLoginViewModel);
        Task<(bool, string)> ChangePassword(EmployeeChangePasswordViewModel employeeChangePasswordViewModel);
        Task<(bool, string)> ForgotPassword(EmployeeForgotPasswordViewModel employeeForgotPasswordViewModel);
    }
}