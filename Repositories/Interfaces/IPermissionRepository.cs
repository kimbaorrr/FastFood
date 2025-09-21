using FastFood.DB.Entities;

namespace FastFood.Repositories.Interfaces
{
    public interface IPermissionRepository
    {
        Task<List<Permission>> GetPermissions();
        Task<List<string>> GetDescriptionByIds(int[] permissionIds);
    }
}
