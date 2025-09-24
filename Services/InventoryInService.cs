using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;

namespace FastFood.Services
{
    public class InventoryInService : CommonService, IInventoryInService
    {
        private readonly IInventoryInRepository _inventoryInRepository;
        public InventoryInService(IInventoryInRepository inventoryInRepository)
        {
            _inventoryInRepository = inventoryInRepository;
        }

        public async Task<int> CountInventoryInByDateTime(DateTime currentDate, DateTime previousDate)
        {
            var inventoryIns = await this._inventoryInRepository.GetInventoryIns();

            return inventoryIns
                .Count(nk => nk.CreatedAt >= previousDate && nk.CreatedAt < currentDate);
        }
    }
}
