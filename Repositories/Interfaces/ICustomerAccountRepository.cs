using FastFood.DB;

namespace FastFood.Repositories.Interfaces
{
    public interface ICustomerAccountRepository
    {
        Task<List<CustomerAccount>> GetCustomerAccounts();
    }
}
