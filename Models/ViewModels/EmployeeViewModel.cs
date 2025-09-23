using FastFood.DB;
using System.ComponentModel.DataAnnotations;

namespace FastFood.Models.ViewModels
{
    /// <summary>
    /// ViewModel đại diện cho thông tin nhân viên.
    /// </summary>
    public class EmployeeViewModel
    {
        /// <summary>
        /// Mã nhân viên.
        /// </summary>
        [DataType(DataType.Text)]
        [Display(Name = "Mã nhân viên")]
        public int EmployeeId { get; set; }

        /// <summary>
        /// Họ và đệm của nhân viên.
        /// </summary>
        [DataType(DataType.Text)]
        [Display(Name = "Họ đệm")]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Tên nhân viên.
        /// </summary>
        [DataType(DataType.Text)]
        [Display(Name = "Tên nhân viên")]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Địa chỉ email của nhân viên.
        /// </summary>
        [DataType(DataType.Text)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Số điện thoại của nhân viên.
        /// </summary>
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Số điện thoại")]
        [MinLength(10)]
        [MaxLength(12)]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Địa chỉ của nhân viên.
        /// </summary>
        [DataType(DataType.Text)]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Đường dẫn đến ảnh đại diện của nhân viên.
        /// </summary>
        [DataType(DataType.Text)]
        [Display(Name = "Ảnh đại diện")]
        public string Avatar { get; set; } = string.Empty;

    }

    /// <summary>
    /// ViewModel mở rộng cho quyền tuỳ chỉnh của nhân viên.
    /// </summary>
    public class CustomEmployeePermissions : EmployeeViewModel
    {
        /// <summary>
        /// Tên đăng nhập của nhân viên.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Vai trò của nhân viên (quyền truy cập).
        /// </summary>
        public bool Role { get; set; } = false;
    }
}
