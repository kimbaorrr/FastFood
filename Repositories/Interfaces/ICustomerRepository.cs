using FastFood.DB.Entities;

namespace FastFood.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<int> CountNewCustomers(DateTime fromTime, DateTime toTime);
        Task<List<Customer>> GetCustomers();
        string GetFullName(int customerId);
        Task<List<Customer>> GetCustomersByOrderStatus(int orderStatus);
        Task<Customer> GetCustomerById(int customerId);

    }
}
