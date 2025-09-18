using FastFood.DB;

namespace FastFood.Repositories.Interfaces
{
    public interface IInventoryInRepository
    {
        Task<List<InventoryIn>> GetInventoryIns();
    }
}
