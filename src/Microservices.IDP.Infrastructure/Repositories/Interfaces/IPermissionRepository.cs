using Microservices.IDP.Infrastructure.Domains;
using Microservices.IDP.Infrastructure.Entities;
using Microservices.IDP.Infrastructure.ViewModels;

namespace Microservices.IDP.Infrastructure.Repositories.Interfaces;

public interface IPermissionRepository : IRepositoryBase<Permission, long>
{
    Task<IReadOnlyList<PermissionViewModel>> GetPermissionsByRole(string roleId);
    Task<PermissionViewModel?> CreatePermission(string roleId, PermissionAddModel model);
    Task DeletePermission(string roleId, string function, string command);
    Task UpdatePermissionsByRoleId(string roleId, IEnumerable<PermissionAddModel> permissionCollection);
    Task<IEnumerable<PermissionUserViewModel>> GetPermissionsByUser(User user);
}