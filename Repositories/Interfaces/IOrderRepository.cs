using FastFood.DB;

namespace FastFood.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrders();
        Task<List<Order>> GetOrdersDelivered();
        Task<int> CountOrders();
        Task<int> CalculateTotalOrdersRevenue();
        Task<double> CalculateAverageOrdersRevenue();
        Task<List<Order>> GetOrdersWithDetails();

    }
}
