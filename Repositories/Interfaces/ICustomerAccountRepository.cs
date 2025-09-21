using FastFood.DB.Entities;

namespace FastFood.Repositories.Interfaces
{
    public interface ICustomerAccountRepository
    {
        Task<List<CustomerAccount>> GetCustomerAccounts();
        Task<CustomerAccount> GetCustomerAccountByUserName(string userName);
        Task AddCustomerAccount(CustomerAccount customerAccount);
    }
}
