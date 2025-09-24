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
        /// <param name="currentDate">Ngày hiện tại.</param>
        /// <param name="previousDate">Ngày trước đó.</param>
        /// <returns>Số lượng phiếu nhập kho.</returns>
        Task<int> CountInventoryInByDateTime(DateTime currentDate, DateTime previousDate);
    }
}