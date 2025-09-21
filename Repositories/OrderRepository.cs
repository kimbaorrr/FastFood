using FastFood.DB;
using FastFood.DB.Entities;
using FastFood.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
namespace FastFood.Repositories;

public class OrderRepository : CommonRepository, IOrderRepository
{
    public OrderRepository(FastFoodEntities fastFoodEntities) : base(fastFoodEntities) { }

    public async Task<List<Order>> GetOrders()
    {
        return await this._fastFoodEntities.Orders.ToListAsync();
    }
    public async Task<List<Order>> GetOrdersByCustomerId(int customerId)
    {
        return await this._fastFoodEntities.Orders.Where(x=>x.Buyer == customerId).ToListAsync();
    }

    public async Task<List<Order>> GetOrdersWithDetails()
    {
        return await this._fastFoodEntities.Orders.Include(x => x.OrderDetails).ToListAsync();
    }

    public async Task<List<Order>> GetOrdersDelivered()
    {
        return await this._fastFoodEntities.Orders.Where(x => x.OrderStatus == 7).ToListAsync();
    }

    public async Task<int> CountOrders()
    {
        return await this._fastFoodEntities.Orders.CountAsync();
    }

    public async Task<int> GetTotalOrdersRevenue()
    {
        return await this._fastFoodEntities.Orders.SumAsync(x => x.TotalPrice);
    }

    public async Task<double> GetAverageOrdersRevenue()
    {
        return await this._fastFoodEntities.Orders.AverageAsync(x => x.TotalPrice);
    }

    public async Task<List<Order>> GetOrdersByOrderStatusId(int orderStatusId)
    {
        return await this._fastFoodEntities.Orders.Where(x=>x.OrderStatus == orderStatusId).ToListAsync();
    }

    public async Task<Order> GetOrderByOrderId(int orderId)
    {
        return await this._fastFoodEntities.Orders
            .FirstOrDefaultAsync(x => x.OrderId == orderId) 
            ?? new Order();
    }


    public async Task AddOrder(Order order)
    {
        await using var dbContextTransaction = await this._fastFoodEntities.Database.BeginTransactionAsync();

        try
        {
            await this._fastFoodEntities.Orders.AddAsync(order);
            await this._fastFoodEntities.SaveChangesAsync();

            await dbContextTransaction.CommitAsync();
        }
        catch (Exception)
        {
            await dbContextTransaction.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateOrder(Order order)
    {
        this._fastFoodEntities.Orders.UpdateRange(order);
        await this._fastFoodEntities.SaveChangesAsync();
    }

    public async Task DeleteOrder(Order order)
    {
        this._fastFoodEntities.Orders.RemoveRange(order);
        await this._fastFoodEntities.SaveChangesAsync();
    }




}
