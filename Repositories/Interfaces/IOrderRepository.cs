using FastFood.DB;

namespace FastFood.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrders();
        Task<List<Order>> GetOrdersDelivered();
        Task<int> CountOrders();
        Task<int> GetTotalOrdersRevenue();
        Task<double> GetAverageOrdersRevenue();
        Task<List<Order>> GetOrdersWithDetails();
        Task<List<Order>> GetOrdersByCustomerId(int customerId);

    }
}
