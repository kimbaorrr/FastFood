namespace FastFood.Services.Interfaces
{
    /// <summary>
    /// Dịch vụ gửi email.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Gửi email đến các địa chỉ nhận.
        /// </summary>
        /// <param name="receivers">Danh sách địa chỉ email nhận, phân tách bằng dấu phẩy.</param>
        /// <param name="subject">Tiêu đề email.</param>
        /// <param name="body">Nội dung email.</param>
        /// <returns>Task đại diện cho quá trình gửi email.</returns>
        Task SendEmail(string receivers, string subject, string body);
    }
}