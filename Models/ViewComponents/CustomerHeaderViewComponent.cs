using FastFood.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Models.ViewComponents
{
    /// <summary>
    /// ViewComponent hiển thị header dành cho khách hàng, bao gồm thông tin cửa hàng.
    /// </summary>
    public class CustomerHeaderViewComponent : ViewComponent
    {
        /// <summary>
        /// Repository lấy thông tin cửa hàng.
        /// </summary>
        private readonly IStoreInfoRepository _storeInfoRepository;

        /// <summary>
        /// Hàm khởi tạo CustomerHeaderViewComponent với repository thông tin cửa hàng.
        /// </summary>
        /// <param name="storeInfoRepository">Repository thông tin cửa hàng.</param>
        public CustomerHeaderViewComponent(IStoreInfoRepository storeInfoRepository)
        {
            _storeInfoRepository = storeInfoRepository;
        }

        /// <summary>
        /// Phương thức bất đồng bộ để lấy thông tin cửa hàng và trả về View cho header.
        /// </summary>
        /// <returns>Kết quả ViewComponent chứa thông tin cửa hàng.</returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var storeInfo = await this._storeInfoRepository.GetStoreInfo();
            ViewBag.StoreInfo = storeInfo;
            return View();
        }
    }
}
