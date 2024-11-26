using System;
namespace FastFood.Models
{
    /// <summary>
    /// Lớp này cung cấp các phương thức tiện ích liên quan đến mật khẩu và xử lý chuỗi.
    /// </summary>
    public static class FastFood_Tools
    {
        /// <summary>
        /// Mã hóa mật khẩu bằng cách sử dụng thuật toán bcrypt.
        /// </summary>
        /// <param name="matKhau">Mật khẩu cần mã hóa.</param>
        /// <returns>Mật khẩu đã được mã hóa.</returns>
        public static string HashPassword(string matKhau)
        {
            return BCrypt.Net.BCrypt.HashPassword(matKhau);
        }

        /// <summary>
        /// Kiểm tra xem mật khẩu nhập vào có khớp với mật khẩu đã mã hóa hay không.
        /// </summary>
        /// <param name="matKhau">Mật khẩu cần kiểm tra.</param>
        /// <param name="matKhauDaBam">Mật khẩu đã mã hóa để so sánh.</param>
        /// <returns>Trả về true nếu mật khẩu khớp, ngược lại false.</returns>
        public static bool CheckPassword(string matKhau, string matKhauDaBam)
        {
            return BCrypt.Net.BCrypt.Verify(matKhau, matKhauDaBam);
        }

        /// <summary>
        /// Tách chuỗi hình ảnh thành một mảng các chuỗi dựa trên dấu phẩy.
        /// </summary>
        /// <param name="hinhAnh">Chuỗi chứa danh sách các hình ảnh, các hình ảnh cách nhau bằng dấu phẩy.</param>
        /// <returns>Mảng các chuỗi hình ảnh.</returns>
        public static string[] SplitAnh(string hinhAnh)
        {
            return (hinhAnh ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}