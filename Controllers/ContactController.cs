using FastFood.DB;
using FastFood.Models;
using FastFood.Models.ViewModels;
using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Controllers
{
    /// <summary>
    /// Controller xử lý các chức năng liên hệ và phản hồi của khách hàng.
    /// </summary>
    [Route("contact")]
    public class ContactController : BaseCustomerController
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IStoreInfoRepository _storeInfoRepository;

        /// <summary>
        /// Khởi tạo controller liên hệ với các dịch vụ cần thiết.
        /// </summary>
        /// <param name="feedbackService">Dịch vụ phản hồi khách hàng.</param>
        /// <param name="storeInfoRepository">Kho lưu trữ thông tin cửa hàng.</param>
        public ContactController(IFeedbackService feedbackService, IStoreInfoRepository storeInfoRepository)
        {
            _feedbackService = feedbackService;
            _storeInfoRepository = storeInfoRepository;
        }

        /// <summary>
        /// Hiển thị trang liên hệ và thông tin cửa hàng.
        /// </summary>
        /// <returns>Trang liên hệ.</returns>
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Liên hệ FastFood";
            var storeInfo = await this._storeInfoRepository.GetStoreInfo();
            ViewBag.StoreInfo = storeInfo;
            return View();
        }

        /// <summary>
        /// Gửi phản hồi của khách hàng.
        /// </summary>
        /// <param name="customerSendFeedbackViewModel">Thông tin phản hồi của khách hàng.</param>
        /// <returns>Kết quả gửi phản hồi dưới dạng JSON.</returns>
        [HttpPost("feedback/send")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> SendFeedback(CustomerSendFeedbackViewModel customerSendFeedbackViewModel)
        {
            (bool success, string message) = await this._feedbackService.AddFeedback(customerSendFeedbackViewModel);
            return CreateJsonResult(success, message);
        }

    }
}