namespace FastFood.Services.Interfaces
{
    public interface ICartService
    {
        bool IsCartEmpty();
        Task AddItem(int productId, int quantity);
        void RemoveItem(int productId);
        void DecreaseQuantity(int productId);
        int TotalPay();

    }
}
