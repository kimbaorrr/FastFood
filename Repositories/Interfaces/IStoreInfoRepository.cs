using FastFood.DB.Entities;

namespace FastFood.Repositories.Interfaces
{
    public interface IStoreInfoRepository
    {
        Task<StoreInfo> GetStoreInfo();
    }
}
