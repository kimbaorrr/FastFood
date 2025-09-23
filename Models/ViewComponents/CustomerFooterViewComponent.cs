using FastFood.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Models.ViewComponents
{
    /// <summary>
    /// ViewComponent hiển thị footer dành cho khách hàng, bao gồm thông tin cửa hàng và lịch làm việc.
    /// </summary>
    public class CustomerFooterViewComponent : ViewComponent
    {
        /// <summary>
        /// Repository lấy thông tin cửa hàng.
        /// </summary>
        private readonly IStoreInfoRepository _storeInfoRepository;

        /// <summary>
        /// Repository lấy lịch làm việc.
        /// </summary>
        private readonly IWorkScheduleRepository _workScheduleRepository;

        /// <summary>
        /// Hàm khởi tạo CustomerFooterViewComponent với các repository cần thiết.
        /// </summary>
        /// <param name="storeInfoRepository">Repository thông tin cửa hàng.</param>
        /// <param name="workScheduleRepository">Repository lịch làm việc.</param>
        public CustomerFooterViewComponent(IStoreInfoRepository storeInfoRepository, IWorkScheduleRepository workScheduleRepository)
        {
            _storeInfoRepository = storeInfoRepository;
            _workScheduleRepository = workScheduleRepository;
        }

        /// <summary>
        /// Phương thức bất đồng bộ để lấy thông tin cửa hàng và lịch làm việc, trả về View cho footer.
        /// </summary>
        /// <returns>Kết quả ViewComponent chứa thông tin cửa hàng và lịch làm việc.</returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var storeInfo = await this._storeInfoRepository.GetStoreInfo();
            var workSchedule = await this._workScheduleRepository.GetWorkSchedules();

            ViewBag.StoreInfo = storeInfo;
            ViewBag.WorkSchedule = workSchedule;

            return View();
        }
    }
}
