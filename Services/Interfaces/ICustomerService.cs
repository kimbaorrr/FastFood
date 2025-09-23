using FastFood.DB.Entities;
using FastFood.Models.ViewModels;
using X.PagedList;

namespace FastFood.Services.Interfaces
{
    /// <summary>
    /// Interface cho các dịch vụ quản lý khách hàng.
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// So sánh khách hàng dựa trên thời gian truy cập.
        /// </summary>
        /// <param name="currentTime">Thời gian hiện tại.</param>
        /// <param name="previousTime">Thời gian trước đó.</param>
        /// <returns>Giá trị so sánh (double).</returns>
        Task<double> CompareCustomersByDateTime(DateTime currentTime, DateTime previousTime);

        /// <summary>
        /// Lấy danh sách khách hàng tiềm năng.
        /// </summary>
        /// <returns>Danh sách khách hàng tiềm năng.</returns>
        Task<List<PotentialCustomersViewModel>> GetPotentialCustomers();

        /// <summary>
        /// Lấy danh sách khách hàng phân trang.
        /// </summary>
        /// <param name="page">Trang hiện tại.</param>
        /// <param name="size">Số lượng khách hàng mỗi trang.</param>
        /// <returns>Danh sách khách hàng phân trang.</returns>
        Task<IPagedList<Customer>> GetCustomersPagedList(int page, int size);

        /// <summary>
        /// Lấy danh sách khách hàng tiềm năng phân trang.
        /// </summary>
        /// <param name="page">Trang hiện tại.</param>
        /// <param name="size">Số lượng khách hàng mỗi trang.</param>
        /// <returns>Danh sách khách hàng tiềm năng phân trang.</returns>
        Task<IPagedList<PotentialCustomersViewModel>> GetPotentialCustomersPagedList(int page, int size);

        /// <summary>
        /// Lấy thông tin chi tiết khách hàng và danh sách đơn hàng phân trang.
        /// </summary>
        /// <param name="customerId">Mã khách hàng.</param>
        /// <param name="page">Trang hiện tại.</param>
        /// <param name="size">Số lượng đơn hàng mỗi trang.</param>
        /// <returns>Thông tin chi tiết khách hàng và danh sách đơn hàng phân trang.</returns>
        Task<(CustomerDetailViewModel, IPagedList<Order>)> GetCustomerDetailWithOrdersPagedList(int customerId, int page, int size);

        /// <summary>
        /// Kiểm tra đăng nhập khách hàng.
        /// </summary>
        /// <param name="customerLoginViewModel">Model đăng nhập khách hàng.</param>
        /// <returns>Kết quả đăng nhập, thông báo và thông tin khách hàng nếu thành công.</returns>
        Task<(bool, string, CustomerClaimInfoViewModel?)> LoginChecker(CustomerLoginViewModel customerLoginViewModel);

        /// <summary>
        /// Đăng ký tài khoản khách hàng mới.
        /// </summary>
        /// <param name="customerRegisterViewModel">Model đăng ký khách hàng.</param>
        /// <returns>Kết quả đăng ký và thông báo.</returns>
        Task<(bool, string)> Register(CustomerRegisterViewModel customerRegisterViewModel);
    }
}
