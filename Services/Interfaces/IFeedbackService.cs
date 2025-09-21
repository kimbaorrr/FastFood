using FastFood.Models.ViewModels;

namespace FastFood.Services.Interfaces
{
    public interface IFeedbackService
    {
        Task<(bool, string)> AddFeedback(CustomerSendFeedbackViewModel customerSendFeedbackViewModel);
    }
}
