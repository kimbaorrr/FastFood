using Microsoft.AspNetCore.Mvc;

namespace FastFood.Models.ViewComponents
{
    /// <summary>
    /// ViewComponent dùng để hiển thị thông tin người dùng khách hàng sau khi đăng nhập.
    /// </summary>
    public class CustomerUserInfoViewComponent : ViewComponent
    {
        /// <summary>
        /// Hàm render giao diện thông tin khách hàng.
        /// </summary>
        /// <returns>
        /// </returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }

}
