using FastFood.DB;

namespace FastFood.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetEmployees();
        string GetFullName(int employeeId);
        Task<Employee> GetEmployeeById(int employeeId);
        Task AddEmployee(Employee employee);
        Task UpdateEmployee(Employee employee);
        Task DeleteEmployee(Employee employee);
        Task<List<Employee>> GetEmployeesNonAccount();
    }
}
