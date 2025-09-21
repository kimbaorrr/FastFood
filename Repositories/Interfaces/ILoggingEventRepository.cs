using FastFood.DB.Entities;

namespace FastFood.Repositories.Interfaces
{
    public interface ILoggingEventRepository
    {
        Task<List<LoggingEvent>> GetLoggingEvents();
        Task<List<LoggingEvent>> GetLoggingEventsByUserType(bool userType);
        Task NewLoggingEvent(bool userType, string userId, string userName, string eventDetail);
    }
}
