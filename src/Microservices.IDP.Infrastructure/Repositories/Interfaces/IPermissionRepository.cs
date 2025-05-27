using Microservices.IDP.Infrastructure.Domains;
using Microservices.IDP.Infrastructure.Entities;

namespace Microservices.IDP.Infrastructure.Repositories.Interfaces;

public interface IPermissionRepository : IRepositoryBase<Permission, long>
{
    Task<IEnumerable<Permission>> GetPermissionsByRole(string roleId, bool trackChanges = false);

    void UpdatePermissionsByRoleId(string roleId, IEnumerable<Permission> permissionCollection,
        bool trackChanges = false);
}