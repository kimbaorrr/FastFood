using FastFood.DB;

namespace FastFood.Repositories.Interfaces
{
    public interface IWorkScheduleRepository
    {
        Task<List<WorkSchedule>> GetWorkSchedules();
    }
}
