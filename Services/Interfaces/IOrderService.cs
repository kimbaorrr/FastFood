using FastFood.Models.ViewModels;

namespace FastFood.Services.Interfaces
{
    public interface IOrderService
    {
        Task<int> CountOrderByDate(DateTime fromDate, DateTime toDate);
        Task<int> GetRevenue(DateTime fromDate, DateTime toDate);
        Task<double> CompareRevenueByPercentage(DateTime currentDate, DateTime previousDate);
        Task<double> CompareOrderCountByPercentage(DateTime currentDate, DateTime previousDate);
        Task<string> GetRevenueByTimeRange(string timeRange);
        Task<string> GetOrdersByDate(DateTime fromTime, DateTime toTime);
        Task<List<CustomOrderViewModel>> GetCustomOrderViewModels();
    }
}
