using FastFood.DB.Entities;
using FastFood.Models.ViewModels;
using X.PagedList;

namespace FastFood.Services.Interfaces
{
    /// <summary>
    /// Dịch vụ quản lý nhân viên.
    /// </summary>
    public interface IEmployeeService
    {
        /// <summary>
        /// Kiểm tra quyền của nhân viên.
        /// </summary>
        /// <param name="employeeId">Mã nhân viên.</param>
        /// <param name="permissonId">Mã quyền cần kiểm tra.</param>
        /// <returns>True nếu nhân viên có quyền, ngược lại là False.</returns>
        Task<bool> CheckPermission(string employeeId, int permissonId);

        /// <summary>
        /// Kiểm tra vai trò của nhân viên.
        /// </summary>
        /// <param name="employeeId">Mã nhân viên.</param>
        /// <returns>True nếu nhân viên có vai trò, ngược lại là False.</returns>
        Task<bool> CheckRole(string employeeId);

        /// <summary>
        /// Tùy chỉnh danh sách quyền từ chuỗi mã quyền.
        /// </summary>
        /// <param name="permissionIds">Chuỗi các mã quyền, phân tách bằng dấu phẩy.</param>
        /// <returns>Danh sách quyền đã được tùy chỉnh.</returns>
        Task<SortedList<string, string>> CustomListPermissions(string permissionIds);

        /// <summary>
        /// Kiểm tra đăng nhập của nhân viên.
        /// </summary>
        /// <param name="employeeLoginViewModel">Thông tin đăng nhập của nhân viên.</param>
        /// <returns>Tuple gồm kết quả đăng nhập và thông tin nhân viên (nếu thành công).</returns>
        Task<(bool, string, EmployeeClaimInfoViewModel?)> LoginChecker(EmployeeLoginViewModel employeeLoginViewModel);

        /// <summary>
        /// Đổi mật khẩu cho nhân viên.
        /// </summary>
        /// <param name="employeeChangePasswordViewModel">Thông tin đổi mật khẩu.</param>
        /// <returns>Tuple gồm kết quả đổi mật khẩu và thông báo.</returns>
        Task<(bool, string)> ChangePassword(EmployeeChangePasswordViewModel employeeChangePasswordViewModel);

        /// <summary>
        /// Quên mật khẩu cho nhân viên.
        /// </summary>
        /// <param name="employeeForgotPasswordViewModel">Thông tin quên mật khẩu.</param>
        /// <returns>Tuple gồm kết quả và thông báo.</returns>
        Task<(bool, string)> ForgotPassword(EmployeeForgotPasswordViewModel employeeForgotPasswordViewModel);

        /// <summary>
        /// Lấy danh sách phân trang quyền của nhân viên.
        /// </summary>
        /// <param name="page">Trang hiện tại.</param>
        /// <param name="size">Số lượng trên mỗi trang.</param>
        /// <returns>Danh sách phân trang quyền của nhân viên.</returns>
        Task<IPagedList<CustomEmployeePermissions>> GetEmployeesPermissionsPagedList(int page, int size);

        /// <summary>
        /// Đăng ký tài khoản đăng nhập cho nhân viên.
        /// </summary>
        /// <param name="employeeRegisterViewModel">Thông tin đăng ký tài khoản.</param>
        /// <param name="selectedPermissions">Chuỗi quyền được chọn.</param>
        /// <returns>Tuple gồm kết quả đăng ký và thông báo.</returns>
        Task<(bool, string)> RegisterLoginAccount(EmployeeRegisterLoginAccountViewModel employeeRegisterViewModel, string selectedPermissions);
    }
}