using FastFood.DB.Entities;

namespace FastFood.Repositories.Interfaces
{
    public interface IFeedbackRepository
    {
        Task AddFeedback(Feedback feedback);
    }
}
