using FastFood.DB;

namespace FastFood.Repositories.Interfaces
{
    public interface IStoreInfoRepository
    {
        Task<StoreInfo> GetStoreInfo();
    }
}
