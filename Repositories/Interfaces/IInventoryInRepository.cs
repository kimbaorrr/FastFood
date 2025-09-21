using FastFood.DB.Entities;

namespace FastFood.Repositories.Interfaces
{
    public interface IInventoryInRepository
    {
        Task<List<InventoryIn>> GetInventoryIns();
    }
}
