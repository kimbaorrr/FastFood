using FastFood.DB.Entities;
using FastFood.Models.ViewModels;
using X.PagedList;

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
        Task<IPagedList<CustomEmployeePermissions>> GetEmployeesPermissionsPagedList(int page, int size);
        Task<(bool, string)> RegisterLoginAccount(EmployeeRegisterLoginAccountViewModel employeeRegisterViewModel, string selectedPermissions);
    }
}