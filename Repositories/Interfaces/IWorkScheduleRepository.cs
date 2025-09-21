using FastFood.DB.Entities;

namespace FastFood.Repositories.Interfaces
{
    public interface IWorkScheduleRepository
    {
        Task<List<WorkSchedule>> GetWorkSchedules();
    }
}
