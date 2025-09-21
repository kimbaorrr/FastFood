using FastFood.DB.Entities;
using FastFood.Models.ViewModels;
using X.PagedList;

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
        Task<IPagedList<Order>> GetOrdersPagedList(int page, int size);
        Task<SortedDictionary<string, int>> GetOrdersStatusCard();
        Task<IPagedList<Order>> GetOrdersByOrderStatusPagedList(int orderStatusId, int page, int size);
        Task<CustomOrderDetailViewModel> GetOrderDetailViewModel(int orderId);
        Task<(bool, string)> UpdateOrderStatus(int orderId, int orderStatus);
        Task<(bool, string)> AddShippingInfo(NewShippingInfo newShippingInfo);
        Task<(bool, string)> IsAddShippingInfo(int orderId);
    }
}
