using FastFood.DB;

namespace FastFood.Repositories.Interfaces
{
    public interface ILoggingEventRepository
    {
        Task<List<LoggingEvent>> GetLoggingEvents();
        Task<List<LoggingEvent>> GetLoggingEventsByUserType(bool userType);
        void NewLoggingEvent(bool userType, string userId, string userName, string eventDetail);
    }
}
