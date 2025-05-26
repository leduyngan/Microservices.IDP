using Microservices.IDP.Common.Domains;
using Microservices.IDP.Entities;

namespace Microservices.IDP.Common.Repositories.Interfaces;

public interface IPermissionRepository : IRepositoryBase<Permission, long>
{
    Task<IEnumerable<Permission>> GetPermissionsByRole(string roleId, bool trackChanges = false);

    void UpdatePermissionsByRoleId(string roleId, IEnumerable<Permission> permissionCollection,
        bool trackChanges = false);
}