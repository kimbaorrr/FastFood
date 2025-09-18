using FastFood.Models.ViewModels;

namespace FastFood.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<double> CompareCustomersByDateTime(DateTime currentTime, DateTime previousTime);
        Task<List<PotentialCustomersViewModel>> GetPotentialCustomers();

    }
}
