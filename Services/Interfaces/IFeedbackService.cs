using FastFood.Models.ViewModels;

namespace FastFood.Services.Interfaces
{
    /// <summary>
    /// Interface cho các dịch vụ liên quan đến phản hồi khách hàng.
    /// </summary>
    public interface IFeedbackService
    {
        /// <summary>
        /// Thêm phản hồi từ khách hàng.
        /// </summary>
        /// <param name="customerSendFeedbackViewModel">Thông tin phản hồi của khách hàng.</param>
        /// <returns>Kết quả thêm phản hồi (thành công/thất bại và thông báo).</returns>
        Task<(bool, string)> AddFeedback(CustomerSendFeedbackViewModel customerSendFeedbackViewModel);
    }
}
