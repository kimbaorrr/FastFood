using FastFood.Models.ViewModels;

namespace FastFood.Services.Interfaces
{
    public interface ICartService
    {
        Dictionary<int, CustomerCartViewModel> CustomerCartViewModel { get; set; }
        bool IsCartEmpty();
        Task AddItem(int productId, int quantity);
        void RemoveItem(int productId);
        void DecreaseQuantity(int productId);
        int TotalPay();
        Task<(bool, string, string)> GetSummaryCheckout(string promoCode);

    }
}
