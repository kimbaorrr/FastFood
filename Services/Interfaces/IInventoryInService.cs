namespace FastFood.Services.Interfaces
{
    public interface IInventoryInService
    {
        Task<int> CountInventoryInByDateTime(DateTime fromDate, DateTime toDate);
    }
}