using FastFood.DB;
using FastFood.Models;
using FastFood.Models.ViewModels;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Areas.Controllers
{
    [Route("contact")]
    public class ContactController : BaseController
    {
        private readonly IFeedbackService _feedbackService;

        public ContactController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.Title = "Liên hệ FastFood";
            return View();
        }

        [HttpPost("feedback/send")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> SendFeedback(CustomerSendFeedbackViewModel customerSendFeedbackViewModel)
        {
            (bool success, string message) = await this._feedbackService.AddFeedback(customerSendFeedbackViewModel);
            return CreateJsonResult(success, message);
        }

    }
}