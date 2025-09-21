using FastFood.DB;
using FastFood.DB.Entities;
using FastFood.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;

namespace FastFood.Repositories;
public class PaymentRepository : CommonRepository, IPaymentRepository
{    
    public PaymentRepository(FastFoodEntities fastFoodEntities) : base(fastFoodEntities)
    {
        
    }

    public async Task<Payment> GetPaymentById(int paymentId)
    {
        return await this._fastFoodEntities.Payments
            .FirstOrDefaultAsync(x => x.PaymentId == paymentId)
            ?? new Payment();
    }

    public async Task UpdatePayment(Payment payment)
    {
        await using var transaction = await this._fastFoodEntities.Database.BeginTransactionAsync();

        try
        {
            await this._fastFoodEntities.Payments.AddAsync(payment);
            await this._fastFoodEntities.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
