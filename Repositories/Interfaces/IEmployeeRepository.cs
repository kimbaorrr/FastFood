using FastFood.DB;

namespace FastFood.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetEmployees();
        string GetFullName(int employeeId);
    }
}
