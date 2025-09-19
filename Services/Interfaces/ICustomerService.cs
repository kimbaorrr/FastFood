using FastFood.DB;
using FastFood.Models.ViewModels;
using X.PagedList;

namespace FastFood.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<double> CompareCustomersByDateTime(DateTime currentTime, DateTime previousTime);
        Task<List<PotentialCustomersViewModel>> GetPotentialCustomers();
        Task<IPagedList<Customer>> GetCustomersPagedList(int page, int size);
        Task<IPagedList<PotentialCustomersViewModel>> GetPotentialCustomersPagedList(int page, int size);
        Task<(CustomerDetailViewModel, IPagedList<Order>)> GetCustomerDetailWithOrdersPagedList(int customerId, int page, int size);

    }
}
