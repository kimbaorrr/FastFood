using FastFood.DB.Entities;

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
        Task<List<Order>> GetOrdersByOrderStatusId(int orderStatusId);
        Task<Order> GetOrderByOrderId(int orderId);
        Task AddOrder(Order order);
        Task UpdateOrder(Order order);
        Task DeleteOrder(Order order);

    }
}
