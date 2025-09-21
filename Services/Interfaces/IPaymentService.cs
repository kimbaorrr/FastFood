using FastFood.Models;
using FastFood.Models.ViewModels;

namespace FastFood.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<AddPaymentViewModel> AddPaymentViewModel(PaymentSummaryViewModel? paymentSummaryViewModel, int customerId);

        Task<string> AddPaymentViewPostModel(AddPaymentViewModel addPaymentViewModel, Dictionary<int, CustomerCartViewModel> customerCartViewModel, int customerId, string ipAddress);

        Task<(bool, string, PaymentResultViewModel)> GetPaymentResultViewModel(IQueryCollection queryString);
    }
}
