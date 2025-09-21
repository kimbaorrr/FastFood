using FastFood.DB.Entities;
using FastFood.Models.ViewModels;
using FastFood.Repositories;
using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;

namespace FastFood.Services
{
    public class FeedbackService : CommonService, IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        public FeedbackService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task<(bool, string)> AddFeedback(CustomerSendFeedbackViewModel customerSendFeedbackViewModel)
        {
            Feedback feedback = new()
            {
                CustomerName = customerSendFeedbackViewModel.CustomerName,
                Content = customerSendFeedbackViewModel.Content,
                Email = customerSendFeedbackViewModel.Email,
                Phone = customerSendFeedbackViewModel.Phone,
            };
            await this._feedbackRepository.AddFeedback(feedback);
            return (true, $"Thêm feedback thành công !");
        }
    }
}
