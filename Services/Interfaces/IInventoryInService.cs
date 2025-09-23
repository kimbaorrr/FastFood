namespace FastFood.Services.Interfaces
{
    /// <summary>
    /// Dịch vụ quản lý nhập kho.
    /// </summary>
    public interface IInventoryInService
    {
        /// <summary>
        /// Đếm số lượng phiếu nhập kho trong khoảng thời gian chỉ định.
        /// </summary>
        /// <param name="fromDate">Ngày bắt đầu.</param>
        /// <param name="toDate">Ngày kết thúc.</param>
        /// <returns>Số lượng phiếu nhập kho.</returns>
        Task<int> CountInventoryInByDateTime(DateTime fromDate, DateTime toDate);
    }
}