using FastFood.DB.Entities;

namespace FastFood.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment> GetPaymentById(int paymentId);
        Task UpdatePayment(Payment payment);
    }
}
