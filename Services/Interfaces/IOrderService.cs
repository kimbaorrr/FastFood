using FastFood.DB.Entities;
using FastFood.Models.ViewModels;
using X.PagedList;

namespace FastFood.Services.Interfaces
{
    /// <summary>
    /// Dịch vụ quản lý đơn hàng.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Đếm số lượng đơn hàng trong khoảng thời gian chỉ định.
        /// </summary>
        /// <param name="fromDate">Ngày bắt đầu.</param>
        /// <param name="toDate">Ngày kết thúc.</param>
        /// <returns>Số lượng đơn hàng.</returns>
        Task<int> CountOrderByDate(DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Lấy tổng doanh thu trong khoảng thời gian chỉ định.
        /// </summary>
        /// <param name="fromDate">Ngày bắt đầu.</param>
        /// <param name="toDate">Ngày kết thúc.</param>
        /// <returns>Tổng doanh thu.</returns>
        Task<int> GetRevenue(DateTime fromDate, DateTime toDate);

        /// <summary>
        /// So sánh doanh thu giữa hai ngày theo phần trăm.
        /// </summary>
        /// <param name="currentDate">Ngày hiện tại.</param>
        /// <param name="previousDate">Ngày trước đó.</param>
        /// <returns>Tỷ lệ phần trăm thay đổi doanh thu.</returns>
        Task<double> CompareRevenueByPercentage(DateTime currentDate, DateTime previousDate);

        /// <summary>
        /// So sánh số lượng đơn hàng giữa hai ngày theo phần trăm.
        /// </summary>
        /// <param name="currentDate">Ngày hiện tại.</param>
        /// <param name="previousDate">Ngày trước đó.</param>
        /// <returns>Tỷ lệ phần trăm thay đổi số lượng đơn hàng.</returns>
        Task<double> CompareOrderCountByPercentage(DateTime currentDate, DateTime previousDate);

        /// <summary>
        /// Lấy doanh thu theo khoảng thời gian.
        /// </summary>
        /// <param name="timeRange">Khoảng thời gian (ví dụ: tuần, tháng).</param>
        /// <returns>Doanh thu dưới dạng chuỗi.</returns>
        Task<string> GetRevenueByTimeRange(string timeRange);

        /// <summary>
        /// Lấy danh sách đơn hàng theo ngày.
        /// </summary>
        /// <param name="fromTime">Thời gian bắt đầu.</param>
        /// <param name="toTime">Thời gian kết thúc.</param>
        /// <returns>Danh sách đơn hàng dưới dạng chuỗi.</returns>
        Task<string> GetOrdersByDate(DateTime fromTime, DateTime toTime);

        /// <summary>
        /// Lấy danh sách đơn hàng tuỳ chỉnh.
        /// </summary>
        /// <returns>Danh sách đơn hàng tuỳ chỉnh.</returns>
        Task<List<CustomOrderViewModel>> GetCustomOrderViewModels();

        /// <summary>
        /// Lấy danh sách đơn hàng phân trang.
        /// </summary>
        /// <param name="page">Trang hiện tại.</param>
        /// <param name="size">Số lượng trên mỗi trang.</param>
        /// <returns>Danh sách đơn hàng phân trang.</returns>
        Task<IPagedList<Order>> GetOrdersPagedList(int page, int size);

        /// <summary>
        /// Lấy thống kê trạng thái đơn hàng.
        /// </summary>
        /// <returns>Danh sách trạng thái đơn hàng và số lượng tương ứng.</returns>
        Task<SortedDictionary<string, int>> GetOrdersStatusCard();

        /// <summary>
        /// Lấy danh sách đơn hàng theo trạng thái và phân trang.
        /// </summary>
        /// <param name="orderStatusId">ID trạng thái đơn hàng.</param>
        /// <param name="page">Trang hiện tại.</param>
        /// <param name="size">Số lượng trên mỗi trang.</param>
        /// <returns>Danh sách đơn hàng phân trang theo trạng thái.</returns>
        Task<IPagedList<Order>> GetOrdersByOrderStatusPagedList(int orderStatusId, int page, int size);

        /// <summary>
        /// Lấy chi tiết đơn hàng.
        /// </summary>
        /// <param name="orderId">ID đơn hàng.</param>
        /// <returns>Chi tiết đơn hàng dưới dạng ViewModel.</returns>
        Task<CustomOrderDetailViewModel> GetOrderDetailViewModel(int orderId);

        /// <summary>
        /// Cập nhật trạng thái đơn hàng.
        /// </summary>
        /// <param name="orderId">ID đơn hàng.</param>
        /// <param name="orderStatus">Trạng thái mới của đơn hàng.</param>
        /// <returns>Tuple gồm kết quả cập nhật và thông báo.</returns>
        Task<(bool, string)> UpdateOrderStatus(int orderId, int orderStatus);

        /// <summary>
        /// Thêm thông tin vận chuyển cho đơn hàng.
        /// </summary>
        /// <param name="newShippingInfo">Thông tin vận chuyển mới.</param>
        /// <returns>Tuple gồm kết quả thêm và thông báo.</returns>
        Task<(bool, string)> AddShippingInfo(NewShippingInfo newShippingInfo);

        /// <summary>
        /// Kiểm tra có thể thêm thông tin vận chuyển cho đơn hàng hay không.
        /// </summary>
        /// <param name="orderId">ID đơn hàng.</param>
        /// <returns>Tuple gồm kết quả kiểm tra và thông báo.</returns>
        Task<(bool, string)> IsAddShippingInfo(int orderId);
    }
}
