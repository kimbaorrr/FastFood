using FastFood.DB.Entities;

namespace FastFood.Repositories.Interfaces
{
    public interface IPromoRepository
    {
        Task<Promo> GetPromoByPromoCode(string promoCode);
    }
}
