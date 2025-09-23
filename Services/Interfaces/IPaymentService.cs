using FastFood.Models;
using FastFood.Models.ViewModels;

namespace FastFood.Services.Interfaces
{
    /// <summary>
    /// Interface cho các dịch vụ liên quan đến thanh toán.
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Tạo model hiển thị thông tin thanh toán cho khách hàng.
        /// </summary>
        /// <param name="paymentSummaryViewModel">Model tóm tắt thanh toán.</param>
        /// <param name="customerId">Mã khách hàng.</param>
        /// <returns>Model thêm thanh toán.</returns>
        Task<AddPaymentViewModel> AddPaymentViewModel(PaymentSummaryViewModel? paymentSummaryViewModel, int customerId);

        /// <summary>
        /// Xử lý dữ liệu khi khách hàng gửi thông tin thanh toán.
        /// </summary>
        /// <param name="addPaymentViewModel">Model thêm thanh toán.</param>
        /// <param name="customerCartViewModel">Giỏ hàng của khách hàng.</param>
        /// <param name="customerId">Mã khách hàng.</param>
        /// <param name="ipAddress">Địa chỉ IP của khách hàng.</param>
        /// <param name="callbackUrl">URL callback sau khi thanh toán.</param>
        /// <returns>URL thanh toán hoặc thông báo lỗi.</returns>
        Task<string> AddPaymentViewPostModel(AddPaymentViewModel addPaymentViewModel, Dictionary<int, CustomerCartViewModel> customerCartViewModel, int customerId, string ipAddress, string callbackUrl);

        /// <summary>
        /// Lấy kết quả thanh toán từ dữ liệu trả về của cổng thanh toán.
        /// </summary>
        /// <param name="queryString">Query string từ cổng thanh toán.</param>
        /// <returns>Kết quả thanh toán và thông báo.</returns>
        Task<(bool, string, PaymentResultViewModel)> GetPaymentResultViewModel(IQueryCollection queryString);
    }
}
